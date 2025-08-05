using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Distributor_Web_App.Models;
using System.Text.Json;
using System.Net.Http.Json;

namespace Distributor_Web_App.Pages.Distributor
{
    public class PlaceOrderModel : PageModel
    {
        private readonly HttpClient _httpClient;

        [BindProperty]
        public PlaceOrderDTO Order { get; set; } = new PlaceOrderDTO();

        public List<BlanketModel> AvailableBlanketModels { get; set; } = new List<BlanketModel>();

        public SelectList BlanketModelsDropdown { get; set; }

        public PlaceOrderModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CozyComfortAPI");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/BlanketModel");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    AvailableBlanketModels = JsonSerializer.Deserialize<List<BlanketModel>>(content, options) ?? new List<BlanketModel>();

                    BlanketModelsDropdown = new SelectList(AvailableBlanketModels, "ModelID", "ModelName");
                    return Page();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve blanket models from the API.");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await OnGetAsync();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/Order", Order);

                if (response.IsSuccessStatusCode)
                {
                    TempData["SuccessMessage"] = "Order placed successfully! The manufacturer has received your order.";
                    return RedirectToPage();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ModelState.AddModelError(string.Empty, $"Error from API: {response.StatusCode} - {errorContent}");
                    return Page();
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
                return Page();
            }
        }
    }
}
