using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace EquipmentMonitoringSystem.Controllers
{
    [Authorize]
    public class ExcelController : Controller
	{
		private readonly DataManager _datamanager;
		private readonly ServicesManager _servicesmanager;

		public ExcelController(DataManager datamanager, ServicesManager servicesmanager)
		{
			_datamanager = datamanager;
			_servicesmanager = servicesmanager;
		}

        public IActionResult ImportExcel()
        {
            return View();
        }

		[HttpPost]
		public IActionResult ImportExcel(IFormFile uploadedFile)
		{
			if (ModelState.IsValid)
			{

				List<StationExcelModel> stations = new List<StationExcelModel>();
				if (uploadedFile.Length > 0)
				{
					var stream = uploadedFile.OpenReadStream();
					try
					{
						using (var package = new ExcelPackage(stream))
						{
							// Проходим все листы
							for (int i = 0; i < package.Workbook.Worksheets.Count - 1; i++)
							{
								StationExcelModel station = new StationExcelModel();
								var worksheet = package.Workbook.Worksheets[i];//Берем текущий лист
								station.Name = worksheet.Name;
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
															if(j - 11 < 10)
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
										}

									}
									catch (Exception ex)
									{
										Console.WriteLine("Что-то пошло не так");
									}
								}
								stations.Add(station);
								station = new StationExcelModel();
							}
						}
						foreach (var station in stations)
						{
							_servicesmanager.Stations.SaveStationExcelModelToDb(station);
						}
						return View(stations);

					}
					catch (Exception e)
					{
						return View();
					}
				} 
            }
			return View();
		}

        [HttpPost]
        public IActionResult LoadingToDb(List<StationExcelModel> stations)
        {
			try
			{
				foreach (var station in stations)
				{
					_servicesmanager.Stations.SaveStationExcelModelToDb(station);
				}
			}
			catch (Exception e)
			{
                return View();
            }

			return Redirect("/Station/Index"); ;
        }
    }
}
