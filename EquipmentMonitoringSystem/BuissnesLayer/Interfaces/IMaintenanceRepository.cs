using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IMaintenanceRepository
    {
        IEnumerable<Maintenance> GetAllMaintenances();
        Maintenance GetMaintenanceById(int id);
        Maintenance GetMaintenanceByEqIdCurrDate(int eqid);
        void SaveMaintenance(Maintenance maintenance);
        void DeleteMaintenance(Maintenance maintenance);
        bool CheckMainComplete(int nainid);
        int GetCountMainMounth(int stid, int month);
    }
}
