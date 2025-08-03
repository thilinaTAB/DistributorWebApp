using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Distributor_Web_App.Models;
using System.Text.Json;

namespace Distributor_Web_App.Pages.CozyComfortInventory
{
    public class BlanketModelsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public BlanketModelsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CozyComfortAPI");
        }

        public List<BlanketModel> BlanketModels { get; set; } = new List<BlanketModel>();

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/BlanketModel");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    BlanketModels = JsonSerializer.Deserialize<List<BlanketModel>>(content, options) ?? new List<BlanketModel>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve blanket models from the API.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
