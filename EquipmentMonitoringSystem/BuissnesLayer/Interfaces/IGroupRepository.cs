using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IGroupRepository
    {
        IEnumerable<Group> GetAllGroups(bool includeequipment = false);
        Group GetGroupById(int grid, bool includeequipment = false);
        int GetEqCountbyGroup(int groupId);
        void SaveGroup(Group group);
        void DeleteGroup(Group group);
    }
}
