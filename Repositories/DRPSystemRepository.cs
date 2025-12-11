using ConversationBackend.Models.DRP;
using ConversationBackend.Repositories.RepositoryInterfaces;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;

namespace ConversationBackend.Repositories
{
    public class DRPSystemRepository : IDRPSystemRepository
    {
        private readonly string ApiUrl;
        private readonly string ClientId;
        private readonly string ClientSecret;
        private readonly string UserName;
        private readonly string Password;
        private readonly string LogEnable;
        public DRPSystemRepository(IConfiguration configuration) 
        {
            ApiUrl = configuration["DRPSettings:ApiUrl"];
            ClientId = configuration["DRPSettings:ClientId"];
            ClientSecret = configuration["DRPSettings:ClientSecret"];
            UserName = configuration["DRPSettings:UserName"];
            Password = configuration["DRPSettings:Password"];
            LogEnable = configuration["DRPSettings:LogEnable"];
        }

        public async Task<DRPNicResponseVM> GetDRPResponse(string nicNo)
        {
            DRPLoginResponseVM dRPLoginResponseVM    = new DRPLoginResponseVM();
            DRPLoginResponseVM newdRPLoginResponseVM = new DRPLoginResponseVM();
            DRPAgreementsUserDataVM agreementIdList  = new DRPAgreementsUserDataVM();
            DRPSubscriptionRequestVM subscriptionIdList = new DRPSubscriptionRequestVM();
            DRPNicRequestBodyVM dRPNicRequestBodyVM = new DRPNicRequestBodyVM();
            DRPNicResponseVM dRPNicResponseVM = new DRPNicResponseVM();
            try
            {
                dRPLoginResponseVM = await GetLoginDetails();

                if (dRPLoginResponseVM.RefreshToken == null || dRPLoginResponseVM.AccessToken == null)
                {
                    throw new Exception($"Refresh token is null: {dRPLoginResponseVM}");
                }

                newdRPLoginResponseVM = await GetAccessTokenUsingRefreshToken(dRPLoginResponseVM.RefreshToken, dRPLoginResponseVM.AccessToken);

                if (newdRPLoginResponseVM.AccessToken == null)
                {
                    throw new Exception($"Access token is null: {newdRPLoginResponseVM}");
                }

                agreementIdList = await GetAgreementId(newdRPLoginResponseVM.AccessToken);

                if (agreementIdList.Data[0] == null)
                {
                    throw new Exception($"Agreement id is null: {agreementIdList}");
                }

                subscriptionIdList = await GetSubscriptionId(newdRPLoginResponseVM.AccessToken, agreementIdList);

                if (subscriptionIdList?.Subscriptions == null || subscriptionIdList.Subscriptions.Count == 0 || subscriptionIdList.Subscriptions[0].SubscriptionId < 0)
                {
                    throw new Exception($"Subscription id is null: {subscriptionIdList}");
                }
                
                dRPNicRequestBodyVM.SubscriptionId = subscriptionIdList.Subscriptions[0].SubscriptionId;
                dRPNicRequestBodyVM.Keys = new DRPKeysVM();
                dRPNicRequestBodyVM.Keys.Nic = nicNo;
                dRPNicRequestBodyVM.Consent = true;

                dRPNicResponseVM = await GetNicInfomation(newdRPLoginResponseVM.AccessToken, dRPNicRequestBodyVM);
                Console.WriteLine("RESPONSE NIC DATA: " + System.Text.Json.JsonSerializer.Serialize(dRPNicResponseVM.Idinformation[0].FrontImage));
                return dRPNicResponseVM;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // (1) Get Access and Refresh tokens
        private async Task<DRPLoginResponseVM> GetLoginDetails()
        {
            try
            {
                using var client = new HttpClient();
                var url = $"{ApiUrl}/v1/public/oauth/org/token";

                DRPLoginRequestVM dRPLoginRequestVM = new DRPLoginRequestVM();
                dRPLoginRequestVM.Username = UserName;
                dRPLoginRequestVM.Password = Password;
                dRPLoginRequestVM.ClientId = ClientId;
                dRPLoginRequestVM.ClientSecret = ClientSecret;

                var requestJson = System.Text.Json.JsonSerializer.Serialize(dRPLoginRequestVM);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // Create request to add custom headers
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                // Add custom header
                request.Headers.Add("uuid", "123");


                var response = await client.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Token API failed: {responseJson}");
                }
                return System.Text.Json.JsonSerializer.Deserialize<DRPLoginResponseVM>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch
            {
                throw;
            }
        }

        // (2) Get new Access Token from refresh token
        private async Task<DRPLoginResponseVM> GetAccessTokenUsingRefreshToken(string refreshToken, string accessToken)
        {
            try
            {
                using var client = new HttpClient();
                var url = $"{ApiUrl}/v1/public/oauth/refresh-token";

                var requestBody = new
                {
                    refreshToken = refreshToken,
                };
                var requestJson = System.Text.Json.JsonSerializer.Serialize(requestBody);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Add("uuid", "123");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if(!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Refresh token API failed: {responseJson}");
                }
                return System.Text.Json.JsonSerializer.Deserialize<DRPLoginResponseVM>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true});
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // (3) Get Agreement Id using latest Access token
        private async Task<DRPAgreementsUserDataVM> GetAgreementId(string accessToken)
        {
            try
            {

                using var client = new HttpClient();
                var url = $"{ApiUrl}/v1/admin-service/org/agreements/user";
                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Add("uuid", "123");
                var response = await client.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if(!response.IsSuccessStatusCode) 
                {
                    throw new Exception($"Agreement API failed: {responseJson}");
                }

                return System.Text.Json.JsonSerializer.Deserialize<DRPAgreementsUserDataVM>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch(Exception ex)
            {
                throw;
            }

        }


        // (4) Get Subscription Id using Agreement Id
        private async Task<DRPSubscriptionRequestVM> GetSubscriptionId(string accessToken, DRPAgreementsUserDataVM agreementIdList)
        {
            try
            {
                using var client = new HttpClient();
                var url = $"{ApiUrl}/v1/admin-service/org/subscription/agreement/{agreementIdList.Data[0].AgreementId}";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                request.Headers.Add("uuid", "123");

                var response = await client.SendAsync(request);

                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Subscription API failed: {responseJson}");
                }

                return System.Text.Json.JsonSerializer.Deserialize<DRPSubscriptionRequestVM>(responseJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        // (5) Retrieve NIC info using subscriptionId
        private async Task<DRPNicResponseVM> GetNicInfomation(string accessToken , DRPNicRequestBodyVM dRPNicRequestBodyVM)
        {
            try
            {
                using var client = new HttpClient();
                var url = $"{ApiUrl}/v1/request-handler-service/org/request";
                var requestJson = JsonConvert.SerializeObject(dRPNicRequestBodyVM);

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };
                request.Headers.Add("uuid", "123");
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                var response = await client.SendAsync(request);
                var responseJson = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception($"Get NIC Info API failed: {responseJson}");
                }
                return JsonConvert.DeserializeObject<DRPNicResponseVM>(responseJson);
            }
            catch 
            {
                throw;
            }
        }
    }
}
