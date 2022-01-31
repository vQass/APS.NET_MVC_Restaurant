using System.ComponentModel;

namespace Restauracja_MVC.Models.Meals
{
    public class MealDetails
    {
        public short ID { get; set; }

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
