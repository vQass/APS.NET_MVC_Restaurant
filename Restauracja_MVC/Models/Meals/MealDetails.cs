using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Meals
{
    public class MealDetails
    {
        public Int16 ID { get; set; }

        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [DisplayName("Cena")]
        public float Price { get; set; }
        
        [DisplayName("Opis")]
        public string Description { get; set; }

        [DisplayName("Kategoria")]
        public string Category { get; set; }
    }
}
