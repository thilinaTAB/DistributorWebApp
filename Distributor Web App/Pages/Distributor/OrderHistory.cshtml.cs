using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Distributor_Web_App.Models;
using System.Text.Json;

namespace Distributor_Web_App.Pages
{
    public class OrderHistoryModel : PageModel
    {
        private static readonly HttpClient client = new HttpClient();
        public List<OrderReadDTO> Orders { get; set; } = new List<OrderReadDTO>();
        public string ErrorMessage { get; set; } = string.Empty;
        public async Task OnGetAsync()
        {
            var distributorId = 1;
            var apiUrl = $"https://localhost:7175/api/Order/1";

            try
            {
                var response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var jsonString = await response.Content.ReadAsStringAsync();

                    Orders = JsonSerializer.Deserialize<List<OrderReadDTO>>(jsonString, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }) ?? new List<OrderReadDTO>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    ErrorMessage = $"Error fetching orders: {response.StatusCode} - {errorContent}";
                    Orders = new List<OrderReadDTO>(); 
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An unexpected error occurred: {ex.Message}";
                Orders = new List<OrderReadDTO>(); 
            }
        }
    }
}
