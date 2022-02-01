using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Restauracja_MVC.Models.Cities
{
    public class CityListItem
    {
        [DisplayName("ID miasta")]
        public byte ID { get; set; }
        
        [DisplayName("Nazwa miasta")]
        public string Name { get; set; }
    }
}
