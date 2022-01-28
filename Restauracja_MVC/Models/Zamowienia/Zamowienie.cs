using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Restauracja_MVC.Models.Zamowienia
{
    public class Zamowienie
    {
        [Required]
        public int UserIDx { get; set; }
        public Int16 IDx { get; set; }
        [Required]
        public String NameOfMeal { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Imię musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public String NameOfUser { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Nazwisko musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public String SurnameOfUser { get; set; }
        [Required]
        [Range(1,3)]
        public int CityIDx { get; set; }
        [Required]
        public String Addressx { get; set; }
        [Required]
        [Phone]
        [StringLength(9, ErrorMessage = "Wprowadz poprawny numer telefonu")]
        [Range(000000000,999999999)]
        public String Phonex { get; set; }
        [Required]
        public float Pricex { get; set; }
        [Required]
        public string Descriptionx { get; set; }
        [Required]
        [StringLength(2)]
        public string Categoryx { get; set; }
        [Required]
        [Range(0,10, ErrorMessage = "10 to maksymalna ilosc zamówionego dania")]
        public int Amountx { get; set; }
    }
}
