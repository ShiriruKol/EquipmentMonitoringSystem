using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class MaintenanceEditModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public double NumberHours { get; set; }  //Кол-во часов
        public bool Status { get; set; } = false; //Статус прохождения ТО
        [Required]
        public string DateMaintenance { get; set; }
        public int EquipmentId { get; set; } = 0;
    }

    public class MaintenanceUnscheduledModel
    {
        public int StationId { get; set; }
        public int GroupId { get; set; }
        public int EquipmentId { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string NumberHours { get; set; }
        [Required]
        public string Date { get; set; }
        public List<SelectListItem> Stations { get; set; }
    }
}
