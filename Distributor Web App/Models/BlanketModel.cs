using System.ComponentModel.DataAnnotations;

namespace Distributor_Web_App.Models
{
    public class BlanketModel
    {
        public int ModelID { get; set; }

        [Display(Name = "Model Name")]
        public string ModelName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        [Display(Name = "Available Stock")]
        public int Stock { get; set; }
        public int MaterialID { get; set; }
        [Display(Name = "Material Name")]
        public string MaterialName { get; set; } = string.Empty;
        [Display(Name = "Material Description")]
        public string MaterialDescription { get; set; } = string.Empty;
    }
}
