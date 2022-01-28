using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Orders
{
    public class OrderListItem
    {
        public long OrderID { get; set; }
        public string Email { get; set; }
        public string OrderDate { get; set; }

    }
}
