using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Users
{
    public class UserEdit
    {
        public long ID { get; set; }

        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        [StringLength(50, ErrorMessage = "Email musi zawierać między 3 a 50 znakami", MinimumLength = 3)]
        public string Email { get; set; }

        [DisplayName("Imię")]
        [StringLength(50, ErrorMessage = "Imię może zawierać maksymalnie 50 znaków")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Imię musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public string Name { get; set; }

        [DisplayName("Nazwisko")]
        [StringLength(50, ErrorMessage = "Nazwisko może zawierać maksymalnie 50 znaków")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Imię musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public string Surname { get; set; }

        [DisplayName("Uprawnienia")]
        public byte Role { get; set; }

        [DisplayName("Numer telefonu")]
        [Phone]
        public string Phone { get; set; }

        [DisplayName("Miasto")]
        public byte? City { get; set; }

        [DisplayName("Adres")]
        [StringLength(50, ErrorMessage = "Adres musi zawierać między 4 a 50 znakami", MinimumLength = 4)]
        public string Address { get; set; }

        public List<SelectListItem> Cities { get; set; }

        public List<SelectListItem> Roles { get; set; }

    }
}
