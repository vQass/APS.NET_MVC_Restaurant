using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Restauracja_MVC.Models.Meals
{
    public class MealCreate
    {
        [DisplayName("Nazwa")]
        [Required(ErrorMessage = "Pole nazwa jest wymagane")]
        [MaxLength(50)]
        [MinLength(1)]
        public string Name { get; set; }

        [DisplayName("Cena")]
        [Required(ErrorMessage = "Pole cena jest wymagane")]
        [Range(0.01, 500.01, ErrorMessage = "Wartość pola {0} powinna być większa od {1}, a mniejsza od 500")]
        public float Price { get; set; }

        [DisplayName("Opis")]
        [MaxLength(8000)]
        [MinLength(1)]
        [Required(ErrorMessage = "Pole opis jest wymagane")]
        public string Description { get; set; }

        [DisplayName("Kategoria")]
        public byte Category { get; set; }

        public List<SelectListItem> MealsCategories { get; set; }
    }
}
