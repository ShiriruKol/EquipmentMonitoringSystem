using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.BuissnesLayer.Interfaces
{
    public interface IStationRepository
    {
        IEnumerable<Station> GetAllStations(bool includegroups = false);
        Station GetStationById(int stationid, bool includegroups = false);
        void SaveStation(Station station);
        void DeleteStation(Station station);
    }
}
