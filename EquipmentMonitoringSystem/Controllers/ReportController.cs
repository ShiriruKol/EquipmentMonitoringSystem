using EquipmentMonitoringSystem.Areas.Identity.Data;
using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Xml.Linq;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize(Roles = "Admin,Главный механик")]
    public class ReportController : Controller
    {
        private readonly DataManager _datamanager;
        private  readonly AuthDbContext _authDbContext;
        public  readonly ServicesManager _servicesManager;

        public ReportController(DataManager datamanager, AuthDbContext authDbContext, ServicesManager servicesManager)
        {
            _datamanager = datamanager;
            _authDbContext = authDbContext;
            _servicesManager = servicesManager;
        }

        [HttpGet]
        public IActionResult Index()
        {
            List<ReportViewModel> reportList = new List<ReportViewModel>();
            List<Report> reports = _datamanager.Reports.GetAllReports(true).ToList();
            foreach (Report report in reports)
            {
                ApplicationUser user = _authDbContext.ApplicationUser.AsNoTracking().FirstOrDefault(x => x.Id == report.IdUser);
                Maintenance maintenance = _datamanager.Maintenances.GetMaintenanceById(report.MaintenanceId);
                string status = maintenance.Status ? "Выполнено" : "Не выполнено";
                ReportViewModel reportView = new ReportViewModel
                {
                    Name = report.Name,
                    Date = report.Date.ToString(),
                    Description = report.Description,
                    Id = report.Id,
                    Main = maintenance.Name + " - " + maintenance.NumberHours + "час.",
                    UserName = user.FullName,
                    Status = status,
                    DateDefacto = report.DateDefacto.ToString(),
                };
                reportList.Add(reportView);
            }
            return View(reportList);
        }

        [HttpGet]
        public ActionResult Appoint(int id)
        {
            if(id == 0)
                return RedirectToAction("Index");

            
            List<SelectListItem> users = UsersToSelectedList();

            CreateReportViewModel newreport = new CreateReportViewModel()
            {
                Users = users,
                Date = _datamanager.Maintenances.GetMaintenanceById(id).DateMaintenance.ToString(),
                mainId = _datamanager.Maintenances.GetMaintenanceById(id).Id,
                EqName = _datamanager.Equipments.GetEquipmentById(_datamanager.Maintenances.GetMaintenanceById(id).EquipmentId).Name,
                Name = _datamanager.Maintenances.GetMaintenanceById(id).Name,
            };

            return View(newreport);
        }

        [HttpPost]
        public ActionResult Appoint(CreateReportViewModel model)
        {
            if (string.IsNullOrEmpty(model.Name) || string.IsNullOrWhiteSpace(model.Name.Trim()))
                ModelState.AddModelError(nameof(model.Name), "Указано некорректное поле!");
            if (string.IsNullOrEmpty(model.EqName) || string.IsNullOrWhiteSpace(model.EqName.Trim()))
                ModelState.AddModelError(nameof(model.EqName), "Указано некорректное поле!");
            if (string.IsNullOrEmpty(model.Date) || string.IsNullOrWhiteSpace(model.Date.Trim()))
                ModelState.AddModelError(nameof(model.Date), "Указано некорректное поле!");
            if (model.idUser == "0")
                ModelState.AddModelError(nameof(model.idUser), "Выберите сотрудника");

            if (!ModelState.IsValid)
            {
                // Список не передается, поэтому следует получить его
                model.Users = UsersToSelectedList();
                return View(model);
            }

            Report report = new Report();
            report.Date = DateOnly.Parse(model.Date);
            report.Name = model.EqName + ' ' + model.Name;
            report.MaintenanceId = _datamanager.Maintenances.GetMaintenanceById(model.mainId).Id;
            report.Description = "---";
            report.IdUser = model.idUser;   

            
            _datamanager.Reports.SaveReport(report);
            return RedirectToAction("Index");
        }

        private List<SelectListItem> UsersToSelectedList()
        {
            List<SelectListItem> usersAll = new List<SelectListItem>();
            var usr = _authDbContext.ApplicationUser.ToList();
            string nameengId = _authDbContext.Roles.FirstOrDefault(x => x.NormalizedName == "ИНЖЕНЕР").Id;
            var idusersEng = _authDbContext.UserRoles.Where(x => x.RoleId == nameengId).ToList();

            List<ApplicationUser> usrsEng = new List<ApplicationUser>();

            foreach (var item in idusersEng)
            {
                if (_authDbContext.ApplicationUser.FirstOrDefault(x => x.Id == item.UserId) != null)
                {
                    ApplicationUser user = _authDbContext.ApplicationUser.FirstOrDefault(x => x.Id == item.UserId);
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
