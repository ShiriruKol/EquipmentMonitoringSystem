using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IEquipmentRepository
    {
        IEnumerable<Equipment> GetAllEquipments(bool includemain = false);
        Equipment GetEquipmentById(int eqid, bool includemain = false);
        void SaveEquipment(Equipment equipment);
        void DeleteEquipment(Equipment equipment);
    }
}
