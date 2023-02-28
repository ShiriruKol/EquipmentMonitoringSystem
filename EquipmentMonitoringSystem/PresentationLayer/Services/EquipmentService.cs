using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.PresentationLayer.Services
{
    public class EquipmentService
    {
        private DataManager _dataManager;
        public EquipmentService(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public List<EquipmentViewModel> GetEquipmentList()
        {
            var _dirs = _dataManager.Equipments.GetAllEquipments();
            List<EquipmentViewModel> _modelsList = new List<EquipmentViewModel>();
            foreach (var item in _dirs)
            {
                _modelsList.Add(EquipmentDBModelToView(item.Id));
            }
            return _modelsList;
        }

        public EquipmentViewModel EquipmentDBModelToView(int equipmentId)
        {
            Equipment equipment = _dataManager.Equipments.GetEquipmentById(equipmentId);
            var _model = new EquipmentViewModel()
            {
                Equipment = equipment,
                GroupName = _dataManager.Groups.GetGroupById(equipment.GroupId).Name
            };

            return _model;
        }

        public EquipmentEditViewModel GetEquipmentEditModel(int Id, bool includeMain = false)
        {
            var _dbModel = _dataManager.Equipments.GetEquipmentById(Id, includeMain);

            List<MaintenanceEditModel> listmain = new List<MaintenanceEditModel>();
            foreach(var item in _dbModel.Maintenances)
            {
                MaintenanceEditModel maintenanceEditModel = new MaintenanceEditModel()
                {
                    Name = item.Name,
                    NumberHours = item.NumberHours,
                    DateMaintenance = item.DateMaintenance.ToString(),
                    Status = item.Status,
                    EquipmentId = item.EquipmentId,
                    Id = item.Id
                };
                listmain.Add(maintenanceEditModel);
            }

            var _editModel = new EquipmentEditViewModel()
            {
                Id = _dbModel.Id = _dbModel.Id,
                Name = _dbModel.Name,
                FactoryNumber = _dbModel.FactoryNumber,
                Type = _dbModel.Type,
                Maintenances = listmain
            };
            return _editModel;
        }

        public EquipmentViewModel SaveEquipmentEditModelToDb(EquipmentEditViewModel editModel)
        {
            Equipment equipment;
            if (editModel.Id != 0)
            {
                equipment = _dataManager.Equipments.GetEquipmentById(editModel.Id);
            }
            else
            {
                equipment = new Equipment();
            }
            equipment.Name = editModel.Name;
            equipment.FactoryNumber = editModel.FactoryNumber;
            equipment.Type = editModel.Type;

            List<Maintenance> arrm = new List<Maintenance>();
            foreach(var item in editModel.Maintenances)
            {
                Maintenance maintenance = new Maintenance()
                {
                    Id = item.Id,
                    EquipmentId = item.EquipmentId,
                    Name = item.Name,
                    NumberHours = item.NumberHours,
                    DateMaintenance = DateOnly.Parse(item.DateMaintenance),
                    Status = item.Status,
                    Equipment = equipment
                };
                arrm.Add(maintenance);
            }

            equipment.Maintenances = arrm;
            equipment.GroupId = editModel.GroupId;
            if (equipment.Maintenances[0].Id != null)
            {
                foreach (var item in equipment.Maintenances)
                {
                    _dataManager.Maintenances.SaveMaintenance(item);
                }
                equipment.Maintenances = null;
            }
            else
            {
                _dataManager.Equipments.SaveEquipment(equipment);
            }
            return EquipmentDBModelToView(equipment.Id);
        }

        public void DeleteEquipment(int id)
        {
            Equipment equipment;
            if (id != 0)
            {
                equipment = _dataManager.Equipments.GetEquipmentById(id);
            }
            else
            {
                equipment = new Equipment();
            }
            _dataManager.Equipments.DeleteEquipment(equipment);
        }

        public EquipmentEditViewModel CreateNewEquipmentEditModel(int groupId)
        {
            return new EquipmentEditViewModel() { GroupId = groupId };
        }
    }

}
