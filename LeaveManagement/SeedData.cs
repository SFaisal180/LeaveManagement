using LeaveManagement.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeaveManagement
{
    public static class SeedData
    {
       // public static void Seed(UserManager<Employee> userManager,
        public static void Seed(UserManager<IdentityUser> userManager,
           RoleManager<IdentityRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager);
        }

       // private static void SeedUsers(UserManager<Employee> userManager)
        private static void SeedUsers(UserManager<IdentityUser> userManager)
        {
            //   var users = userManager.GetUsersInRoleAsync("Employee").Result;

            if (userManager.FindByNameAsync("admin@localhost").Result == null)
            {
                var user = new Employee
                {
                    UserName = "admin@localhost",
                    Email = "admin@localhost"
                };
                var result = userManager.CreateAsync(user, "Password@12").Result; //User Created 
                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "Administrator").Wait();  // Assigned the above created user to a Role
                }
            }
        }

        private static void SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Administrator"
                };
                // roleManager.CreateAsync(role);
                var result = roleManager.CreateAsync(role).Result;
            }

            if (!roleManager.RoleExistsAsync("Employee").Result)
            {
                var role = new IdentityRole
                {
                    Name = "Employee"
                };
                //   roleManager.CreateAsync(role);
                var result = roleManager.CreateAsync(role).Result;
            }
        }
    }
}
