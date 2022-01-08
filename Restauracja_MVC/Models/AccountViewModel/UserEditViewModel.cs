using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.AccountViewModel
{
    public class UserEditViewModel
    {
        [DisplayName("Imię")]
        [StringLength(50, ErrorMessage = "Imię może zawierać maksymalnie 50 znaków")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Imię musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public string Name { get; set; }

        [DisplayName("Nazwisko")]
        [StringLength(50, ErrorMessage = "Nazwisko może zawierać maksymalnie 50 znaków")]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Imię musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public string Surname { get; set; }

        [DisplayName("Miasto")]
        public string City { get; set; }

        [DisplayName("Adres")]
        [StringLength(50, ErrorMessage = "Imię musi zawierać między 4 a 50 znakami", MinimumLength = 4)]
        public string Address { get; set; }

        [DisplayName("Numer telefonu")]
        [Phone]
        public string Phone { get; set; }
    }
}
