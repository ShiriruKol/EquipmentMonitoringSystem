using EquipmentMonitoringSystem.Areas.Identity.Data;
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
        private readonly AuthDbContext _authdb;

        public MaintenanceController(DataManager datamanager, ServicesManager servicesmanager, AuthDbContext authdb)
        {
            _datamanager = datamanager;
            _servicesmanager = servicesmanager;
            _authdb = authdb;
        }

        [HttpGet]
        public IActionResult AddUnscheduled()
        {
            ApplicationUser user = _authdb.ApplicationUser.FirstOrDefault(x => x.UserName == User.Identity.Name);
            MaintenanceUnscheduledModel model = new MaintenanceUnscheduledModel();  
            var stlist = StationsToSelectedList();
            var users = UsersToSelectedList();
            model.Stations = stlist;
            model.Users = users;
            model.Role = _authdb.Roles.FirstOrDefault(x=>x.Id == _authdb.UserRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId).NormalizedName;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddUnscheduled(MaintenanceUnscheduledModel model)
        {
            model.Stations = StationsToSelectedList();
            
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

            Report report = new Report()
            {
                MaintenanceId = maintenance.Id,
                Name = maintenance.Name,
                Date = maintenance.DateMaintenance,
                Description = maintenance.Description,
            };

            ApplicationUser user = _authdb.ApplicationUser.FirstOrDefault(x => x.UserName == User.Identity.Name);
            var Role = _authdb.Roles.FirstOrDefault(x => x.Id == _authdb.UserRoles.FirstOrDefault(x => x.UserId == user.Id).RoleId).NormalizedName;
            if (Role == "ИНЖЕНЕР")            
            {
                report.IdUser = user.Id;
            }
            else
            {
                report.IdUser = model.idUser;
            }

            
            _datamanager.Reports.SaveReport(report);
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

        private List<SelectListItem> UsersToSelectedList()
        {
            List<SelectListItem> usersAll = new List<SelectListItem>();
            var usr = _authdb.ApplicationUser.ToList();
            string nameengId = _authdb.Roles.FirstOrDefault(x => x.NormalizedName == "ИНЖЕНЕР").Id;
            var idusersEng = _authdb.UserRoles.Where(x => x.RoleId == nameengId).ToList();

            List<ApplicationUser> usrsEng = new List<ApplicationUser>();

            foreach (var item in idusersEng)
            {
                if (_authdb.ApplicationUser.FirstOrDefault(x => x.Id == item.UserId) != null)
                {
                    ApplicationUser user = _authdb.ApplicationUser.FirstOrDefault(x => x.Id == item.UserId);
                    SelectListItem ur = new SelectListItem()
                    {
                        Value = user.Id.ToString(),
                        Text = user.FullName,
                    };
                    usersAll.Add(ur);
                }

            }

            return usersAll;
        }
    }
}
