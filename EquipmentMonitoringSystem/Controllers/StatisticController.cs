using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class StatisticController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public StatisticController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }

        public IActionResult Index()
        {
            List<StationIndexViewModel> _dirs = _servicesmanager.Stations.GetStationList();
            return View(_dirs);
        }
        [HttpPost]
        public List<object> GetStatistics()
        {
            List<object> statistics = new List<object>();
            var sts = _datamanager.Stations.GetAllStations();
            List<string> statstr = new List<string>();
            List<int> eqcount = new List<int>();
            foreach (var station in sts)
            {
                statstr.Add(station.Name);
                eqcount.Add(_datamanager.Stations.GetEquipmentCount(station.Id));
            }
            statistics.Add(statstr);
            statistics.Add(eqcount);
            return statistics;
        }

        [HttpPost]
        public List<object> GetStatisticsMounth()
        {
            List<object> statistics = new List<object>();
            var sts = _datamanager.Stations.GetAllStations();
            List<string> statstr = new List<string>();
            List<int> eqcount = new List<int>();
            foreach (var station in sts)
            {
                statstr.Add(station.Name);
                eqcount.Add(_datamanager.Stations.GetSuccsesMaintenancesCount(station.Id));
            }
            statistics.Add(statstr);
            statistics.Add(eqcount);
            return statistics;
        }

        [HttpPost]
        public List<object> GetStatisticsUplanned(int idmounth, bool checkunpl = true)
        {
            List<object> statistics = new List<object>();
            var sts = _datamanager.Stations.GetAllStations();
            List<string> statstr = new List<string>();
            List<int> eqcount = new List<int>();
            foreach (var station in sts)
            {
                statstr.Add(station.Name);
                eqcount.Add(_datamanager.Stations.GetUnplannedCount(station.Id, idmounth, checkunpl));
            }
            statistics.Add(statstr);
            statistics.Add(eqcount);
            return statistics;
        }

        [HttpPost]
        public List<object> GetStatisticsStation(int idstat)
        {
            List<object> stcount = new List<object>();
            var st = _datamanager.Stations.GetStationById(idstat);
            for (int i = 1; i <= 12; i++)
            {
                stcount.Add(_datamanager.Maintenances.GetCountMainMounth(idstat, i));
            }
            return stcount;
        }
    }
}
