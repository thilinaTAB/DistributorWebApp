using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Distributor_Web_App.Models;
using System.Text.Json;

// The namespace reflects the new folder structure.
namespace Distributor_Web_App.Pages.CozyComfortInventory
{
    // The class name has been updated from IndexModel to MaterialsModel to match the file name.
    public class MaterialsModel : PageModel
    {
        private readonly HttpClient _httpClient;

        // The constructor name has been updated to match the new class name.
        public MaterialsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CozyComfortAPI");
        }

        public List<Material> Materials { get; set; } = new List<Material>();

        public async Task OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Material");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Materials = JsonSerializer.Deserialize<List<Material>>(content, options) ?? new List<Material>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve materials from the API. Check your API key and URL.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
