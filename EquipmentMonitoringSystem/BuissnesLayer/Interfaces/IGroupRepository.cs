using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IGroupRepository
    {
        IEnumerable<Group> GetAllGroups(bool includeequipment = false);
        IEnumerable<Group> GetAllGroupsByStId(bool includeequipment = false, int idst = 0);
        Group GetGroupById(int grid, bool includeequipment = false);
        int GetEqCountbyGroup(int groupId);
        void SaveGroup(Group group);
        void DeleteGroup(Group group);
    }
}
