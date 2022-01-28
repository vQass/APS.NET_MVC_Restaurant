using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Zamowienia
{
    public class UserDetails
    {
        public int UserID { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public int CityID { get; set; }
        public String Address { get; set; }
        public String Phone { get; set; }
    }
}
