using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoringSystem.Controllers
{
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
            return View();
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
    }
}
