using System.ComponentModel.DataAnnotations;

namespace Restauracja_MVC.Models.Orders
{
    public class Orders
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int MealID { get; set; }
        [Required]
        public int Amount { get; set; }
    }
}
