using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace EquipmentMonitoringSystem.Controllers
{
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

        /*Вывод всех имен оборудований*/
        List<string> ParsStr(string equps)
		{

			List<string> Names = new List<string>();
			string Name = "";
			if (equps.IndexOf('\n') != -1)
			{
				for (int i = 0; i < equps.Length - 1; i++)
				{
					if (equps[i] == '\n')
					{
						Names.Add(Name);
						i++;
						Name = "";
					}
					else
						Name += equps[i];
				}
				Names.Add(Name + equps[equps.Length - 1]);
			}
			else
				Names.Add(equps);

			return Names;
		}

		[HttpPost]
		public IActionResult ImportExcel(IFormFile uploadedFile)
		{
			if (ModelState.IsValid)
			{
				List<StationEditModel> stations = new List<StationEditModel>();
				List<GroupEditViewModel> groups = new List<GroupEditViewModel>();
				List<EquipmentEditViewModel> equipments = new List<EquipmentEditViewModel>();
				if (uploadedFile.Length > 0)
				{
					var stream = uploadedFile.OpenReadStream();

					bool checkGroup = true;
					try
					{
						using (var package = new ExcelPackage(stream))
						{
							// Проходим все листы
							for (int i = 0; i < package.Workbook.Worksheets.Count; i++)
							{
								StationEditModel station = new StationEditModel();
								GroupEditViewModel group = new GroupEditViewModel();
								var worksheet = package.Workbook.Worksheets[i];//Берем текущий лист
								station.Name = worksheet.Name;
								var rowCount = worksheet.Dimension.Rows; //Берем количество строк

								for (var row = 11; row <= rowCount - 6; row++)
								{
									try
									{

										//Проверяем группа или оборудование
										if (worksheet.Cells[row, 2].Value?.ToString() == "1")
										{
											if (checkGroup == false)
											{
												//Если группа существует, то добавляем ее в список групп
												if (group.Name != null)
												{
													groups.Add(group);
													group = new GroupEditViewModel();
												}
											}
											var GroupName = worksheet.Cells[row, 1].Value?.ToString();
											group.Name = GroupName;
											checkGroup = false;
										}
										else if (worksheet.Cells[row, 2].Value?.ToString() != null)
										{
											var NameEmpls = worksheet.Cells[row, 5].Value?.ToString();
											if (NameEmpls != null)
											{
												List<string> Names = ParsStr(NameEmpls);
												foreach (var item in Names)
												{
													EquipmentEditViewModel eq = new EquipmentEditViewModel();
													eq.Name = item;
													equipments.Add(eq);
												}
											}

										}

									}
									catch (Exception ex)
									{
										Console.WriteLine("Что-то пошло не так");
									}
								}
								stations.Add(station);
								station = new StationEditModel();
							}
						}
						var o = 8;
						return View();

					}
					catch (Exception e)
					{
						return View();
					}
				}

			}
			return View();
		}
	}
}
