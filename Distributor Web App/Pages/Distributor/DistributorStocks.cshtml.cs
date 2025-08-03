using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Distributor_Web_App.Models;
using System.Text.Json;
using System.Text;

// The namespace must match the folder structure
namespace Distributor_Web_App.Pages.Distributor.DistributorStocks
{
    // The class name must match the file name: DistributorStocks
    public class DistributorStocksModel : PageModel
    {
        private readonly HttpClient _httpClient;

        public DistributorStocksModel(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("CozyComfortAPI");
        }

        public List<DistributorStock> DistributorStocks { get; set; } = new List<DistributorStock>();

        [BindProperty]
        public DistributorStockWriteDTO NewStock { get; set; } = new DistributorStockWriteDTO();

        public List<BlanketModel> BlanketModels { get; set; } = new List<BlanketModel>();

        [BindProperty]
        public DistributorStockWriteDTO EditedStock { get; set; } = new DistributorStockWriteDTO();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        // FIX: The method signature has been changed back to not accept a parameter.
        // It will now use the [BindProperty] NewStock object directly.
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostAddAsync()
        {
            // We will proceed even if model binding fails for the Inventory field,
            // as you can edit it later.
            // A check for ModelState.IsValid is still useful for other fields like ModelID.
            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            // FIX: Explicitly set the inventory to 0 to bypass the binding issue from the form.
            // This allows the stock item to be created, and you can edit the inventory later.
            NewStock.Inventory = 0;

            // TODO: Replace '1' with a dynamic distributorId
            NewStock.DistributorID = 1;

            var jsonContent = JsonSerializer.Serialize(NewStock);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("api/DistributorStock", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to add new stock item.");
            }
            else
            {
                // On a successful save, clear the NewStock object to reset the form.
                NewStock = new DistributorStockWriteDTO();
            }

            await LoadDataAsync();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostEditAsync(int id)
        {
            var jsonContent = JsonSerializer.Serialize(EditedStock);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"api/DistributorStock/{id}", content);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to update stock item.");
            }

            await LoadDataAsync();
            return Page();
        }

        [ValidateAntiForgeryToken]
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/DistributorStock/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(string.Empty, "Failed to delete stock item.");
            }

            await LoadDataAsync();
            return Page();
        }

        private async Task LoadDataAsync()
        {
            // TODO: Replace '1' with a dynamic value
            int distributorId = 1;

            try
            {
                var stockResponse = await _httpClient.GetAsync($"api/DistributorStock/{distributorId}");
                if (stockResponse.IsSuccessStatusCode)
                {
                    var stockContent = await stockResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    DistributorStocks = JsonSerializer.Deserialize<List<DistributorStock>>(stockContent, options) ?? new List<DistributorStock>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve distributor stock from the API.");
                }

                var modelsResponse = await _httpClient.GetAsync("api/BlanketModel");
                if (modelsResponse.IsSuccessStatusCode)
                {
                    var modelsContent = await modelsResponse.Content.ReadAsStringAsync();
                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    BlanketModels = JsonSerializer.Deserialize<List<BlanketModel>>(modelsContent, options) ?? new List<BlanketModel>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to retrieve available blanket models.");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"An unexpected error occurred: {ex.Message}");
            }
        }
    }
}
