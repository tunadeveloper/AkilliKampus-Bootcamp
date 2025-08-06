using System.Collections.Generic;

namespace Bootcamp.PresentationLayer.Areas.Admin.Models
{
    public class UserWithRolesViewModel
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public List<string> Roles { get; set; }
    }
}