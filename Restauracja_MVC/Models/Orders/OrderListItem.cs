using System.ComponentModel;

namespace Restauracja_MVC.Models.Orders
{
    public class OrderListItem
    {
        [DisplayName("ID zamówienia")]
        public long OrderID { get; set; }
        public string Email { get; set; }
        [DisplayName("Data zamówienia")]
        public string OrderDate { get; set; }

    }
}
