using System.ComponentModel.DataAnnotations;

namespace Restauracja_MVC.Models
{
    public class Skladniki
    {
        [Key]
        public int IDsk { get; set; }
        [Required]
        public string Namesk { get; set; }
        public bool isCheckedsk { get; set; }
    }

}
