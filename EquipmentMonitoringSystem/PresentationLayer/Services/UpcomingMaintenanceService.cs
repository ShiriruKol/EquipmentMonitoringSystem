using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.PresentationLayer.Services
{
    public class UpcomingMaintenanceService
    {
        private DataManager _dataManager;
        private MaintenanceService _groupService;
        public UpcomingMaintenanceService(DataManager dataManager)
        {
            this._dataManager = dataManager;
            _groupService = new MaintenanceService(dataManager);
        }

        public List<UpcomingViewMaintenanceModel> GetUpcomingList()
        {
            var _dirs = _dataManager.UpcomingMaintenanceRepository.GetAllUpcomingMaintenance();
            List<UpcomingViewMaintenanceModel> _modelsList = new List<UpcomingViewMaintenanceModel>();
            foreach (var item in _dirs)
            {
                _modelsList.Add(UpcomingViewMaintenanceDBToViewIndexModelById(item.Id));
            }
            return _modelsList;
        }
        public UpcomingViewMaintenanceModel UpcomingViewMaintenanceDBToViewIndexModelById(int Id)
        {
            var _directory = _dataManager.UpcomingMaintenanceRepository.GetUpcomingMaintenanceById(Id);

            string namemain = _dataManager.Maintenances.GetMaintenanceById(_directory.MaintenancesID).Name;
            string datemain = _dataManager.Maintenances.GetMaintenanceById(_directory.MaintenancesID).DateMaintenance.ToString();
            string nameequip = _dataManager.Equipments.GetEquipmentById(_dataManager.Maintenances.GetMaintenanceById(_directory.MaintenancesID).EquipmentId).Name;

            return new UpcomingViewMaintenanceModel() { 
                NameMain = namemain,
                Date = datemain,
                NameEquip = nameequip,
            };
        }
    }
}
