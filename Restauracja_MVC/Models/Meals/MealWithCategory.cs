using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Meals
{
    public class MealWithCategory : Meal
    {
        [DisplayName("Kategoria")]
        public string CategoryName { get; set; }
    }
}
