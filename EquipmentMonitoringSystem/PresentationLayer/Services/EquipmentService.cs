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

        public EquipmentEditViewModel GetEquipmentEditModel(int Id)
        {
            var _dbModel = _dataManager.Equipments.GetEquipmentById(Id);
            var _editModel = new EquipmentEditViewModel()
            {
                Id = _dbModel.Id = _dbModel.Id,
                Name = _dbModel.Name,
                FactoryNumber = _dbModel.FactoryNumber,
                Type = _dbModel.Type,
                ListTO = _dbModel.arrayRepair.ToList(),
                MauthTO = _dbModel.arrayMouthRepair.ToList(),
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
            equipment.GroupId = editModel.GroupId;
            equipment.arrayMouthRepair = editModel.MauthTO.ToArray();
            equipment.arrayRepair = editModel.ListTO.ToArray();
            _dataManager.Equipments.SaveEquipment(equipment);
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
