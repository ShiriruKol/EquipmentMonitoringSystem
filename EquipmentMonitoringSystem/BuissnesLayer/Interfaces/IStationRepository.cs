using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IStationRepository
    {
        IEnumerable<Station> GetAllStations(bool includegroups = false);
        Station GetStationById(int stationid, bool includegroups = false);
        int GetNumGroupsStationId(int stationid);
        string GetStationName(int stationid);
        List<Group> GetGroupsByStation(int stationid);
        int GetEquipmentCount(int stationid);
        void SaveStation(Station station);
        void DeleteStation(Station station);
        int GetSuccsesMaintenancesCount(int stationid);
        int GetUnplannedCount(int stationid, int monthnum, bool checkunpl);
        void DeleteStationCheck();
        void UpdateStationCheck();
    }
}
