using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;


namespace Restauracja_MVC.Models.AccountViewModel
{
    public class UserEditCitiesViewModel
    {
        public UserEditViewModel User { get; set; }
        public List<SelectListItem> Cities { get; set; }
    }
}
