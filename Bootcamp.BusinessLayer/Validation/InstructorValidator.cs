using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class InstructorValidator : AbstractValidator<Instructor>
    {
        public InstructorValidator()
        {
            RuleFor(x => x.NameSurname).NotEmpty().WithMessage("Ad Soyad boş bırakılamaz.");
            RuleFor(x => x.Position).NotEmpty().WithMessage("Pozisyon boş bırakılamaz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz.");
            RuleFor(x => x.ImageUrl).NotEmpty().WithMessage("Resim URL boş bırakılamaz.");
        }
    }
}