using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.PresentationLayer.Services
{
    public class StationService
    {
        private DataManager _dataManager;
        private GroupService _groupService;
        public StationService(DataManager dataManager)
        {
            this._dataManager = dataManager;
            _groupService = new GroupService(dataManager);
        }

        public List<StationViewModel> GetStationList()
        {
            var _dirs = _dataManager.Stations.GetAllStations(true);
            List<StationViewModel> _modelsList = new List<StationViewModel>();
            foreach (var item in _dirs)
            {
                _modelsList.Add(StationDBToViewModelById(item.Id));
            }
            return _modelsList;
        }

        public StationViewModel StationDBToViewModelById(int stationId)
        {
            var _directory = _dataManager.Stations.GetStationById(stationId, true);

            List<GroupViewModel> _materialsViewModelList = new List<GroupViewModel>();
            foreach (var item in _directory.Groups)
            {
                _materialsViewModelList.Add(_groupService.GroupDBToViewModelById(item.Id));
            }
            return new StationViewModel() { Station = _directory, Groups = _materialsViewModelList };
        }
        public StationEditModel GetStationEditModel(int directoryid = 0)
        {
            if (directoryid != 0)
            {
                var _dirDB = _dataManager.Stations.GetStationById(directoryid);
                var _dirEditModel = new StationEditModel()
                {
                    Id = _dirDB.Id,
                    Name = _dirDB.Name,
                };
                return _dirEditModel;
            }
            else { return new StationEditModel() { }; }
        }
        public StationViewModel SaveStationEditModelToDb(StationEditModel stationEditModel)
        {
            DataLayer.Entityes.Station _stationDbModel;
            if (stationEditModel.Id != 0)
            {
                _stationDbModel = _dataManager.Stations.GetStationById(stationEditModel.Id);
            }
            else
            {
                _stationDbModel = new DataLayer.Entityes.Station();
            }
            _stationDbModel.Name = stationEditModel.Name;

            _dataManager.Stations.SaveStation(_stationDbModel);

            return StationDBToViewModelById(_stationDbModel.Id);
        }

        public void DeleteStation(int id)
        {
            Station _directoryDbModel;
            if (id != 0)
            {
                _directoryDbModel = _dataManager.Stations.GetStationById(id);
            }
            else
            {
                _directoryDbModel = new Station();
            }
            _dataManager.Stations.DeleteStation(_directoryDbModel);
        }

        public StationEditModel CreateNewStationEditModel(int id)
        {
            return new StationEditModel() { };
        }

        public StationEditModel CreateNewStationEditModel()
        {
            return new StationEditModel() { };
        }
    }
}
