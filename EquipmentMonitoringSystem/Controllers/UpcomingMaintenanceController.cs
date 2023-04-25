using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public IActionResult Index(UpcomingViewMaintenanceModel model)
        {
            var stlist = StationsToSelectedList();
            model.Stations = stlist;

            if (model.StationId != 0)
            {
                var groupsList = GroupsToSelectedList(model.StationId);
                model.Groups = groupsList;
            }
            if (model.StationId != 0 && model.GroupId != 0)
            {
                List<UpMain> upMains = new List<UpMain>();
                List<Equipment> equipmentlist = _datamanager.Equipments.GetEquipmentsByIdGroup(model.GroupId, true).ToList();
                List<Maintenance> maintenances = new List<Maintenance>();
                foreach (var equipment in equipmentlist)
                {
                    foreach (var maitenance in equipment.Maintenances)
                    {
                        if (_datamanager.UpcomingMaintenance.CheckMainInUpComMain(maitenance.Id))
                        {
                            UpMain upMain = new UpMain()
                            {
                                NameMain = maitenance.Name,
                                NameEquip = equipment.Name,
                                Date = maitenance.DateMaintenance.ToString(),
                            };
                            upMains.Add(upMain);
                        }
                    }
                }
                model.UpcomingMaintenances = upMains;
            }
            return View(model);
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

        [HttpPost]
        public object CountNortify()
        {
            object count = _datamanager.Nortify.GetAllNortify().Count();
            return count;
        }

        [HttpGet]
        public IActionResult IndexNortf() { 
            return View(); 
        }
    }
}
