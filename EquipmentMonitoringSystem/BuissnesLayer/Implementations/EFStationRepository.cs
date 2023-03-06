using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace EquipmentMonitoringSystem.BuissnesLayer.Implementations
{
    public class EFStationRepository : IStationRepository
    {
        private readonly EFDBContext _context;
        public EFStationRepository(EFDBContext context)
        {

            _context = context;
        }

        public void DeleteStation(Station station)
        {
            _context.Stations.Remove(station);
            _context.SaveChanges();
        }

        public IEnumerable<Station> GetAllStations(bool includegroups = false)
        {
            if (includegroups)
                return _context.Set<Station>().Include(x => x.Groups).AsNoTracking().ToList();
            else
                return _context.Stations.ToList();
        }

        public Station GetStationById(int stationid, bool includegroups = false)
        {
            if (includegroups)
                return _context.Set<Station>().Include(x => x.Groups).AsNoTracking().FirstOrDefault(x => x.Id == stationid)!;
            else
                return _context.Stations.FirstOrDefault(x => x.Id == stationid)!;
        }

        public int GetNumGroupsStationId(int stationid)
        {
            return _context.Set<Station>().Include(x => x.Groups).AsNoTracking().FirstOrDefault(x => x.Id == stationid).Groups.Count!;
        }

        public void SaveStation(Station station)
        {
            if (station.Id == 0)
                _context.Stations.Add(station);
            else
                _context.Entry(station).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
        }

        public string GetStationName(int stationid)
        {
            return _context.Stations.FirstOrDefault(x => x.Id == stationid)!.Name;
        }

        public List<Group> GetGroupsByStation(int stationid)
        {
            return _context.Set<Group>().Include(x => x.Equipments).Where(x => x.StationId == stationid).AsNoTracking().ToList();
        }
    }
}
