using EquipmentMonitoringSystem.DataLayer.Entityes;
using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class StationViewModel
    {
        public Station Station { get; set; }
        public List<GroupViewModel> Groups { get; set; }
    }

    public class StationEditModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Наименование является обязательным полем!")]
        public string Name { get; set; } = string.Empty;
    }

}
