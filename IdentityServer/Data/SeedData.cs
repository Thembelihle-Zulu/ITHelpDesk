using Microsoft.AspNetCore.Identity;
using SharedLibrary.Model;

namespace AuthProject.Data
{
    public static class SeedData
    {

        public static async Task CreateRoles(IServiceProvider serviceProvider, string username, string password, string email)
        {
            //add custom roles
            var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var UserManager =serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            string[] rolenames = { "Admin","SuperAdmin", "User", "Agent" };

            Console.WriteLine(email);
            Console.WriteLine(username);
            Console.WriteLine(password);
            IdentityResult roleResult;

            foreach(var roleName in rolenames)
            {   
                //creating the roles and seeding them to the database
                var roleExist = await RoleManager.RoleExistsAsync(roleName);

                if (!roleExist)
                {
                    roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            //creating a super user who could maintain the web app
            var superAdmin = new ApplicationUser
            {
                UserName = username,
                Email=email
            };
            //Console.WriteLine(superAdmin.Email);
            //Console.WriteLine(superAdmin.UserName);
            var User=await UserManager.FindByEmailAsync(superAdmin.Email);

            if ( User == null)
            {
                var superUser = await UserManager.CreateAsync(superAdmin,password);

                if (superUser.Succeeded)
                {
                    Console.WriteLine("Super admin created successfully!");
                    await UserManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
                else
                {
                    Console.WriteLine("Failed to create super admin");
                    foreach(var error in superUser.Errors)
                    {
                        Console.WriteLine(error);
                    }
                }
            }
            else
            {
                Console.WriteLine("Super Admin already exists");
            }
        }
    }
}
