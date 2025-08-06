using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class SiteSettingValidator : AbstractValidator<SiteSetting>
    {
        public SiteSettingValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Anahtar boş bırakılamaz.")
                .MaximumLength(100).WithMessage("Anahtar en fazla 100 karakter olabilir.");

            RuleFor(x => x.Value)
                .NotEmpty().WithMessage("Değer boş bırakılamaz.")
                .MaximumLength(1000).WithMessage("Değer en fazla 1000 karakter olabilir.");

            RuleFor(x => x.Group)
                .NotEmpty().WithMessage("Grup boş bırakılamaz.")
                .MaximumLength(50).WithMessage("Grup en fazla 50 karakter olabilir.");
        }
    }
} 