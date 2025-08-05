using System.ComponentModel.DataAnnotations;

namespace Distributor_Web_App.Models
{

    public class PlaceOrderDTO
    {
        [Required(ErrorMessage = "Please select a blanket model.")]
        public int ModelID { get; set; }
        [Required(ErrorMessage = "Please enter a quantity.")]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1.")]
        public int Quantity { get; set; }
    }
}
