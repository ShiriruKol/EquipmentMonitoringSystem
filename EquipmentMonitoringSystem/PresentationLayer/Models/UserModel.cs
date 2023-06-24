using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class UserModel
    {
    }

    public class UserViewModel {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; } = string.Empty;
    }

    public class UserEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public string RoleId { get; set; } = string.Empty;
        public List<SelectListItem> Roles { get; set; } = new();

    }
}
