using Microsoft.AspNetCore.Mvc.Rendering;
using Restauracja_MVC.Models.Zamowienia;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Restauracja_MVC.ViewModels
{
    public class zamowienieViewModel
    {
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Imię musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public String NameOfUser { get; set; }
        [Required]
        [RegularExpression(@"^[A-Z][a-zA-Z]*$", ErrorMessage = "Nazwisko musi zaczynać się z dużej litery oraz zawierać tylko litery")]
        public String SurnameOfUser { get; set; }
        [Required]
        public int CityIDx { get; set; }
        [Required]
        public String Addressx { get; set; }
        [Required]
        [Phone]
        public String Phonex { get; set; }
        public List<Zamowienie> listaZamowien { get; set; }
        public List<SelectListItem> CityList { get; set; }
    }
}
