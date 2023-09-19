using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RaxRot.Blog.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) 
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);

            //Seed Roles(Admin,User,SuperAdmin)
            var adminRoleId = "3cc5d533-3694-4513-b578-05c28383c0ab";
            var superAdminRoleId = "08a1679d-81df-4744-b26e-f7d6efea2d2b";
            var userRoleId = "27e8f186-00e5-4a4b-8224-29997e6362c7";


            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Name= "Admin",
                    NormalizedName="Admin",
                    Id=adminRoleId,
                    ConcurrencyStamp=adminRoleId
                },
                new IdentityRole
                {
                    Name="SuperAdmin",
                    NormalizedName="SuperAdmin",
                    Id=superAdminRoleId,
                    ConcurrencyStamp=superAdminRoleId
                },
                new IdentityRole
                {
                    Name="User",
                    NormalizedName="User",
                    Id=userRoleId,
                    ConcurrencyStamp=userRoleId
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            //Seed SuperAdmin
            var superAdminId = "e5274c5f-ec86-4c3d-b4bc-3f5225d59cb3";
            var superAdminUser = new IdentityUser
            {
                UserName = "superadmin@raxrot.com",
                Email = "superadmin@raxrot.com",
                NormalizedEmail = "superadmin@raxrot.com",
                NormalizedUserName = "superadmin@raxrot.com",
                Id = superAdminId

            };
            superAdminUser.PasswordHash = new PasswordHasher<IdentityUser>()
                .HashPassword(superAdminUser, "123456!Aa");

            builder.Entity<IdentityUser>().HasData(superAdminUser);

            //Add all roles to superAdmin
           var superAdminRoles = new List<IdentityUserRole<string>>
            {
                new IdentityUserRole<string>
                {
                   RoleId= adminRoleId,
                   UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                   RoleId= superAdminRoleId,
                   UserId=superAdminId
                },
                new IdentityUserRole<string>
                {
                   RoleId= userRoleId,
                   UserId=superAdminId
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(superAdminRoles);
        }
    }
}
