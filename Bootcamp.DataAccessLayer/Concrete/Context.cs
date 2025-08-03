using Bootcamp.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bootcamp.DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=TUNA\\SQLEXPRESS;Database=AkilliKampusBootcamp;Integrated Security=True;TrustServerCertificate=True");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Course>()
                .HasMany(c => c.Users)
                .WithMany(u => u.Courses)
                .UsingEntity(j => j.ToTable("CourseUser"));
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseLevel> CourseLevels { get; set; }
        public DbSet<CourseOutcome> CourseOutcomes { get; set; }
        public DbSet<CourseVideo> CourseVideos { get; set; }
        public DbSet<Instructor> Instructors { get; set; }
        public DbSet<Progress> Progresses { get; set; }
        public DbSet<Comment> Comments { get; set; }
    }
}
