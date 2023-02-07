

namespace EquipmentMonitoringSystem.DataLayer.Entityes
{
    public class Equipment
    {
        public int Id { get; set; }
        public string Name { get; set; } // Наименование
        public string Type { get; set; } // Тип
        public string FactoryNumber { get; set; } // Заводской номер

        public double[] arrayRepair = new double[5]; // Виды ТО

        public int[] arrayMouthRepair = new int[12]; // Месяцы, когда будут проводится нужные ТО

        public int GroupId { get; set; } // Внешний ключ
        public Group Group { get; set; } // Навигационное свойство
    }
}
