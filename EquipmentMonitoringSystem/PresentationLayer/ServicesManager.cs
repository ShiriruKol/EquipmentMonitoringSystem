using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.PresentationLayer.Services;

namespace EquipmentMonitoringSystem.PresentationLayer
{
    public class ServicesManager
    {
        DataManager _dataManager;
        private StationService _stationService;
        private GroupService _groupService;
        private EquipmentService _equipmentService;

        public ServicesManager(
            DataManager dataManager
            )
        {
            _dataManager = dataManager;
            _stationService = new StationService(_dataManager);
            _groupService = new GroupService(_dataManager);
            _equipmentService = new EquipmentService(_dataManager);
        }
        public StationService Stations { get { return _stationService; } }
        public GroupService Groups { get { return _groupService; } }
        public EquipmentService Equipments { get { return _equipmentService; } }
    }
}
