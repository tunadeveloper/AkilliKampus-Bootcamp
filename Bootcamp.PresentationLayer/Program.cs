using Bootcamp.BusinessLayer.Abstract;
using Bootcamp.BusinessLayer.Concrete;
using Bootcamp.BusinessLayer.Validation;
using Bootcamp.DataAccessLayer.Abstract;
using Bootcamp.DataAccessLayer.Concrete;
using Bootcamp.DataAccessLayer.EntityFramework;
using Bootcamp.EntityLayer.Concrete;
using Bootcamp.PresentationLayer.Middleware;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>();

// FluentValidation servisleri
builder.Services.AddScoped<IValidator<Course>, CourseValidator>();
builder.Services.AddScoped<IValidator<CourseOutcome>, CourseOutcomeValidator>();
builder.Services.AddScoped<IValidator<Instructor>, InstructorValidator>();
builder.Services.AddScoped<IValidator<CourseLevel>, CourseLevelValidator>();
builder.Services.AddScoped<IValidator<CourseCategory>, CourseCategoryValidator>();
builder.Services.AddScoped<IValidator<Comment>, CommentValidator>();

builder.Services.AddScoped<ICommentDal, EfCommentDal>();
builder.Services.AddScoped<ICommentService, CommentManager>();

builder.Services.AddScoped<IApplicationUserDal, EfApplicationUserDal>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserManager>();

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

builder.Services.AddScoped<ICourseLevelDal, EfCourseLevelDal>();
builder.Services.AddScoped<ICourseLevelService, CourseLevelManager>();

builder.Services.AddScoped<ICourseOutcomeDal, EfCourseOutcomeDal>();
builder.Services.AddScoped<ICourseOutcomeService, CourseOutcomeManager>();

// FAQ Services
builder.Services.AddScoped<IValidator<FAQ>, FAQValidator>();
builder.Services.AddScoped<IFAQDal, EfFAQDal>();
builder.Services.AddScoped<IFAQService, FAQManager>();

// Reference Services
builder.Services.AddScoped<IValidator<Reference>, ReferenceValidator>();
builder.Services.AddScoped<IReferenceDal, EfReferenceDal>();
builder.Services.AddScoped<IReferenceService, ReferenceManager>();

// SiteSetting Services
builder.Services.AddScoped<IValidator<SiteSetting>, SiteSettingValidator>();
builder.Services.AddScoped<ISiteSettingDal, EfSiteSettingDal>();
builder.Services.AddScoped<ISiteSettingService, SiteSettingManager>();

// Gemini API servisleri
builder.Services.AddHttpClient("GeminiClient", client =>
{
    client.Timeout = TimeSpan.FromMinutes(5); // 5 dakika timeout
    client.DefaultRequestHeaders.Add("User-Agent", "AkilliKampus-Bootcamp/1.0");
});
builder.Services.AddScoped<IGeminiService>(provider =>
{
    var httpClient = provider.GetRequiredService<IHttpClientFactory>().CreateClient("GeminiClient");
    var configuration = provider.GetRequiredService<IConfiguration>();
    var apiKey = configuration["GeminiApi:ApiKey"] ?? "YOUR_GEMINI_API_KEY";
    var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
    var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();
    return new GeminiManager(httpClient, apiKey, userManager, httpContextAccessor);
});

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

// Rol oluşturma middleware'i
app.UseMiddleware<RoleInitializationMiddleware>();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Statistics}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
