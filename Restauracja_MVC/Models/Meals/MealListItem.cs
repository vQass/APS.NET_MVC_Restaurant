using System.ComponentModel;

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
