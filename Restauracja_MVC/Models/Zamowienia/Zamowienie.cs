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
        public float Pricex { get; set; }
        [Required]
        public string Descriptionx { get; set; }
        [Required]
        public string Categoryx { get; set; }
        [Required]
        public int Amountx { get; set; }
    }
}
