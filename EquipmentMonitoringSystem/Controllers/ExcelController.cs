using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using OfficeOpenXml;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize(Roles = "Admin,Главный механик")]
    public class ExcelController : Controller
	{
		private readonly DataManager _datamanager;
		private readonly ServicesManager _servicesmanager;

		public ExcelController(DataManager datamanager, ServicesManager servicesmanager)
		{
			_datamanager = datamanager;
			_servicesmanager = servicesmanager;
		}

		[HttpGet]
		public IActionResult ImportExcel()
		{
			return View();
		}


        [HttpPost]
        public IActionResult ImportExcel(IFormFile uploadedFile)
        {
            if (ModelState.IsValid)
            {
                ExcelStationsInfo infosts = new ExcelStationsInfo();
                List<StationExcelModel> Stations = new List<StationExcelModel>();
                if (uploadedFile.Length > 0)
                {
                    var stream = uploadedFile.OpenReadStream();
                    infosts.FileName = uploadedFile.FileName;
                    try
                    {
                        using (var package = new ExcelPackage(stream))
                        {
                            // Проходим все листы
                            for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
                            {
                                StationExcelModel station = new StationExcelModel();
                                var worksheet = package.Workbook.Worksheets[i];//Берем текущий лист
                                station.Name = worksheet.Name;
                                //Ставим недоверенную станцию
                                station.Checkst = false;
                                var rowCount = worksheet.Dimension.Rows; //Берем количество строк

                                for (var row = 11; row <= rowCount; row++)
                                {
                                    if (row == 51)
                                    {
                                        int ut = 74;
                                    }
                                    try
                                    {
                                        //Проверяем группа или оборудование
                                        if (worksheet.Cells[row, 2].Value?.ToString() == "1" && worksheet.Cells[row, 1].Value?.ToString() != null)
                                        {
                                            GroupExcelModel group = new GroupExcelModel();
                                            var groupname = worksheet.Cells[row, 1].Value?.ToString();
                                            group.Name = groupname;
                                            row++;
                                            //Пока не дошли до следующей группы, выполняем действия
                                            while (worksheet.Cells[row, 2].Value?.ToString() != "1" && worksheet.Cells[row, 1].Value?.ToString() != null)
                                            {
                                                var NameEmpls = worksheet.Cells[row, 5].Value?.ToString();
                                                if (NameEmpls != null)
                                                {
                                                    List<string> Names = NameEmpls.Split('\n').ToList();
                                                    List<EquipmentExcelModel> eqs = new List<EquipmentExcelModel>();
                                                    foreach (var item in Names)
                                                    {
                                                        EquipmentExcelModel eq = new EquipmentExcelModel();
                                                        eq.Name = item;
                                                        eqs.Add(eq);
                                                    }

                                                    //Собираем все виды ТО оборудования
                                                    List<MaintenanceEditModel> maintenances = new List<MaintenanceEditModel>();

                                                    for (int j = 12; j <= 23; j++)
                                                    {
                                                        if (worksheet.Cells[row, j].Value?.ToString() != "" && worksheet.Cells[row, j].Value?.ToString() != null)
                                                        {
                                                            MaintenanceEditModel m = new MaintenanceEditModel();
                                                            m.Name = worksheet.Cells[row, j].Value?.ToString();
                                                            //Определяем месяц
                                                            if (j - 11 < 10)
                                                                m.DateMaintenance = DateTime.Now.Year.ToString() + "-0" + (j - 11).ToString() + "-01";
                                                            else
                                                                m.DateMaintenance = DateTime.Now.Year.ToString() + "-" + (j - 11).ToString() + "-01";

                                                            maintenances.Add(m);
                                                        }
                                                    }

                                                    foreach (var mn in maintenances)
                                                    {
                                                        switch (mn.Name)
                                                        {
                                                            case "ТО  1":
                                                                mn.NumberHours = worksheet.Cells[row, 6].Value?.ToString() != "" ? Convert.ToDouble(worksheet.Cells[row, 6].Value?.ToString()) : 0;
                                                                break;
                                                            case "ТО  3":
                                                                mn.NumberHours = worksheet.Cells[row, 7].Value?.ToString() != "" ? Convert.ToDouble(worksheet.Cells[row, 7].Value?.ToString()) : 0;
                                                                break;
                                                            case "ТО  4":
                                                                mn.NumberHours = worksheet.Cells[row, 8].Value?.ToString() != "" ? Convert.ToDouble(worksheet.Cells[row, 8].Value?.ToString()) : 0;
                                                                break;
                                                            case "ТО  6":
                                                                mn.NumberHours = worksheet.Cells[row, 9].Value?.ToString() != "" ? Convert.ToDouble(worksheet.Cells[row, 9].Value?.ToString()) : 0;
                                                                break;
                                                            case "ТО12":
                                                                mn.NumberHours = worksheet.Cells[row, 10].Value?.ToString() != "" ? Convert.ToDouble(worksheet.Cells[row, 10].Value?.ToString()) : 0;
                                                                break;
                                                            case "ТО24":
                                                                mn.NumberHours = worksheet.Cells[row, 11].Value?.ToString() != "" ? Convert.ToDouble(worksheet.Cells[row, 11].Value?.ToString()) : 0;
                                                                break;
                                                            default:
                                                                mn.NumberHours = 0;
                                                                break;
                                                        }
                                                    }

                                                    foreach (var eq in eqs)
                                                    {
                                                        eq.Maintenances.AddRange(maintenances);
                                                    }
                                                    group.Equipments.AddRange(eqs);
                                                    
                                                }
                                                row++;
                                            }
                                            row--;
                                            station.Groups.Add(group);
                                            infosts.GrCount += 1;
                                            infosts.EqCount += group.Equipments.Count;
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                        Console.WriteLine("Что-то пошло не так");
                                        return RedirectToAction("FailureView");
                                    }
                                }
                                Stations.Add(station);
                                infosts.StCount += 1;
                                station = new StationExcelModel();
                            }
                        }

                        foreach (var station in Stations)
                        {
                            _servicesmanager.Stations.SaveStationExcelModelToDb(station);
                        }
                        infosts.CheckFile = _datamanager.FileNames.CheckFileName(infosts.FileName);
                        
                        return View(infosts);

                    }
                    catch (Exception e)
                    {
                        return RedirectToAction("FailureView");
                    }
                }
            }
            return View();
        }

		[HttpGet]
		public IActionResult FailureView()
		{
			return View();
		}

		[HttpGet]
		public IActionResult DeleteNewSts()
		{
            _datamanager.Stations.DeleteStationCheck();
            return RedirectToAction("ImportExcel");
        }

        [HttpPost]
        public IActionResult DownNewSts(ExcelStationsInfo model)
        {
            if (!model.CheckFile)
            {
                FileNames fileName = new FileNames()
                {
                    Id = 0,
                    Name = model.FileName,
                };
                _datamanager.FileNames.SaveFileName(fileName);
            }
            _datamanager.Stations.UpdateStationCheck();

            // название процедуры
            string sqlExpression = "update_upmaintenance";
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

            return RedirectToAction("SuccessfullyView");
        }

        [HttpGet]
		public IActionResult SuccessfullyView()
		{
			return View();
		}
	}
}
