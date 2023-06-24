using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;

namespace EquipmentMonitoringSystem.BuissnesLayer
{
    public class DataManager
    {
        private IStationRepository _stationRepository;
        private IGroupRepository _groupRepository;
        private IEquipmentRepository _equipmentRepository;
        private IMaintenanceRepository _maintenanceRepository;
        private IUpcomingMaintenanceRepository _upcomingMaintenanceRepository;
        private INortifyRepository _nortifyRepository;
        private IFileNamesRepository _fileNamesRepository;
        private IReportRepository _reportRepository;

        public DataManager(IStationRepository stationRepository, IGroupRepository groupRepository,
            IEquipmentRepository equipmentRepository, IMaintenanceRepository maintenanceRepository,
            IUpcomingMaintenanceRepository upcomingMaintenanceRepository, INortifyRepository nortifyRepository, IFileNamesRepository fileNamesRepository, IReportRepository reportRepository)
        {
            _stationRepository = stationRepository;
            _groupRepository = groupRepository;
            _equipmentRepository = equipmentRepository;
            _maintenanceRepository = maintenanceRepository;
            _upcomingMaintenanceRepository = upcomingMaintenanceRepository;
            _nortifyRepository = nortifyRepository;
            _fileNamesRepository = fileNamesRepository;
            _reportRepository = reportRepository;
        }

        public IStationRepository Stations { get { return _stationRepository; } }
        public IGroupRepository Groups { get { return _groupRepository; } }
        public IEquipmentRepository Equipments { get { return _equipmentRepository; } }
        public IMaintenanceRepository Maintenances { get { return _maintenanceRepository; } }
        public IUpcomingMaintenanceRepository UpcomingMaintenance{ get { return _upcomingMaintenanceRepository; } }
        public INortifyRepository Nortify { get { return _nortifyRepository; } }
        public IFileNamesRepository FileNames { get {  return _fileNamesRepository; } }
        public IReportRepository Reports { get { return _reportRepository; } }
    }
}
