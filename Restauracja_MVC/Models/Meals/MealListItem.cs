using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Meals
{
    public class MealListItem
    {
        public short ID { get; set; }

        [DisplayName("Nazwa")]
        public string Name { get; set; }

        [DisplayName("Cena")]
        public float Price { get; set; }

        [DisplayName("Kategoria")]
        public string Category { get; set; }
    }
}
