using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Ingredients
{
    public class Ingredient
    {
        [DisplayName("Nazwa")]
        [Required(ErrorMessage = "Pole nazwa jest wymagane")]
        public string Name { get; set; }
    }
}
