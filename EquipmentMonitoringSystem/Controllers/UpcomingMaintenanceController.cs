using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Npgsql;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class UpcomingMaintenanceController : Controller
    {
        private readonly DataManager _datamanager;

        public UpcomingMaintenanceController(DataManager datamanager)
        {
            _datamanager = datamanager;
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
            List<UnplannedMainView> unplannedMains = new List<UnplannedMainView>();
            List<Nortify> nortfs = _datamanager.Nortify.GetAllNortify().ToList();
            
            foreach (var nort in nortfs)
            {
                Maintenance main = _datamanager.Maintenances.GetMaintenanceById(nort.MaintenancesID);
                Equipment eq = _datamanager.Equipments.GetEquipmentById(main.EquipmentId);
                Group gr = _datamanager.Groups.GetGroupById(eq.GroupId);
                string namest = _datamanager.Stations.GetStationName(gr.StationId);
                
                UnplannedMainView unplanned = new UnplannedMainView() {
                    Id = main.Id,
                    Header = nort.Heding,
                    Description = nort.Description,
                    EmplName = eq.Name,
                    StatName = namest,
                };

                unplannedMains.Add(unplanned);
            }

            return View(unplannedMains); 
        }

        [HttpGet]
        public IActionResult Fix(int id)
        {
            if (id == 0)
                return BadRequest();

            Maintenance main = _datamanager.Maintenances.GetMaintenanceById(id);
            main.Status = true;
            _datamanager.Maintenances.SaveMaintenance(main);
            string sqlExpression = "update_nortifys";
            string connectionString = @"Server=localhost;Database=PulseRigDB;Port=5432;User Id=postgres;Password=12K345i678R9;";
            using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
            {
                connection.Open();
                NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                var result = command.ExecuteScalar();

                Console.WriteLine("Успешный вызов процедуры", result);
            }
            return RedirectToAction("IndexNortf");
        }

        [HttpPost]
        public List<object> UnplanAll()
        {
            var lstunpla = _datamanager.Nortify.GetAllNortify().ToList();

            List<object> listunpObject = new List<object>();
            List<string> desc = new List<string>();
            List<string> head = new List<string>();
            List<int> idMain = new List<int>();
            List<string> eqnamelist = new List<string>();
            List<string> stnamelist = new List<string>();


            foreach (var item in lstunpla)
            {
                desc.Add(item.Description);
                head.Add(item.Heding);
                idMain.Add(item.MaintenancesID);

                Maintenance main = _datamanager.Maintenances.GetMaintenanceById(item.MaintenancesID);
                Equipment eq = _datamanager.Equipments.GetEquipmentById(main.EquipmentId);
                Group gr = _datamanager.Groups.GetGroupById(eq.GroupId);
                string namest = _datamanager.Stations.GetStationName(gr.StationId);

                eqnamelist.Add(eq.Name);
                stnamelist.Add(namest);
            }
            
            listunpObject.Add(head);
            listunpObject.Add(desc);
            listunpObject.Add(idMain);
            listunpObject.Add(eqnamelist);
            listunpObject.Add(stnamelist);
            return listunpObject;
        }

        [HttpPost]
        public List<object> PlanAll(int grid)
        {
            List<object> listunpObject = new List<object>();

            List<string> desc = new List<string>();
            List<string> head = new List<string>();
            List<int> idMain = new List<int>();
            List<string> eqnamelist = new List<string>();
            string stname = _datamanager.Stations.GetStationName(_datamanager.Groups.GetGroupById(grid).StationId);


            var Listeq = _datamanager.Equipments.GetEquipmentsByIdGroup(grid).ToList();

            foreach ( var item in Listeq )
            {
                var listplan = _datamanager.UpcomingMaintenance.GetUpmainByEquipId(item.Id);

                foreach ( var item2 in listplan)
                {
                    desc.Add("---");
                    head.Add("Плановый ремонт");
                    idMain.Add(item2.MaintenancesID);
                    eqnamelist.Add(item.Name);

                    listunpObject.Add(head);
                    listunpObject.Add(desc);
                    listunpObject.Add(idMain);
                    listunpObject.Add(eqnamelist);
                    listunpObject.Add(stname);

                }

            }

            
            return listunpObject;
        }
    }
}
