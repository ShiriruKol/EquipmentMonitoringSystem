using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFNortifyRepository : INortifyRepository
    {
        private readonly EFDBContext _context;
        public EFNortifyRepository(EFDBContext context)
        {
            _context = context;
        }

        public void DeleteNortify(Nortify nortify)
        {
            _context.Nortifys.Remove(nortify);
            _context.SaveChanges();
        }

        public IEnumerable<Nortify> GetAllNortify()
        {
            return _context.Nortifys.ToList();
        }

        public Nortify GetNortifyById(int id)
        {
            return _context.Nortifys.FirstOrDefault(x => x.Id == id)!;
        }

        public void SaveNortify(Nortify nortify)
        {
            if (nortify.Id == 0)
                _context.Nortifys.Add(nortify);
            else
                _context.Entry(nortify).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }
    }
}
