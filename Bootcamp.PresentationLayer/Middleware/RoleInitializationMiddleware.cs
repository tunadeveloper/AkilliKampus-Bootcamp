using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;

namespace Bootcamp.PresentationLayer.Middleware
{
    public class RoleInitializationMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleInitializationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roleNames = { "Admin", "User" };

                foreach (var roleName in roleNames)
                {
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        await roleManager.CreateAsync(new IdentityRole<int>(roleName));
                    }
                }

                // Admin kullanıcısı oluştur (eğer yoksa)
                var adminEmail = "admin@akillikampus.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);

                if (adminUser == null)
                {
                    var admin = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        NameSurname = "Admin User",
                        Gender = "Erkek"
                    };

                    var result = await userManager.CreateAsync(admin, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(admin, "Admin");
                    }
                }
            }

            await _next(context);
        }
    }
} 