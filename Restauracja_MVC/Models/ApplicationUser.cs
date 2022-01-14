using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models
{
    public class ApplicationUser
    {
        public string ID { get; set; } // change to int64 later
        public string Email { get; set; }
        public string Role { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Address { get; set; }

        public ApplicationUser()
        {

        }

        public ApplicationUser(string ID, string email, string role, string name, string surname, string phone, string city, string address)
        {
            this.ID = ID;
            Email = email;
            Role = role;
            Name = name;
            Surname = surname;
            Phone = phone;
            City = city;
            Address = address;
        }


    }
}
