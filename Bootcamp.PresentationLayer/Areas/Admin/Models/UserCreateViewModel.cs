using System.Collections.Generic;

namespace Bootcamp.PresentationLayer.Areas.Admin.Models
{
    public class UserCreateViewModel
    {
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}