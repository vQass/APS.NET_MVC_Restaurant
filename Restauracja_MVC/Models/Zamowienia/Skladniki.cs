using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models
{
    public class Skladniki
    {
        [Key]
        public int IDsk { get; set; }
        [Required]
        public string Namesk { get; set; }
        public bool isCheckedsk { get; set; }
    }

}
