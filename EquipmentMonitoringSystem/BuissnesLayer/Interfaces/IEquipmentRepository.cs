using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IEquipmentRepository
    {
        IEnumerable<Equipment> GetAllEquipments();
        Equipment GetEquipmentById(int eqid);
        void SaveEquipment(Equipment equipment);
        void DeleteEquipment(Equipment equipment);
    }
}
