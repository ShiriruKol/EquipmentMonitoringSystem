namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Nortify
    {
        public int Id { get; set; }
        public string Heding { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int MaintenancesID { get; set; }
        public Maintenance Maintenance { get; set; } // Навигационное свойство
    }
}
