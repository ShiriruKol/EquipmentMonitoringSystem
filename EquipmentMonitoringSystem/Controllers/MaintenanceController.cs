using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EquipmentMonitoringSystem.Controllers
{
    public class MaintenanceController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public MaintenanceController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }

        [HttpGet]
        public IActionResult AddUnscheduled()
        {
            MaintenanceUnscheduledModel model = new MaintenanceUnscheduledModel();  
            var stlist = StationsToSelectedList();
            model.Stations = stlist;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddUnscheduled(MaintenanceUnscheduledModel model)
        {
            Maintenance maintenance = new Maintenance()
            {
                Name = "Внеплановый",
                Description = model.Description,
                DateMaintenance = DateOnly.Parse(model.Date),
                NumberHours = Convert.ToDouble(model.NumberHours),
                EquipmentId = model.EquipmentId,
            };
            _datamanager.Maintenances.SaveMaintenance(maintenance);
            return RedirectToAction("AddUnscheduled");
        }

        private List<SelectListItem> StationsToSelectedList()
        {
            var stations = _datamanager.Stations.GetAllStations().Select(st => new SelectListItem
            {
                Value = st.Id.ToString(),
                Text = st.Name,
            }).ToList();

            return stations;
        }
    }
}
