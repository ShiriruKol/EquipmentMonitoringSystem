using EquipmentMonitoringSystem.DataLayer.Entityes;
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
        public string NameMain { get; set; } = string.Empty;
        public string NameEquip { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
    }
}
