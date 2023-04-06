using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFUpcomingMaintenanceRepository : IUpcomingMaintenanceRepository
    {
        private readonly EFDBContext _context;
        public EFUpcomingMaintenanceRepository(EFDBContext context)
        {
            _context = context;
        }

        public IEnumerable<UpcomingMaintenance> GetAllUpcomingMaintenance()
        {
            return _context.UpcomingMaintenances.ToList();
        }

        public UpcomingMaintenance GetUpcomingMaintenanceById(int id)
        {
            return _context.Set<UpcomingMaintenance>().AsNoTracking().FirstOrDefault(x => x.Id == id)!;
        }
    }
}
