using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class EquipmentViewModel
    {
        public Equipment Equipment { get; set; }

        public string GroupName { get; set; }
    }

    public class EquipmentIndexViewModel
    {
        public int StationId { get; set; }
        public int GroupId { get; set; }
        public List<SelectListItem> Stations { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public List<Equipment> Equipments { get; set; }
    }

    public class EquipmentEditViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Тип является обязательным полем!")]
        public string Type { get; set; } = string.Empty;

        [Required(ErrorMessage = "Заводской номер является обязательным полем!")]
        public string FactoryNumber { get; set; } = string.Empty;
        public List<MaintenanceEditModel> Maintenances { get; set; } = new List<MaintenanceEditModel>();
        public int GroupId { get; set; } = 0;
        public List<SelectListItem> Groups { get; set; } = new();

    }

    public class EquipmentExcelModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string FactoryNumber { get; set; } = string.Empty;
        public List<MaintenanceEditModel> Maintenances { get; set; } = new List<MaintenanceEditModel>();
        public int GroupId { get; set; } = 0;
    }

}
