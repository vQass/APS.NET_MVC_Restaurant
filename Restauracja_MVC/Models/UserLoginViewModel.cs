using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models
{
    public class UserLoginViewModel
    {
        [Required(ErrorMessage = "Email jest wymagany")]
        [EmailAddress(ErrorMessage = "Niepoprawny format adresu email")]
        [StringLength(50, ErrorMessage = "Email musi zawierać między 3 a 50 znakami", MinimumLength = 3)]
        public string Email { get; set; }

        [DisplayName("Hasło")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Hasło jest wymagane")]
        [StringLength(50, ErrorMessage = "Hasło musi zawierać między 5 a 50 znakami", MinimumLength = 4)]
        public string Password { get; set; }
    }
}
