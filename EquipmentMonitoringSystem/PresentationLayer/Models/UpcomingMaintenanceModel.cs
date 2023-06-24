using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class UpcomingViewMaintenanceModel
    {

        public int StationId { get; set; }
        public int GroupId { get; set; }
        public List<SelectListItem> Stations { get; set; }
        public List<SelectListItem> Groups { get; set; }
        public List<UpMain> UpcomingMaintenances { get; set; }
        public int CountMain { get; set; }
        public List<int> StCount { get; set; } = new List<int>();
    }

    public class UpMain 
    {
        public int Id { get; set; }
        public string NameMain { get; set; } = string.Empty;
        public string NameEquip { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }

    public class EqPlan
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string FactoryNumber { get; set; } = string.Empty;
        public string MainDateName { get; set; } = string.Empty;
        public int MainId { get; set;}
    }

    public class EqupsGroup
    {
        public string NameGroup { get; set; }
        public List<EqPlan> Equipments { get; set; } = new List<EqPlan>();
    }
}
