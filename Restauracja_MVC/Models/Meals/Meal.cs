using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Meals
{
    public class Meal
    {
        [DisplayName("Nazwa")]
        [Required(ErrorMessage = "Pole nazwa jest wymagane")]
        public string Name { get; set; }

        [DisplayName("Cena")]
        [Required(ErrorMessage = "Pole cena jest wymagane")]
        [Range(0.001, float.MaxValue, ErrorMessage = "Wartość pola {0} powinna być większa od {1}")]
        public float Price { get; set; }
        
        [DisplayName("Opis")]
        [Required(ErrorMessage = "Pole opis jest wymagane")]
        public string Description { get; set; }

        [DisplayName("Kategoria")]
        public Byte Category { get; set; }
    }
}
