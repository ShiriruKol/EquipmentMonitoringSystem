using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;

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
            return _context.UpcomingMaintenances.FirstOrDefault(x => x.Id == id)!;
        }
    }
}
