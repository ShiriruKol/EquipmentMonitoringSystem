using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using EquipmentMonitoringSystem.PresentationLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize(Roles = "Admin,Главный механик")]
    public class GroupController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly ServicesManager _servicesmanager;

        public GroupController(DataManager datamanager, ServicesManager servicesmanager)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
        }

        public IActionResult Index(GroupViewIndexModel _model)
        {
            if(_model.Stations == null)
            {
                var stlist = StationsToSelectedList();
                _model.Stations = stlist;
            }
            
            if(_model.StationId != 0)
            {
                var grs = _datamanager.Groups.GetAllGroupsByStId(false, _model.StationId).Select(gr => new GroupModel
                {
                    Group = gr,
                    EqCount = _datamanager.Groups.GetEqCountbyGroup(gr.Id),
                }).ToList();
                _model.Groups = grs;
            }

            return View(_model);
        }
       

        [HttpGet]
        public IActionResult Add()
        {
            var stationsList = StationsToSelectedList();
            var model = new GroupEditViewModel
            {
                Stations = stationsList,
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult Add(GroupEditViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное наименование!");
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrWhiteSpace(model.Description.Trim()))
                ModelState.AddModelError(nameof(model.Description), "Указано некорректное описание!");
            if (model.StationId == 0)
                ModelState.AddModelError(nameof(model.StationId), "Выберите станцию");

            if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Stations = StationsToSelectedList();
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Add", model) });
            }

            _servicesmanager.Groups.SaveAlbumEditModelToDb(model);
            

            GroupViewIndexModel tmp = new GroupViewIndexModel();
            var stlist = StationsToSelectedList();
            tmp.Stations = stlist;

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", tmp) });
        }
        [HttpGet]
        public IActionResult Info(int id)
        {
            if (id == 0)
                return BadRequest();

            GroupInfoModel model = _servicesmanager.Groups.GroupDBToViewInfoModelById(id);

            return View(model);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _servicesmanager.Groups.GetGroupEditModel(id);
            if (model == null)
                return NotFound(nameof(model));

            var stationsList = StationsToSelectedList();
            model.Stations = stationsList;

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(GroupEditViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное наименование!");
            if (string.IsNullOrEmpty(model.Description) || string.IsNullOrWhiteSpace(model.Description.Trim()))
                ModelState.AddModelError(nameof(model.Description), "Указано некорректное описание!");
            if (model.StationId == 0)
                ModelState.AddModelError(nameof(model.StationId), "Выберите станцию");

            if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Stations = StationsToSelectedList();
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, "Edit", model) });
            }

            _servicesmanager.Groups.SaveAlbumEditModelToDb(model);

            GroupViewIndexModel tmp = new GroupViewIndexModel();
            var stlist = StationsToSelectedList();
            tmp.Stations = stlist;

            return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, "_ViewAll", tmp) });
        }

        private List<SelectListItem> StationsToSelectedList()
        {
            var stations = _datamanager.Stations.GetAllStations().Select(station => new SelectListItem
            {
                Value = station.Id.ToString(),
                Text = station.Name,
            }).ToList();

            return stations;
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {

            if (id == 0)
            {
                return NotFound();
            }

            var model = _servicesmanager.Groups.GroupDBToViewModelById(id);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost, ActionName("Remove")]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveConf(int id)
        {
            _servicesmanager.Groups.DeleteGroup(id);
            return RedirectToAction("Index");
        }
    }
}
