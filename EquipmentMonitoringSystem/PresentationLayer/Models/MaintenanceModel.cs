using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class MaintenanceEditModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public double NumberHours { get; set; } //Кол-во часов
        public bool Status { get; set; } = false; //Статус прохождения ТО
        [Required]
        public DateOnly DateMaintenance { get; set; }
    }
}
