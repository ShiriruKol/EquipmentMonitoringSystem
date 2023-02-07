using System.ComponentModel.DataAnnotations;

namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Group
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Equipment> Equipments { get; set; }
        public int StationId { get; set; } //Внешний ключ
        public Station Station { get; set; } //Навигационное свойство
    }
}
