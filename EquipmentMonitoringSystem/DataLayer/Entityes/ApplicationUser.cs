using Microsoft.AspNetCore.Identity;

namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}
