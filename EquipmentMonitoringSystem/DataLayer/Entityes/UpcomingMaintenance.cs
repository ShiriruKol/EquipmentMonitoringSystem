using MessagePack;
using System.ComponentModel.DataAnnotations.Schema;

namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class UpcomingMaintenance
    {
        public int Id { get; set; }
        public int MaintenancesID { get; set; }
        public Maintenance Maintenance { get; set; } // Навигационное свойство
    }
}
