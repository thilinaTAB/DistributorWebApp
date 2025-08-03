using System.ComponentModel.DataAnnotations;

namespace Distributor_Web_App.Models
{
    public class DistributorStock
    {
        public int DistributorStockID { get; set; }
        public int Inventory { get; set; }
        public int DistributorID { get; set; }
        public int ModelID { get; set; }
        public BlanketModel BlanketModel { get; set; } = new BlanketModel();
    }

    public class DistributorStockWriteDTO
    {
        [Required]
        [Display(Name = "Inventory")]
        public int Inventory { get; set; }

        public int DistributorID { get; set; }

        [Required]
        [Display(Name = "Model")]
        public int ModelID { get; set; }
    }
}
