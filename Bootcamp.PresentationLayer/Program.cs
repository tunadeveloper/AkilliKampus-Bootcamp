using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.BusinessLayer.Concrete;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.EntityFramework;
using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>();

builder.Services.AddScoped<ICommentDal, EfCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddScoped<ICourseCategoryDal, EfCourseCategoryDal>();
builder.Services.AddScoped<ICourseCategoryService, CourseCategoryManager>();

builder.Services.AddScoped<IInstructorDal, EfInstructorDal>();
builder.Services.AddScoped<IInstructorService, InstructorManager>();

builder.Services.AddScoped<ICourseDal, EfCourseDal>();
builder.Services.AddScoped<ICourseService, CourseManager>();

builder.Services.AddScoped<IProgressDal, EfProgressDal>();
builder.Services.AddScoped<IProgressService, ProgressManager>();

builder.Services.AddScoped<ICourseVideoDal, EfCourseVideoDal>();
builder.Services.AddScoped<ICourseVideoService, CourseVideoManager>();

builder.Services.AddScoped<ICourseEnrollmentDal, EfCourseEnrollmentDal>();
builder.Services.AddScoped<ICourseEnrollmentService, CourseEnrollmentManager>();

builder.Services.AddScoped<IVideoCompletionDal, EfVideoCompletionDal>();
builder.Services.AddScoped<IVideoCompletionService, VideoCompletionManager>();

builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
    .AddEntityFrameworkStores<Context>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
