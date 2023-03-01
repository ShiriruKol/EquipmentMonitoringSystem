using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using EquipmentMonitoringSystem.PresentationLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class StationController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public StationController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }

        public IActionResult Index()
        {
            List<StationViewModel> _dirs = _servicesmanager.Stations.GetStationList();
            return View(_dirs);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View(new StationEditModel());
        }

        [HttpPost]
        public IActionResult Add(StationEditModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное наименование!");

            if (!ModelState.IsValid)
                return View(model);

            _servicesmanager.Stations.SaveStationEditModelToDb(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Info(int id)
        {
            if (id == 0)
                return BadRequest();

            var model = _servicesmanager.Stations.StationDBToViewModelById(id);

            return View(model);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _servicesmanager.Stations.GetStationEditModel(id);
            if (model == null)
                return NotFound(nameof(model));

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(StationEditModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное наименование!");

            if (!ModelState.IsValid)
                return View(model);

            _servicesmanager.Stations.SaveStationEditModelToDb(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            _servicesmanager.Stations.DeleteStation(id);
            return RedirectToAction("Index");
        }

    }
}
