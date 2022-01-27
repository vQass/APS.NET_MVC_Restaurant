using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Users
{
    public class UserDetails : UserListItem
    {

        [DisplayName("Imię")]
        public string Name { get; set; }

        [DisplayName("Nazwisko")]
        public string Surname { get; set; }

        [DisplayName("Uprawnienia")]
        public string Role { get; set; }

        [DisplayName("Numer telefonu")]
        public string Phone { get; set; }

        [DisplayName("Miasto")]
        public string City { get; set; }

        [DisplayName("Adres")]
        public string Address { get; set; }

        [DisplayName("Data założenia")]
        public string CreateDate { get; set; }

        [DisplayName("Data modyfikacji")]
        public string UpdateDate { get; set; }
    }
}
