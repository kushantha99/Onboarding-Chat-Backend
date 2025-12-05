using ConversationBackend.Models;
using ConversationBackend.Repositories.RepositoryInterfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ConversationBackend.Repositories
{
    public class AMLRepository : IAMLRepository
    {
        private IConfiguration Configuration { get; }
        private readonly string _ApiKey;
        private readonly string _ApiSecret;
        private readonly string _GroupId;
        private readonly string _Protocol;
        private readonly string _GatewayHost;
        private readonly string _GatewayUrl;
        private readonly string _Content;
        private readonly string _LogEnable;
        private readonly string _ProxyEnable;
        private readonly string _ProxyAddress;
        public AMLRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            _ApiKey = Configuration["AMLSettings:ApiKey"];
            _ApiSecret = Configuration["AMLSettings:ApiSecret"];
            _GroupId = Configuration["AMLSettings:GroupId"];
            _Protocol = Configuration["AMLSettings:Protocol"];
            _GatewayHost = Configuration["AMLSettings:GatewayHost"];
            _GatewayUrl = Configuration["AMLSettings:GatewayUrl"];
            _Content = Configuration["AMLSettings:Content"];
            _LogEnable = Configuration["AMLSettings:LogEnable"];
            _ProxyEnable = Configuration["AMLSettings:ProxyEnable"];
            _ProxyAddress = Configuration["AMLSettings:ProxyAddress"];

        }

        public BusinessPartnerAMLExtVM screeningRequest(BusinessPartnerAMLVM businessPartnerAMLVM)
        {
            string url = $"{_Protocol}{_GatewayHost}{_GatewayUrl}cases/screeningRequest";
            DateTime dateTime = DateTime.Now;

            var date = String.Format("{0:r}", dateTime);
            var requestData = businessPartnerAMLVM.IsIndividual
                ? CreateIndividualRequstData(businessPartnerAMLVM)
                : CreateOrganisationRequstData(businessPartnerAMLVM);

            UTF8Encoding encoding = new UTF8Encoding();
            byte[] dataByte = encoding.GetBytes(requestData);

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Method = "POST";
            webRequest.Headers.Add("Authorization", GenerateAuthenticationCode(requestData, date));
            webRequest.Headers.Add("Cache-Control", "no-cache");
            webRequest.ContentLength = requestData.Length;
            webRequest.Date = dateTime;
            webRequest.ContentType = "application/json";
            webRequest.ContentLength = dataByte.Length;

            if (_ProxyEnable == "1")
            {
                WebProxy proxy = new WebProxy(_ProxyAddress, true);
                webRequest.Proxy = proxy;
            }

            using (Stream dataStream = webRequest.GetRequestStream())
            {

                dataStream.Write(dataByte, 0, dataByte.Length);
            }
            using (HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse())
            {

                // Get the Response data
                Stream Answer = response.GetResponseStream();
                StreamReader _Answer = new StreamReader(Answer);
                string jsontxt = _Answer.ReadToEnd();

                var json = (JObject)JsonConvert.DeserializeObject(jsontxt);

                BusinessPartnerAMLExtVM businessPartnerAMLExtVM = new BusinessPartnerAMLExtVM();
                Console.WriteLine("RES : " + System.Text.Json.JsonSerializer.Serialize(businessPartnerAMLExtVM));

                businessPartnerAMLExtVM.CaseId = json["caseId"].Value<string>();
                businessPartnerAMLExtVM.Result = json["results"].ToString();

                return businessPartnerAMLExtVM;
            }
        }



        private string CreateIndividualRequstData(BusinessPartnerAMLVM businessPartnerAMLVM)
        {
            object[] secondaryFields =
            {
                new 
                { 
                    typeId = "SFCT_1", 
                    value = businessPartnerAMLVM.Gender 
                },
                new 
                { 
                    typeId = "SFCT_2", 
                    dateTimeValue = new 
                    { 
                        timelinePrecision = "ON", pointInTimePrecision = "DAY", utcDateTime = (long)(businessPartnerAMLVM.DateOfBirth?.Subtract(new DateTime(1970, 1, 1)))?.TotalMilliseconds
                    } 
                },
                new
                {
                    typeId = "SFCT_5",
                    value = businessPartnerAMLVM.Nationality
                }
            };
            string[] watchList = { "WATCHLIST" };
            var tempIndividualRequestVM = new
            {
                groupId = _GroupId,
                entityType = "INDIVIDUAL",
                caseScreeningState = new
                {
                    WATCHLIST = "ONGOING"
                },
                providerTypes = watchList,
                name = businessPartnerAMLVM.Name,
                secondaryFields
            };

            string jsonString = JsonConvert.SerializeObject(tempIndividualRequestVM);
            return jsonString;
        }


        private string CreateOrganisationRequstData(BusinessPartnerAMLVM businessPartnerAMLVM)
        {
            object[] secondaryFields = {
            new
            {
                typeId = "SFCT_6",
                value = businessPartnerAMLVM.RegisteredCountry
            }
            };

            string[] watchList = { "WATCHLIST" };

            var tempIndividualRequestVM = new
            {
                groupId = _GroupId,
                entityType = "ORGANISATION",
                caseScreeningState = new
                {
                    WATCHLIST = "ONGOING"
                },
                providerTypes = watchList,
                name = businessPartnerAMLVM.Name,
                secondaryFields
            };

            string jsonString = JsonConvert.SerializeObject(tempIndividualRequestVM);
            return jsonString;
        }


        private string GenerateAuthenticationCode(string content, string date)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("(request-target): post {0}cases/screeningRequest\n", _GatewayUrl));
            sb.Append(string.Format("host: {0}\n", _GatewayHost));
            sb.Append(string.Format("date: {0}\n", date));
            sb.Append(string.Format("content-type: {0}\n", _Content));
            sb.Append(string.Format("content-length: {0}\n", content.Length));
            sb.Append(content);

            var dataToSign = sb.ToString();

            var hmac = GenerateAuthHeader(dataToSign);

            string authorization = $"Signature keyId=\"{_ApiKey}\",algorithm=\"hmac-sha256\",headers=\"(request-target) host date content-type content-length\",signature=\"{hmac}\"";

            return authorization;
        }

        private string GenerateAuthHeader(string dataToSign)
        {
            var hash = CreateToken(dataToSign, _ApiSecret);
            return hash;
        }

        private string CreateToken(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using (var hmacsha256 = new HMACSHA256(keyByte))
            {
                byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
                return Convert.ToBase64String(hashmessage);
            }
        }
    }
}
