using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class UpcomingMaintenanceController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public UpcomingMaintenanceController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }
        public IActionResult Index()
        {
            List<UpcomingViewMaintenanceModel> _dirs = _servicesmanager.UpcomingMaintenanceService.GetStationList();
            return View(_dirs);
        }
    }
}
