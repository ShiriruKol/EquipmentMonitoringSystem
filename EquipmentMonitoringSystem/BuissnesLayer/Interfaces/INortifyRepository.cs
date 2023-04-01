using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface INortifyRepository
    {
        IEnumerable<Nortify> GetAllNortify();
        Nortify GetNortifyById(int id);
        void SaveNortify(Nortify nortify);
        void DeleteNortify(Nortify nortify);
    }
}
