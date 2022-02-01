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
        [MaxLength(50)]
        [MinLength(1)]
        public String NameOfUser { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public String SurnameOfUser { get; set; }
        [Required]
        public int CityIDx { get; set; }
        [Required]
        [MaxLength(63)]
        public String Addressx { get; set; }
        [Required]
        [Phone]
        [MaxLength(15)]
        [MinLength(9)]
        public String Phonex { get; set; }
        public List<Zamowienie> listaZamowien { get; set; }
        public List<SelectListItem> CityList { get; set; }
    }
}
