using System.ComponentModel.DataAnnotations;

namespace Distributor_Web_App.Models
{
    public class Material
    {
        public int MaterialID { get; set; }

        [Display(Name = "Material Name")]
        public string MaterialName { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
