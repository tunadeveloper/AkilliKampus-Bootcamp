using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        public CommentValidator()
        {
            RuleFor(x => x.Text)
                .NotEmpty().WithMessage("Yorum metni boş bırakılamaz.")
                .MaximumLength(500).WithMessage("Yorum metni en fazla 500 karakter olabilir.");

            RuleFor(x => x.ApplicationUserId)
                .GreaterThan(0).WithMessage("Kullanıcı seçilmelidir.");

            RuleFor(x => x.CourseId)
                .GreaterThan(0).WithMessage("Kurs seçilmelidir.");
        }
    }
} 