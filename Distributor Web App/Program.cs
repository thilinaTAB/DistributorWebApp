using Distributor_Web_App.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();

// Configure a named HttpClient
builder.Services.AddHttpClient("CozyComfortAPI", httpClient =>
{
    // Configure the base URL for the API
    httpClient.BaseAddress = new Uri(builder.Configuration.GetValue<string>("CozyComfortApi:BaseUrl") ?? string.Empty);

    // Get the API key from the appsettings.json file
    var apiKey = builder.Configuration.GetValue<string>("CozyComfortApi:DistributorApiKey");

    // Check if the API key exists and add it to the request headers
    if (!string.IsNullOrEmpty(apiKey))
    {
        // Add the API key to the request headers. The key name "X-API-KEY" is a common convention
        // and should be configured to match what your API's ApiKeyAuth attribute expects.
        httpClient.DefaultRequestHeaders.Add("X-API-KEY", apiKey);
    }

    // CORRECT FIX: Use MediaTypeWithQualityHeaderValue to add the Accept header
    httpClient.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();
