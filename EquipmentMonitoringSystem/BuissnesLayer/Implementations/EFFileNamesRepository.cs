using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFFileNamesRepository : IFileNamesRepository
    {
        private readonly EFDBContext _context;
        public EFFileNamesRepository(EFDBContext context)
        {
            _context = context;
        }

        public bool CheckFileName(string name)
        {
            return _context.FileNames.Any(x => x.Name == name);
        }

        public void SaveFileName(FileNames file)
        {
            _context.FileNames.Add(file);
            _context.SaveChanges();
        }
    }
}
