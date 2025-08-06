using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class CourseOutcomeValidator : AbstractValidator<CourseOutcome>
    {
        public CourseOutcomeValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage("Kazanım başlığı boş bırakılamaz.");
            RuleFor(x => x.CourseId).NotEmpty().WithMessage("Kurs seçilmelidir.");
        }
    }
}