namespace EquipmentMonitoringSystem.PresentationLayer.Models
{
    public class UnplannedMainView
    {
        public int Id { get; set; }
        public string? Header { get; set; }
        public string? Description { get; set; }
        public string? EmplName { get; set; }
        public string? StatName { get; set; }
        public string? NumHours { get; set; }
    }

    public class UnplannedMainViewForUser
    {
        public int IdUser { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }

        public List<UnplannedMainView> UnplannedMainViews;
    }
}
