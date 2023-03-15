using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.PresentationLayer.Services
{
    public class UpcomingMaintenanceService
    {
        private DataManager _dataManager;
        private GroupService _groupService;
        public UpcomingMaintenanceService(DataManager dataManager)
        {
            this._dataManager = dataManager;
            _groupService = new GroupService(dataManager);
        }

        public List<UpcomingViewMaintenanceModel> GetStationList()
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

            return new UpcomingViewMaintenanceModel() { UpcomingMaintenance  = _directory };
        }
    }
}
