using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.Controllers
{
    public class EquipmentController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public EquipmentController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }

        public IActionResult Index()
        {
            List<EquipmentViewModel> _dirs = _servicesmanager.Equipments.GetEquipmentList();
            return View(_dirs);
        }
        [HttpGet]
        public IActionResult Add()
        {
            var groupsList = GroupsToSelectedList();
            var model = new EquipmentEditViewModel
            {
                Groups = groupsList,
            };
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
            var model = _servicesmanager.Equipments.GetEquipmentEditModel(id);
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
        public IActionResult Remove(int id)
        {
            _servicesmanager.Equipments.DeleteEquipment(id);
            return RedirectToAction("Index");
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
