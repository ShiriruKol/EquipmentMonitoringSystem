namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Maintenance
    {
        public int Id { get; set; }
        public string Name { get; set; } // Наименование
        public double NumberHours { get; set; } //Кол-во часов
        public bool Status { get; set; } //Статус прохождения ТО
        public DateOnly DateMaintenance { get; set; } //Дата прохождения ТО
        public int EquipmentId { get; set; } // Внешний ключ
        public Equipment Equipment { get; set; } // Навигационное свойство
    }
}
