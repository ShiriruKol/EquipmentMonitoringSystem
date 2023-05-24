using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.Extensions.FileProviders;
using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class StationViewModel
    {
        public Station Station { get; set; }
        public List<GroupViewModel> Groups { get; set; }
    }

    public class StationInfoViewModel
    {
        public string StationName { get; set; }
        public List<Group> Groups { get; set; }
    }

    public class StationIndexViewModel
    {
        public Station Station { get; set; }
        public int NumberGroups { get; set; } 
    }

    public class StationEditModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Наименование является обязательным полем!")]
        public string Name { get; set; } = string.Empty;
    }

    public class StationExcelModel
    {
        public int Id { get; set; } 
        public string Name { get; set; }
        public bool Checkst { get; set; } = true;
        public List<GroupExcelModel> Groups { get; set; } = new List<GroupExcelModel>();
    }

    public class ExcelStationsInfo
    {
        public int StCount { get; set; } = 0;
        public int GrCount { get; set; } = 0;
        public int EqCount { get; set; } = 0;
        public string FileName { get; set; } = string.Empty;
    }
}
