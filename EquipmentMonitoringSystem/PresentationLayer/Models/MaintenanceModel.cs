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

    public class MaintenacesCompleteModel
    {

        public int StationId { get; set; }
        public int GroupId { get; set; }
        public List<SelectListItem> Stations { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public List<Main> UpcomingMaintenances { get; set; }
    }

    public class Main
    {
        public string NameMain { get; set; } = string.Empty;
        public string NameEquip { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }

    public class MaintenanceUnscheduledModel
    {
        [Required]
        public int StationId { get; set; }
        [Required]
        public int GroupId { get; set; }
        [Required]
        public int EquipmentId { get; set; }
        [Required(ErrorMessage = "Описание является обязательным полем!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Кол-во часов является обязательным полем!")]
        public string NumberHours { get; set; }
        [Required(ErrorMessage = "Дата является обязательным полем!")]
        public string Date { get; set; }
        [Required]
        public List<SelectListItem> Stations { get; set; }
    }
}
