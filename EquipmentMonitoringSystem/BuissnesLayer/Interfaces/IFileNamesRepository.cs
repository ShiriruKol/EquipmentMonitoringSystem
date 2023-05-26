using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IFileNamesRepository
    {
        void SaveFileName(FileNames file);
        bool CheckFileName(string name);
    }
}
