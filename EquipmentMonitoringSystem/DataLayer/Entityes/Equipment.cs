

namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } // Наименование
        public string Type { get; set; } // Тип
        public string FactoryNumber { get; set; } // Заводской номер
        public List<Maintenance> Maintenances { get; set; } //Список ТО
        public int GroupId { get; set; } // Внешний ключ
        public Group Group { get; set; } // Навигационное свойство
    }
}
