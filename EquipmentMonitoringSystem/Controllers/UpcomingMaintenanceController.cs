﻿using EquipmentMonitoringSystem.Areas.Identity.Data;
using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Npgsql;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Logical;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class UpcomingMaintenanceController : Controller
    {
        private readonly DataManager _datamanager;
        private readonly AuthDbContext _authDbContext;


        public UpcomingMaintenanceController(DataManager datamanager, AuthDbContext authDbContext)
        {
            _datamanager = datamanager;
            _authDbContext = authDbContext;
        }

        public IActionResult Index(UpcomingViewMaintenanceModel model)
        {
            var stlist = StationsToSelectedList();
            model.Stations = stlist;

            var sts = _datamanager.Stations.GetAllStations();
            foreach (var item in sts)
            {
                model.StCount.Add(_datamanager.UpcomingMaintenance.GetUpcomingMaintenanceCountStationId(item.Id));
            }

            if (model.StationId != 0)
            {
                var groupsList = GroupsToSelectedList(model.StationId);
                model.Groups = groupsList;
            }
            if (model.StationId != 0 && model.GroupId != 0)
            {
                List<UpMain> upMains = new List<UpMain>();
                List<Equipment> equipmentlist = _datamanager.Equipments.GetEquipmentsByIdGroup(model.GroupId, true).ToList();

                foreach (var equipment in equipmentlist)
                {
                    foreach (var maitenance in equipment.Maintenances)
                    {
                        if (_datamanager.UpcomingMaintenance.CheckMainInUpComMain(maitenance.Id))
                        {
                            UpMain upMain = new UpMain()
                            {
                                Id = maitenance.Id,
                                NameMain = maitenance.Name,
                                NameEquip = equipment.Name,
                                Date = maitenance.DateMaintenance.ToString(),
                                Appointed = _datamanager.Reports.AvailabilityMain(maitenance.Id) != null ? true : false,
                            };
                            upMains.Add(upMain);
                        }
                    }
                }
                model.UpcomingMaintenances = upMains;
            }
            model.CountMain = _datamanager.UpcomingMaintenance.GetAllUpcomingMaintenance().Count();
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

        private InfoUser GetUserInfo()
        {
            InfoUser infoUser = new InfoUser();
            //тут сохранен логин, по нему найти id и тогда сделать полноценные уведомления
            ApplicationUser user = _authDbContext.ApplicationUser.FirstOrDefault(x => x.UserName == User.Identity.Name);

            string nameengId = _authDbContext.Roles.FirstOrDefault(x => x.NormalizedName == "ИНЖЕНЕР").Id;
            var idusersEng = _authDbContext.UserRoles.Where(x => x.RoleId == nameengId).ToList();

            foreach (var iduser in idusersEng)
            {
                if (iduser.UserId == user.Id)
                {
                    infoUser.IdUser = user.Id;
                    infoUser.FullName = user.FullName;
                    infoUser.Role = _authDbContext.Roles.FirstOrDefault(x => x.Id == nameengId).NormalizedName;
                }
            }

            nameengId = _authDbContext.Roles.FirstOrDefault(x => x.NormalizedName == "ГЛАВНЫЙ МЕХАНИК").Id;
            idusersEng = _authDbContext.UserRoles.Where(x => x.RoleId == nameengId).ToList();

            foreach (var iduser in idusersEng)
            {
                if (iduser.UserId == user.Id)
                {
                    infoUser.IdUser = user.Id;
                    infoUser.FullName = user.FullName;
                    infoUser.Role = _authDbContext.Roles.FirstOrDefault(x => x.Id == nameengId).NormalizedName;
                }
            }

            nameengId = _authDbContext.Roles.FirstOrDefault(x => x.NormalizedName == "ADMIN").Id;
            idusersEng = _authDbContext.UserRoles.Where(x => x.RoleId == nameengId).ToList();

            foreach (var iduser in idusersEng)
            {
                if (iduser.UserId == user.Id)
                {
                    infoUser.IdUser = user.Id;
                    infoUser.FullName = user.FullName;
                    infoUser.Role = _authDbContext.Roles.FirstOrDefault(x => x.Id == nameengId).NormalizedName;
                }
            }

            return infoUser;
        }

        [HttpPost]
        public object CountNortify()
        {
            List<object> listunpObject = new List<object>();
            InfoUser infoUser = GetUserInfo();
            if (infoUser.Role == "ИНЖЕНЕР")
            {
                int count_f = _datamanager.Reports.GetAllReports().Where(x => x.IdUser == infoUser.IdUser && !_datamanager.Maintenances.GetMaintenanceById(x.MaintenanceId).Status).Count();
                List<object> listunpObject_f = new List<object>();
                listunpObject_f.Add(count_f);
                listunpObject_f.Add(infoUser.IdUser);
                listunpObject_f.Add(infoUser.Role);
                return listunpObject_f;
            }
            else
            {
                int count = _datamanager.Reports.GetAllReports().Where(x=>x.MaintenanceId == _datamanager.Maintenances.GetMaintenanceById(x.MaintenanceId).Id && !_datamanager.Maintenances.GetMaintenanceById(x.MaintenanceId).Status).Count();
                listunpObject.Add(count);
                listunpObject.Add(infoUser.IdUser);
                listunpObject.Add(infoUser.Role);

            }
            
            return listunpObject;
        }

        [HttpGet]
        public IActionResult IndexNortf() {
            UnplannedMainViewForUser unplannedMainViewForUser = new UnplannedMainViewForUser();

            ApplicationUser user = _authDbContext.ApplicationUser.FirstOrDefault(x => x.UserName == User.Identity.Name);

            InfoUser infoUser = GetUserInfo();
            unplannedMainViewForUser.FullName = infoUser.FullName;
            unplannedMainViewForUser.IdUser = infoUser.IdUser;
            unplannedMainViewForUser.Role = infoUser.Role;

            List <UnplannedMainView> unplannedMains = new List<UnplannedMainView>();
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
                    NumHours = main.NumberHours.ToString(),
                    Appointed = _datamanager.Reports.AvailabilityMain(main.Id) != null ? true : false,
                };

                unplannedMains.Add(unplanned);
            }
            
            if(unplannedMainViewForUser.Role == "ИНЖЕНЕР")
            {
                List<Report> repuser = _datamanager.Reports.GetAllReports().Where(x => x.IdUser == user.Id).ToList();
                List<UnplannedMainView> planed = new List<UnplannedMainView>();

                foreach(var rep in repuser)
                {
                    Maintenance main = _datamanager.Maintenances.GetMaintenanceById(rep.MaintenanceId);
                    if (!main.Status)
                    {
                        Equipment eq = _datamanager.Equipments.GetEquipmentById(main.EquipmentId);
                        Group gr = _datamanager.Groups.GetGroupById(eq.GroupId);
                        string namest = _datamanager.Stations.GetStationName(gr.StationId);

                        UnplannedMainView unplanned = new UnplannedMainView()
                        {
                            Id = main.Id,
                            Header = main.IsUnplanned == true ? "Внеплановый" : "Плановый",
                            Description = main.Description != "" ? main.Description : "---",
                            EmplName = eq.Name,
                            StatName = namest,
                        };

                        planed.Add(unplanned);
                    }
                }
                unplannedMainViewForUser.UnplannedMainViews = planed;
            }
            else
            {
                unplannedMainViewForUser.UnplannedMainViews = unplannedMains;
            }

            return View(unplannedMainViewForUser);
        }

        [HttpGet]
        public IActionResult Fix(int id)
        {
            if (id == 0)
                return BadRequest();

            Maintenance main = _datamanager.Maintenances.GetMaintenanceById(id);
            main.Status = true;
            _datamanager.Maintenances.SaveMaintenance(main);

            Report rep = _datamanager.Reports.AvailabilityMain(main.Id);
            rep.DateDefacto = DateOnly.FromDateTime(DateTime.Now);

            _datamanager.Reports.SaveReport(rep);
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
            List<bool> appointedlist = new List<bool>();


            foreach (var item in lstunpla)
            {
                desc.Add(item.Description);
                head.Add(item.Heding);
                idMain.Add(item.MaintenancesID);

                Maintenance main = _datamanager.Maintenances.GetMaintenanceById(item.MaintenancesID);
                Equipment eq = _datamanager.Equipments.GetEquipmentById(main.EquipmentId);
                Group gr = _datamanager.Groups.GetGroupById(eq.GroupId);
                string namest = _datamanager.Stations.GetStationName(gr.StationId);
                bool appinted = _datamanager.Reports.AvailabilityMain(main.Id) != null ? true : false;
                eqnamelist.Add(eq.Name);
                stnamelist.Add(namest);
                appointedlist.Add(appinted);
            }

            listunpObject.Add(head);
            listunpObject.Add(desc);
            listunpObject.Add(idMain);
            listunpObject.Add(eqnamelist);
            listunpObject.Add(stnamelist);
            listunpObject.Add(appointedlist);
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
            List<bool> appointedlist = new List<bool>();
            string stname = _datamanager.Stations.GetStationName(_datamanager.Groups.GetGroupById(grid).StationId);


            var Listeq = _datamanager.Equipments.GetEquipmentsByIdGroup(grid).ToList();

            foreach (var item in Listeq)
            {
                var listplan = _datamanager.UpcomingMaintenance.GetUpmainByEquipId(item.Id);

                foreach (var item2 in listplan)
                {
                    desc.Add("---");
                    head.Add("Плановый ремонт");
                    idMain.Add(item2.MaintenancesID);
                    eqnamelist.Add(item.Name);
                    appointedlist.Add(_datamanager.Reports.AvailabilityMain(item2.MaintenancesID) != null ? true : false);

                    listunpObject.Add(head);
                    listunpObject.Add(desc);
                    listunpObject.Add(idMain);
                    listunpObject.Add(eqnamelist);
                    listunpObject.Add(stname);
                    listunpObject.Add(appointedlist);

                }

            }
            return listunpObject;
        }

        [HttpPost]
        public List<object> SelGroupsCount(int stid)
        {
            List<object> selGroupsCount = new List<object>();
            List<int> ids = new List<int>();
            List<string> names = new List<string>();
            List<int> counts = new List<int>();

            var groups = _datamanager.Groups.GetAllGroupsByStId(false, stid);
            foreach (var group in groups)
            {
                ids.Add(group.Id);
                names.Add(group.Name);
                counts.Add(_datamanager.UpcomingMaintenance.GetUpcomingMaintenanceCountGroupId(group.Id));
            }

            selGroupsCount.Add(ids);
            selGroupsCount.Add(names);
            selGroupsCount.Add(counts);

            return selGroupsCount;
        }

        [HttpGet]
        public IActionResult EqsGroup(int id)
        {
            try
            {
                var eqs = _datamanager.UpcomingMaintenance.GetUpcomingMaintenanceGroupId(id, true).ToList();
                List<EqPlan> eqPlans = new List<EqPlan>();
                foreach (var eq in eqs)
                {
                    if(eq.Maintenance.Status == false)
                    {
                        EqPlan eqPlan = new EqPlan()
                        {
                            Id = eq.Maintenance.Id,
                            Name = _datamanager.Equipments.GetEquipmentById(eq.Maintenance.EquipmentId).Name,
                            FactoryNumber = _datamanager.Equipments.GetEquipmentById(eq.Maintenance.EquipmentId).FactoryNumber,
                            Type = _datamanager.Equipments.GetEquipmentById(eq.Maintenance.EquipmentId).Type,
                            MainDateName = eq.Maintenance.Name + ' ' + eq.Maintenance.DateMaintenance.ToString(),
                            MainId = eq.Maintenance.Id,
                            Appointed = _datamanager.Reports.AvailabilityMain(eq.Maintenance.Id) != null ? true : false,
                        };
                        eqPlans.Add(eqPlan);
                    }
                }

                EqupsGroup equpsGroup = new EqupsGroup()
                {
                    NameGroup = _datamanager.Groups.GetGroupById(id).Name,
                    Equipments = eqPlans
                };
                return View(equpsGroup);
            }
            catch (Exception e)
            {

                return RedirectToRoute(new { controller = "Excel", action = "FailureView" });
            }
            
        }

        [HttpGet]
        public IActionResult Fix2(int id)
        {
            if (id == 0)
                return BadRequest();

            Maintenance main = _datamanager.Maintenances.GetMaintenanceById(id);
            main.Status = true;
            _datamanager.Maintenances.SaveMaintenance(main);

            Report rep = _datamanager.Reports.AvailabilityMain(main.Id);
            rep.DateDefacto = rep.DateDefacto = DateOnly.FromDateTime(DateTime.Now);

            _datamanager.Reports.SaveReport(rep);

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
            int ideq = _datamanager.Maintenances.GetMaintenanceById(id).EquipmentId;
            int idgr = _datamanager.Equipments.GetEquipmentById(ideq).GroupId;
            return RedirectToRoute(new { controller = "UpcomingMaintenance", action = "EqsGroup", id = idgr });
        }
    }
}
