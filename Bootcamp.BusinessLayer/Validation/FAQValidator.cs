using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class FAQValidator : AbstractValidator<FAQ>
    {
        public FAQValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Başlık boş bırakılamaz.")
                .MaximumLength(200).WithMessage("Başlık en fazla 200 karakter olabilir.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Açıklama boş bırakılamaz.")
                .MaximumLength(1000).WithMessage("Açıklama en fazla 1000 karakter olabilir.");

            RuleFor(x => x.Icon)
                .NotEmpty().WithMessage("İkon boş bırakılamaz.")
                .MaximumLength(50).WithMessage("İkon en fazla 50 karakter olabilir.");
        }
    }
} 