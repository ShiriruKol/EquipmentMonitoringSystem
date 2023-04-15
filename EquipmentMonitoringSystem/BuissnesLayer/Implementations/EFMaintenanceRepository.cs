using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFMaintenanceRepository : IMaintenanceRepository
    {
        private readonly EFDBContext _context;
        public EFMaintenanceRepository(EFDBContext context)
        {
            _context = context;
        }

        public void DeleteMaintenance(Maintenance maintenance)
        {
            _context.Maintenances.Remove(maintenance);
            _context.SaveChanges();
        }

        public IEnumerable<Maintenance> GetAllMaintenances()
        {
            return _context.Maintenances.ToList();
        }

        public Maintenance GetMaintenanceByEqIdCurrDate(int eqid)
        {
            return _context.Maintenances.Where(x => x.EquipmentId == eqid && x.DateMaintenance > DateOnly.FromDateTime(DateTime.Now)).FirstOrDefault();
        }

        public Maintenance GetMaintenanceById(int id)
        {
            return _context.Maintenances.FirstOrDefault(x => x.Id == id)!;
        }

        public void SaveMaintenance(Maintenance maintenance)
        {
            if (maintenance.Id == 0)
                _context.Maintenances.Add(maintenance);
            else
                _context.Entry(maintenance).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
