using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using EquipmentMonitoringSystem.DataLayer.Entityes;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class EquipmentController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public EquipmentController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }

        public IActionResult Index(EquipmentIndexViewModel model)
        {
           
            
            var stlist = StationsToSelectedList();
            model.Stations = stlist;

            if(model.StationId != 0 && model.GroupId == 0)
            {
                var groupsList = GroupsToSelectedList(model.StationId);
                model.Groups = groupsList;
            }
            if (model.StationId != 0 && model.GroupId != 0)
            {
                List<Equipment> equipments = _datamanager.Equipments.GetEquipmentsByIdGroup(model.GroupId).ToList();
                foreach(var eq in equipments)
                {
                    Maintenance main = _datamanager.Maintenances.GetMaintenanceByEqIdCurrDate(eq.Id);
                    List<Maintenance> maintenances = new List<Maintenance>();
                    maintenances.Add(main);
                    eq.Maintenances = maintenances;
                }
                model.Equipments = equipments;
            }

            return View(model);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var groupsList = GroupsToSelectedList();
            var model = new EquipmentEditViewModel
            {
                Groups = groupsList,
            };

            for (int i = 0; i < 6; i++) {
                var m = new MaintenanceEditModel()
                {
                    Name = "",
                    NumberHours = 0,
                    Status = false,
                    DateMaintenance = ""
                };
                model.Maintenances.Add(m);
            }
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(EquipmentEditViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное наименование!");
            if (string.IsNullOrEmpty(model.Type) || string.IsNullOrWhiteSpace(model.Type.Trim()))
                ModelState.AddModelError(nameof(model.Type), "Указано некорректный тип!");
            if (string.IsNullOrEmpty(model.FactoryNumber) || string.IsNullOrWhiteSpace(model.FactoryNumber.Trim()))
                ModelState.AddModelError(nameof(model.FactoryNumber), "Указано некорректный заводской номер!");
            if (model.GroupId == 0)
                ModelState.AddModelError(nameof(model.GroupId), "Выберите Группу");

            if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Groups = GroupsToSelectedList();
                return View(model);
            }

            _servicesmanager.Equipments.SaveEquipmentEditModelToDb(model);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _servicesmanager.Equipments.GetEquipmentEditModel(id, true);
            if (model == null)
                return NotFound(nameof(model));

            var groupsList = GroupsToSelectedList();
            model.Groups = groupsList;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EquipmentEditViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное наименование!");
            if (string.IsNullOrEmpty(model.Type) || string.IsNullOrWhiteSpace(model.Type.Trim()))
                ModelState.AddModelError(nameof(model.Type), "Указано некорректный тип!");
            if (string.IsNullOrEmpty(model.FactoryNumber) || string.IsNullOrWhiteSpace(model.FactoryNumber.Trim()))
                ModelState.AddModelError(nameof(model.FactoryNumber), "Указано некорректный заводской номер!");
            if (model.GroupId == 0)
                ModelState.AddModelError(nameof(model.GroupId), "Выберите Группу");

            if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Groups = GroupsToSelectedList();
                return View(model);
            }

            _servicesmanager.Equipments.SaveEquipmentEditModelToDb(model);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Info(int id)
        {
            if (id == 0)
                return BadRequest();

            var model = _servicesmanager.Equipments.EquipmentDBModelToView(id);

            return View(model);
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            _servicesmanager.Equipments.DeleteEquipment(id);
            return RedirectToAction("Index");
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

        private List<SelectListItem> GroupsToSelectedList(int stid)
        {
            var groups = _datamanager.Groups.GetAllGroupsByStId(false, stid).Select(group => new SelectListItem
            {
                Value = group.Id.ToString(),
                Text = group.Name,
            }).ToList();

            return groups;
        }

        private List<SelectListItem> GroupsToSelectedList()
        {
            var groups = _datamanager.Groups.GetAllGroups().Select(group => new SelectListItem
            {
                Value = group.Id.ToString(),
                Text = group.Name,
            }).ToList();

            return groups;
        }

    }
}
