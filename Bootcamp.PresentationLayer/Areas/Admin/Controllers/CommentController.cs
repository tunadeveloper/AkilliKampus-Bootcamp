using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.EntityLayer.Concrete;
using Bootcamp.PresentationLayer.Areas.Admin.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Bootcamp.PresentationLayer.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ICourseService _courseService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IValidator<Comment> _validator;

        public CommentController(ICommentService commentService, ICourseService courseService, 
            UserManager<ApplicationUser> userManager, IValidator<Comment> validator)
        {
            _commentService = commentService;
            _courseService = courseService;
            _userManager = userManager;
            _validator = validator;
        }

        public IActionResult Index()
        {
            var comments = _commentService.GetListBL();
            return View(comments);
        }

        public IActionResult Details(int id)
        {
            var comment = _commentService.GetByIdBL(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        public IActionResult Create()
        {
            ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
            ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "NameSurname");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Comment comment)
        {
            var validationResult = _validator.Validate(comment);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
                ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "NameSurname");
                return View(comment);
            }

            comment.CreatedAt = DateTime.Now;
            _commentService.InsertBL(comment);
            TempData["Success"] = "Yorum başarıyla eklendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var comment = _commentService.GetByIdBL(id);
            if (comment == null)
            {
                return NotFound();
            }

            ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
            ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "NameSurname");
            return View(comment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            var validationResult = _validator.Validate(comment);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
                ViewBag.Courses = new SelectList(_courseService.GetListBL(), "Id", "Name");
                ViewBag.Users = new SelectList(_userManager.Users.ToList(), "Id", "NameSurname");
                return View(comment);
            }

            _commentService.UpdateBL(comment);
            TempData["Success"] = "Yorum başarıyla güncellendi.";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var comment = _commentService.GetByIdBL(id);
            if (comment == null)
            {
                return NotFound();
            }
            return View(comment);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var comment = _commentService.GetByIdBL(id);
            if (comment == null)
            {
                return NotFound();
            }

            _commentService.DeleteBL(comment);
            TempData["Success"] = "Yorum başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
} 