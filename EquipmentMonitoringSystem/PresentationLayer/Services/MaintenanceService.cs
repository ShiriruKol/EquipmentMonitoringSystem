using EquipmentMonitoringSystem.BuissnesLayer;
using EquipmentMonitoringSystem.DataLayer.Entityes;
using EquipmentMonitoringSystem.PresentationLayer.Models;

namespace EquipmentMonitoringSystem.PresentationLayer.Services
{
    public class MaintenanceService
    {
        private DataManager _dataManager;
        public MaintenanceService(DataManager dataManager)
        {
            _dataManager = dataManager;
        }

        /*public void SaveMaintenanceEditModelToDB(MaintenanceEditModel model, int eqId)
        {
            Maintenance maintenance;
            if(model.Id == 0)
            {
                model.
                equipment = _dataManager.Equipments.GetEquipmentById(editModel.Id);
            }
        }*/
    }
}
