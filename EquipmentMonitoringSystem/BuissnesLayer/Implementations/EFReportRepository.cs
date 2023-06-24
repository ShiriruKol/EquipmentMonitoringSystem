using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFReportRepository : IReportRepository
    {
        private readonly EFDBContext _context;
        public EFReportRepository(EFDBContext context)
        {
            _context = context;
        }

        public Report AvailabilityMain(int main)
        {
            return _context.Reports.FirstOrDefault(x => x.MaintenanceId == main);
        }

        public void DeleteReport(Report rep)
        {
            _context.Reports.Remove(rep);
            _context.SaveChanges();
        }

        public IEnumerable<Report> GetAllReports(bool includemain = false)
        {
                return _context.Reports.ToList();
        }

        public Report GetReportById(int repid, bool includemain = false)
        {
                return _context.Reports.FirstOrDefault(x => x.Id == repid)!;
        }

        public void SaveReport(Report rep)
        {

            if (rep.Id == 0)
            {
                _context.Reports.Add(rep);
            }
            else
                _context.Entry(rep).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
