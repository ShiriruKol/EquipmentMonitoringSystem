namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Nortify
    {
        public int Id { get; set; }
        public string Heding { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime? Date { get; set; } = default(DateTime?);
    }
}
