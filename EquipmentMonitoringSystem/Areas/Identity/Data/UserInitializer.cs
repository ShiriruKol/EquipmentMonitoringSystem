using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.AspNetCore.Identity;

namespace EquipmentMonitoringSystem.Areas.Identity.Data
{
    public class UserInitializer
    {
        private const string AdminFullName = "Грибов Александр Макарович";
        private const string AdminRoleName = "Admin";
        private const string AdminUserEmail = "admin@example.com";
        private const string AdminUserPassword = "Admin1!";

        private const string ChiefFullName = "Озеров Евгений Григорьевич";
        private const string ChiefRoleName = "Главный механик";
        private const string ChiefUserEmail = "Chief@example.com";
        private const string ChiefUserPassword = "Chief1!";

        private const string EngineerFullName = "Федоров Марк Станиславович";
        private const string EngineerRoleName = "Инженер";
        private const string EngineerUserEmail = "Engineer@example.com";
        private const string EngineerUserPassword = "Engineer1!";

        public static async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await CreateRole(roleManager, AdminRoleName);
            await CreateUser(userManager, AdminUserEmail, AdminUserPassword, AdminRoleName, AdminFullName);

            await CreateRole(roleManager, ChiefRoleName);
            await CreateUser(userManager, ChiefUserEmail, ChiefUserPassword, ChiefRoleName, ChiefFullName);

            await CreateRole(roleManager, EngineerRoleName);
            await CreateUser(userManager, EngineerUserEmail, EngineerUserPassword, EngineerRoleName, EngineerFullName);
        }
        private static async Task CreateUser(UserManager<IdentityUser> userManager, string userEmail, string userPassword, string roleName, string fullName)
        {
            if (await userManager.FindByNameAsync(userEmail) == null)
            {
                ApplicationUser admin = new ApplicationUser { Email = userEmail, UserName = userEmail , FullName = fullName};
                IdentityResult result = await userManager.CreateAsync(admin, userPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, roleName);
                }
            }
        }
        private static async Task CreateRole(RoleManager<IdentityRole> roleManager, string roleName)
        {
            if (await roleManager.FindByNameAsync(roleName) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
