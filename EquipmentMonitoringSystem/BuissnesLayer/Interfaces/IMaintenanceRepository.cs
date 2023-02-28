using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IMaintenanceRepository
    {
        IEnumerable<Maintenance> GetAllMaintenances();
        Maintenance GetMaintenanceById(int id);
        void SaveMaintenance(Maintenance maintenance);
        void DeleteMaintenance(Maintenance maintenance);
    }
}
