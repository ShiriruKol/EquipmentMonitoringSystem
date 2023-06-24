using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IReportRepository
    {
        IEnumerable<Report> GetAllReports(bool includemain = false);
        Report GetReportById(int repid, bool includemain = false);
        void SaveReport(Report repid);
        void DeleteReport(Report repid);
        Report AvailabilityMain(int main);
    }
}
