namespace EquipmentMonitoringSystem.DataLayer
{
    public static class SampleData
    {
        public static void InitData(EFDBContext context)
        {
            if (!context.Stations.Any())
            {
                context.Stations.Add(new Entityes.Station() { Name = "Станция №1" });
                context.SaveChanges();
                context.Groups.Add(new Entityes.Group() { Name = "Группа №1", Description = "---", StationId = context.Stations.First().Id });
                context.Groups.Add(new Entityes.Group() { Name = "Группа №2", Description = "---", StationId = context.Stations.First().Id });
                context.SaveChanges();

                context.Equipments.Add(new Entityes.Equipment()
                {
                    Name = "Оборудование №1",
                    Type = "---",
                    FactoryNumber = "---",
                    GroupId = context.Groups.First().Id,
                });

                context.Equipments.Add(new Entityes.Equipment()
                {
                    Name = "Оборудование №1",
                    Type = "---",
                    FactoryNumber = "---",
                    GroupId = context.Groups.OrderBy(gr => gr.Id).LastOrDefault().Id,
                });
                context.SaveChanges();
            }
        }
    }
}
