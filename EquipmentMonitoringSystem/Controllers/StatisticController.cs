using Microsoft.AspNetCore.Mvc;

namespace EquipmentMonitoringSystem.Controllers
{
    public class StatisticController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
