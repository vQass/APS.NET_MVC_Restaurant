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
        public string Name { get; set; }

        [DisplayName("Cena")]
        [Required(ErrorMessage = "Pole cena jest wymagane")]
        [Range(0.01, 500.01, ErrorMessage = "Wartość pola {0} powinna być większa od {1} i mniejsza od {2}")]
        public float Price { get; set; }

        [DisplayName("Opis")]
        [Required(ErrorMessage = "Pole opis jest wymagane")]
        public string Description { get; set; }

        [DisplayName("Kategoria")]
        public byte Category { get; set; }

        public List<SelectListItem> MealsCategories { get; set; }
    }
}
