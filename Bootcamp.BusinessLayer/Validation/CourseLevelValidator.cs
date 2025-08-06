using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class CourseLevelValidator : AbstractValidator<CourseLevel>
    {
        public CourseLevelValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Seviye adı boş bırakılamaz.");
        }
    }
}