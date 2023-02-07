using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.DataLayer;

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

        public IEnumerable<Equipment> GetAllEquipments()
        {
            return _context.Equipments.ToList();
        }

        public Equipment GetEquipmentById(int eqid)
        {
            return _context.Equipments.FirstOrDefault(x => x.Id == eqid)!;
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
