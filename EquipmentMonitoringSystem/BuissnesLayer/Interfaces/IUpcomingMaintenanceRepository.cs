using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IUpcomingMaintenanceRepository
    {
        IEnumerable<UpcomingMaintenance> GetAllUpcomingMaintenance();
        UpcomingMaintenance GetUpcomingMaintenanceById(int id);
        IEnumerable<UpcomingMaintenance> GetUpmainByEquipId(int id);
        bool CheckMainInUpComMain(int mainid);
        int GetUpcomingMaintenanceCountStationId(int stid);
        int GetUpcomingMaintenanceCountGroupId(int grid);
        IEnumerable<UpcomingMaintenance> GetUpcomingMaintenanceGroupId(int grid, bool includemain = false);
    }
}
