using Bootcamp.EntityLayer.Concrete;
using FluentValidation;

namespace Bootcamp.BusinessLayer.Validation
{
    public class CourseValidator : AbstractValidator<Course>
    {
        public CourseValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Kurs adı boş bırakılamaz.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Açıklama boş bırakılamaz.");
            RuleFor(x => x.ThumbnailUrl).NotEmpty().WithMessage("Kapak görseli boş bırakılamaz.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Kategori seçilmelidir.");
            RuleFor(x => x.CourseLevelId).NotEmpty().WithMessage("Seviye seçilmelidir.");
            RuleFor(x => x.InstructorId).NotEmpty().WithMessage("Eğitmen seçilmelidir.");
        }
    }
}