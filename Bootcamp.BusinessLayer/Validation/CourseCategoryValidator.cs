using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class CourseCategoryValidator : AbstractValidator<CourseCategory>
    {
        public CourseCategoryValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kategori adı boş bırakılamaz.");
        }
    }
}