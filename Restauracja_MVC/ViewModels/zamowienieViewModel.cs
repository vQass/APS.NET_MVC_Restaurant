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
        public String NameOfUser { get; set; }
        [Required]
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
