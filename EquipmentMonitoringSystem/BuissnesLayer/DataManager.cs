﻿using EquipmentMonitoringSystem.BuissnesLayer.Interfaces;

namespace EquipmentMonitoringSystem.BuissnesLayer
{
    public class DataManager
    {
        private IStationRepository _stationRepository;
        private IGroupRepository _groupRepository;
        private IEquipmentRepository _equipmentRepository;
        private IMaintenanceRepository _maintenanceRepository;

        public DataManager(IStationRepository stationRepository, IGroupRepository groupRepository, IEquipmentRepository equipmentRepository, IMaintenanceRepository maintenanceRepository)
        {
            _stationRepository = stationRepository;
            _groupRepository = groupRepository;
            _equipmentRepository = equipmentRepository;
            _maintenanceRepository = maintenanceRepository;
        }

        public IStationRepository Stations { get { return _stationRepository; } }
        public IGroupRepository Groups { get { return _groupRepository; } }
        public IEquipmentRepository Equipments { get { return _equipmentRepository; } }
        public IMaintenanceRepository Maintenances { get { return _maintenanceRepository; } }
    }
}
