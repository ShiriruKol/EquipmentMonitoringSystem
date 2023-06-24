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
            model.Stations = StationsToSelectedList();
            /*if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Stations = StationsToSelectedList();
                return View(model);
            }
            */

            Maintenance maintenance = new Maintenance()
            {
                Name = "Внеплановый",
                Description = model.Description == null ? "none" : model.Description,
                DateMaintenance = DateOnly.Parse(model.Date),
                NumberHours = Convert.ToDouble(model.NumberHours),
                IsUnplanned = true,
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

        public IActionResult CompleteMaintenances(MaintenacesCompleteModel model)
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
                List<Main> Mains = new List<Main>();
                List<Equipment> equipmentlist = _datamanager.Equipments.GetEquipmentsByIdGroup(model.GroupId, true).ToList();
                List<Maintenance> maintenances = new List<Maintenance>();
                foreach (var equipment in equipmentlist)
                {
                    foreach (var maitenance in equipment.Maintenances)
                    {
                        if (_datamanager.Maintenances.CheckMainComplete(maitenance.Id))
                        {
                            Main upMain = new Main()
                            {
                                NameMain = maitenance.Name,
                                NameEquip = equipment.Name,
                                Date = maitenance.DateMaintenance.ToString(),
                            };
                            Mains.Add(upMain);
                        }
                    }
                }
                model.UpcomingMaintenances = Mains;
            }
            return View(model);
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
    }
}
