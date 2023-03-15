using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IUpcomingMaintenanceRepository
    {
        IEnumerable<UpcomingMaintenance> GetAllUpcomingMaintenance();
        UpcomingMaintenance GetUpcomingMaintenanceById(int id);
    }
}
