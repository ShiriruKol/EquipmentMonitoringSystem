using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;
using System.IO;

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

        public List<StationIndexViewModel> GetStationList()
        {
            var _dirs = _dataManager.Stations.GetAllStations();
            List<StationIndexViewModel> _modelsList = new List<StationIndexViewModel>();
            foreach (var item in _dirs)
            {
                _modelsList.Add(StationDBToViewIndexModelById(item.Id));
            }
            return _modelsList;
        }

        public StationIndexViewModel StationDBToViewIndexModelById(int stationId)
        {
            var _directory = _dataManager.Stations.GetStationById(stationId);
            int _count = _dataManager.Stations.GetNumGroupsStationId(stationId);
            
            return new StationIndexViewModel() { Station = _directory, NumberGroups = _count };
        }

        public StationInfoViewModel StationDBToInfoViewModel(int stationId)
        {
            var _name = _dataManager.Stations.GetStationName(stationId);
            List<Group> _groups = _dataManager.Stations.GetGroupsByStation(stationId);

            return new StationInfoViewModel() { StationName = _name, Groups = _groups };
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
        public StationIndexViewModel SaveStationEditModelToDb(StationEditModel stationEditModel)
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

            return StationDBToViewIndexModelById(_stationDbModel.Id);
        }

        public void SaveStationExcelModelToDb(StationExcelModel stationExcelModel)
        {
            try
            {
                DataLayer.Entityes.Station _stationDbModel;
                _stationDbModel = new DataLayer.Entityes.Station();
                _stationDbModel.Name = stationExcelModel.Name;

                List<Group> groups = new List<Group>();
                foreach (var group in stationExcelModel.Groups)
                {
                    Group gr = new Group();
                    gr.Name = group.Name;
                    gr.Description = " ";
                    List<Equipment> equipments = new List<Equipment>();
                    foreach (var equipment in group.Equipments)
                    {
                        Equipment eq = new Equipment();
                        eq.Name = equipment.Name;
                        eq.FactoryNumber = " ";
                        eq.Type = " ";

                        List<Maintenance> maintenances = new List<Maintenance>();
                        foreach (var maintenance in equipment.Maintenances)
                        {
                            Maintenance ma = new Maintenance();
                            ma.Name = maintenance.Name;
                            ma.Description = maintenance.Description;
                            ma.Status = maintenance.Status;
                            ma.DateMaintenance = DateOnly.Parse(maintenance.DateMaintenance);
                            ma.NumberHours = maintenance.NumberHours;
                            maintenances.Add(ma);
                        }
                        eq.Maintenances = maintenances;
                        equipments.Add(eq);
                    }
                    gr.Equipments = equipments;
                    groups.Add(gr);
                }
                _stationDbModel.Groups = groups;

                _dataManager.Stations.SaveStation(_stationDbModel);
            }
            catch (Exception e)
            {
                throw;
            }
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
