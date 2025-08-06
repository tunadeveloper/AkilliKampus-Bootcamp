namespace Bootcamp.PresentationLayer.Areas.Admin.Models
{
    public class UserEditViewModel
    {
        public int Id { get; set; }
        public string NameSurname { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string GeminiApiKey { get; set; }
        public List<string> SelectedRoles { get; set; }
    }
}