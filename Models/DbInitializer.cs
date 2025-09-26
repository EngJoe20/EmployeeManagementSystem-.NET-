using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class DbInitializer
    {
        public static async Task Seed(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // Roles
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));

            // Admin user
            var adminEmail = "admin@company.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Normal user
            var userEmail = "user@company.com";
            if (await userManager.FindByEmailAsync(userEmail) == null)
            {
                var normalUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Normal",
                    LastName = "User",
                    EmailConfirmed = true
                };
                var result = await userManager.CreateAsync(normalUser, "User@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(normalUser, "User");
                }
            }

            // Departments
            if (!dbContext.Departments.Any())
            {
                var departments = new List<Department>
                {
                    new Department { Name = "Information Technology", Description = "IT Department", IsActive = true },
                    new Department { Name = "Human Resources", Description = "HR Department", IsActive = true },
                    new Department { Name = "Finance", Description = "Finance Department", IsActive = true },
                    new Department { Name = "Marketing", Description = "Marketing Department", IsActive = true }
                };
                dbContext.Departments.AddRange(departments);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
