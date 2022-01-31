using System.Collections.Generic;
using System.ComponentModel;

namespace Restauracja_MVC.Models.Recipes
{
    public class RecipeEdit
    {
        [DisplayName("ID dania")]
        public short MealID { get; set; }

        [DisplayName("Nazwa dania")]
        public string MealName { get; set; }

        [DisplayName("Składniki w przepisie")]
        public Dictionary<short, string> IngredientsAdded { get; set; }

        [DisplayName("Dostępne składniki")]
        public Dictionary<short, string> IngredientsNotAdded { get; set; }

    }
}
