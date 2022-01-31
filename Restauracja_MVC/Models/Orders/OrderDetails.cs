using System.Collections.Generic;
using System.ComponentModel;

namespace Restauracja_MVC.Models.Orders
{
    public class OrderDetails
    {
        public long OrderID { get; set; }
        public string Email { get; set; }

        [DisplayName("Imie")]
        public string Name { get; set; }

        [DisplayName("Nazwisko")]
        public string Surname { get; set; }

        [DisplayName("Miasto")]
        public string City { get; set; }

        [DisplayName("Adres")]
        public string Address { get; set; }

        [DisplayName("Numer telefonu")]
        public string Phone { get; set; }

        [DisplayName("Całkowita cena")]
        public string TotalPrice { get; set; }

        [DisplayName("Data zamówienia")]
        public string OrderDate { get; set; }
        [DisplayName("Zamówienie")]
        public Dictionary<string, byte> OrderList { get; set; }
    }
}
