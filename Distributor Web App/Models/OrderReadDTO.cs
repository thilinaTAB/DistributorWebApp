using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace Distributor_Web_App.Models
{
    // DTO for reading order data from the API
    public class OrderReadDTO
    {
        // Maps to the 'orderId' property in the JSON response
        [JsonPropertyName("orderId")]
        public int OrderID { get; set; }

        // Maps to the 'orderDate' property in the JSON response
        [JsonPropertyName("orderDate")]
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }

        // Maps to the 'modelName' property in the JSON response
        [JsonPropertyName("modelName")]
        [Display(Name = "Blanket Model")]
        public string ModelName { get; set; } = string.Empty;

        // Maps to the 'quantity' property in the JSON response
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        // Maps to the 'price' property in the JSON response
        [JsonPropertyName("price")]
        [Display(Name = "Unit Price")]
        public decimal Price { get; set; }

        // Maps to the 'total' property in the JSON response
        [JsonPropertyName("total")]
        public decimal Total { get; set; }

        // Maps to the 'status' property in the JSON response
        [JsonPropertyName("status")]
        [Display(Name = "Order Status")]
        public string Status { get; set; } = string.Empty;
    }
}
