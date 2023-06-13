using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ToDo.DomainModel.Classes;
using ToDo.DomainModel.Context;

namespace ToDo.WebAPI.Context
{
    /// <summary>
    /// identity context class.
    /// </summary>
    public class IdentityContext : IdentityDbContext<IdentityUser>
    {
        private const string AdminLogin = "admin";
        private const string AdminPassword = "S3cretP@ssw0rd";

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityContext"/> class.
        /// </summary>
        /// <param name="options">Options for Database Context.</param>
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Sets relations between entities and seeds data.
        /// </summary>
        /// <param name="builder"><see cref="ModelBuilder"/>.</param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            const string ADMIN_ID = "a18be9c0-aa65-4af8-bd17-00bd9344e575";
            const string ROLE_ID = ADMIN_ID;
            builder.Entity<IdentityRole>().HasData(new IdentityRole
            {
                Id = ROLE_ID,
                Name = AdminLogin,
                NormalizedName = AdminLogin,
            });

            var hasher = new PasswordHasher<IdentityUser>();
            builder.Entity<IdentityUser>().HasData(new IdentityUser
            {
                Id = ADMIN_ID,
                UserName = AdminLogin,
                NormalizedUserName = AdminLogin,
                Email = "admin@example.com",
                NormalizedEmail = "admin@example.com",
                EmailConfirmed = true,
                PasswordHash = hasher.HashPassword(null, AdminPassword),
                SecurityStamp = string.Empty,
            });

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                RoleId = ROLE_ID,
                UserId = ADMIN_ID,
            });
        }
    }
}
