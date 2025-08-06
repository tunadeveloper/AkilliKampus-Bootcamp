using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class ReferenceValidator : AbstractValidator<Reference>
    {
        public ReferenceValidator()
        {
            RuleFor(x => x.NameSurname)
                .NotEmpty().WithMessage("Ad Soyad boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Ad Soyad en fazla 100 karakter olabilir.");

            RuleFor(x => x.PositionName)
                .NotEmpty().WithMessage("Pozisyon boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Pozisyon en fazla 100 karakter olabilir.");

            RuleFor(x => x.Comment)
                .NotEmpty().WithMessage("Yorum boş bırakılamaz.")
                .MaximumLength(500).WithMessage("Yorum en fazla 500 karakter olabilir.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Resim URL boş bırakılamaz.")
                .MaximumLength(500).WithMessage("Resim URL en fazla 500 karakter olabilir.");
        }
    }
} 