namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Report
    {
        public int Id { get; set; }
        public string Name { get; set; } // Наименование
        public DateOnly Date { get; set; } //Дата проведения
        public DateOnly DateDefacto { get; set; } //Дата когда ТО было пройдено
        public int MaintenanceId { get; set; } // Внешний ключ
        public string Description { get; set; } //Описание
        public string IdUser { get; set; } // Индефикатор пользователя, который должен провести ТО
    }
}
