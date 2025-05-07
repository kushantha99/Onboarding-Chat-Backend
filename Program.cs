using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ConversationBackend.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<MongoDbService>();
//builder.Services.AddTransient<DataSeeder>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(); // Enable Swagger JSON endpoint
    app.UseSwaggerUI(c => // Enable Swagger UI
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "ConversationBackend API v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at root (e.g., http://localhost:5149)
    });
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

// Seed initial data
using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider; // Correct variable name
    var mongoDbService = serviceProvider.GetRequiredService<MongoDbService>();
    //var dataSeeder = new DataSeeder(mongoDbService);
    //await dataSeeder.SeedData();
}

await app.RunAsync();