using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.PresentationLayer.Services
{
    public class GroupService
    {
        private DataManager _dataManager;
        private EquipmentService _equipmentService;
        public GroupService(DataManager dataManager)
        {
            this._dataManager = dataManager;
            _equipmentService = new EquipmentService(dataManager);
        }

        /*public List<GroupViewIndexModel> GetGroupsList()
        {
            var _dirs = _dataManager.Groups.GetAllGroups(true);

            List<GroupViewIndexModel> _modelsList = new List<GroupViewIndexModel>();
            foreach (var item in _dirs)
            {
                GroupModel _tmp = GroupDBToViewModelById(item.Id);
                GroupViewIndexModel _model = new GroupViewIndexModel();
                _model.Groups.Add(_tmp);
                _modelsList.Add();
            }
            return _modelsList;
        }*/

        public GroupModel GroupDBToViewModelById(int grouId)
        {
            Group _directory = _dataManager.Groups.GetGroupById(grouId);
            int _eqcount = _dataManager.Groups.GetEqCountbyGroup(grouId);
            return new GroupModel() { Group = _directory, EqCount = _eqcount};
        }

        public GroupInfoModel GroupDBToViewInfoModelById(int grouId)
        {
            Group _directory = _dataManager.Groups.GetGroupById(grouId, true);
            return new GroupInfoModel() { Group = _directory };
        }

        public GroupEditViewModel GetGroupEditModel(int directoryid = 0)
        {
            if (directoryid != 0)
            {
                var _dirDB = _dataManager.Groups.GetGroupById(directoryid);
                var _dirEditModel = new GroupEditViewModel()
                {
                    Id = _dirDB.Id,
                    Name = _dirDB.Name,
                    Description = _dirDB.Description,
                };
                return _dirEditModel;
            }
            else { return new GroupEditViewModel() { }; }
        }

        public GroupModel SaveAlbumEditModelToDb(GroupEditViewModel groupEditModel)
        {
            Group _groupDbModel;

            if (groupEditModel.Id != 0)
            {
                _groupDbModel = _dataManager.Groups.GetGroupById(groupEditModel.Id);
            }
            else
            {
                _groupDbModel = new Group();
            }
            _groupDbModel.Name = groupEditModel.Name;
            _groupDbModel.Description = groupEditModel.Description;
            _groupDbModel.StationId = groupEditModel.StationId;

            _dataManager.Groups.SaveGroup(_groupDbModel);

            return GroupDBToViewModelById(_groupDbModel.Id);
        }

        public void DeleteGroup(int id)
        {
            Group _directoryDbModel;
            if (id != 0)
            {
                _directoryDbModel = _dataManager.Groups.GetGroupById(id);
            }
            else
            {
                _directoryDbModel = new Group();
            }
            _dataManager.Groups.DeleteGroup(_directoryDbModel);
        }

        public GroupEditViewModel CreateNewAlbumEditModel(int id)
        {
            return new GroupEditViewModel() { };
        }

        public GroupEditViewModel CreateNewAlbumEditModel()
        {
            return new GroupEditViewModel() { };
        }
    }
}
