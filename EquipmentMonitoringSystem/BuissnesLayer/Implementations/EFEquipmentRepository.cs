using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFEquipmentRepository : IEquipmentRepository
    {
        private readonly EFDBContext _context;
        public EFEquipmentRepository(EFDBContext context)
        {
            _context = context;
        }
        public void DeleteEquipment(Equipment equipment)
        {
            _context.Equipments.Remove(equipment);
            _context.SaveChanges();
        }

        public IEnumerable<Equipment> GetAllEquipments(bool includemain = false)
        {
            if (includemain)
                return _context.Set<Equipment>().Include(x => x.Maintenances).AsNoTracking().ToList();
            else
                return _context.Equipments.ToList();
        }

        public Equipment GetEquipmentById(int eqid, bool includemain = false)
        {
            if (includemain)
                return _context.Set<Equipment>().Include(x => x.Maintenances).AsNoTracking().FirstOrDefault(x => x.Id == eqid)!;
            else
                return _context.Equipments.FirstOrDefault(x => x.Id == eqid)!;
        }

        public IEnumerable<Equipment> GetEquipmentsByIdGroup(int idgr)
        {
            return _context.Set<Equipment>().Where(x => x.GroupId == idgr).AsNoTracking()!;
        }

        public void SaveEquipment(Equipment equipment)
        {
            if (equipment.Id == 0)
                _context.Equipments.Add(equipment);
            else
                _context.Entry(equipment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }

}
