using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class ReportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Main { get; set; }
        public string Date { get; set; }
        public string UserName { get; set; }
        public string Status { get; set; }
    }

    public class CreateReportViewModel
    {
        [Required]
        public int mainId { get; set; }
        [Required(ErrorMessage = "Имя является обязательным полем!")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Имя является обязательным полем!")]
        public string EqName { get; set; }
        [Required(ErrorMessage = "Имя является обязательным полем!")]
        public string Date { get; set; }
        public string idUser { get; set; } = "0";
        public List<SelectListItem> Users { get; set; } = new();
    }
}
