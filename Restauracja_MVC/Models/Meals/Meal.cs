using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Meals
{
    public class Meal
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string PictureName { get; set; }
        public int Category { get; set; }
    }
}
