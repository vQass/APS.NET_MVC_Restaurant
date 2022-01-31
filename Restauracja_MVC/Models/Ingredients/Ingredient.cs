using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restauracja_MVC.Models.Ingredients
{
    public class Ingredient
    {
        [DisplayName("Nazwa")]
        [Required(ErrorMessage = "Pole nazwa jest wymagane")]
        public string Name { get; set; }
    }
}
