namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class UpcomingMaintenance
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; } // Наименование оборудования
        public string MaintenanceName { get; set; } // Наименование тех.обслуживания
        public DateOnly DateMaintenance { get; set; } // Дата проведения тех. обслуживания
    }
}
