
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Restauracja_MVC.Models.Cities
{
    public class CityCreate
    {
        [DisplayName("Nazwa miasta")]
        [Required]
        [MaxLength(50, ErrorMessage = "Nazwa miasta może zawierać maksymalnie 50 znaków")]
        public string Name { get; set; }
    }
}
