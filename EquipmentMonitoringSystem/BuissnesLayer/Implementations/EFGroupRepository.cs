using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFGroupRepository : IGroupRepository
    {
        private readonly EFDBContext _context;
        public EFGroupRepository(EFDBContext context)
        {
            _context = context;
        }

        public void DeleteGroup(DataLayer.Entityes.Group group)
        {
            _context.Groups.Remove(group);
            _context.SaveChanges();
        }

        public IEnumerable<DataLayer.Entityes.Group> GetAllGroups(bool includeequipment = false)
        {
            if (includeequipment)
                return _context.Set<DataLayer.Entityes.Group>().Include(x => x.Equipments).AsNoTracking().ToList();
            else
                return _context.Groups.ToList();
        }

        public IEnumerable<Group> GetAllGroupsByStId(bool includeequipment = false, int idst = 0)
        {
            if (includeequipment)
                return _context.Set<DataLayer.Entityes.Group>().Where(x => x.StationId == idst).Include(x => x.Equipments).AsNoTracking().ToList();
            else
                return _context.Groups.Where(x => x.StationId == idst).ToList();
        }

        public int GetEqCountbyGroup(int groupId)
        {
            return _context.Set<Group>().Include(x => x.Equipments).AsNoTracking().FirstOrDefault(x => x.Id == groupId).Equipments.Count!;
        }

        public DataLayer.Entityes.Group GetGroupById(int grid, bool includeequipment = false)
        {
            if (includeequipment)
                return _context.Set<DataLayer.Entityes.Group>().Include(x => x.Equipments).AsNoTracking().FirstOrDefault(x => x.Id == grid)!;
            else
                return _context.Groups.FirstOrDefault(x => x.Id == grid)!;
        }

        public void SaveGroup(DataLayer.Entityes.Group group)
        {
            if (group.Id == 0)
            {
                _context.Groups.Add(group);
            }
            else
                _context.Entry(group).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
