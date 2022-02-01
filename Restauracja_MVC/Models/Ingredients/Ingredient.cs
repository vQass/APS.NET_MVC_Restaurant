using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restauracja_MVC.Models.Ingredients
{
    public class Ingredient
    {
        [DisplayName("Nazwa")]
        [Required(ErrorMessage = "Pole nazwa jest wymagane")]
        [MaxLength(50)]
        [MinLength(1)]
        public string Name { get; set; }
    }
}
