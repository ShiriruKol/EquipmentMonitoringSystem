using EquipmentMonitoringSystem.DataLayer;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Npgsql;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Xml.Linq;

namespace EquipmentMonitoringSystem.TimerTask
{
    public class BackgroundTask : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (!stoppingToken.IsCancellationRequested) {

                // название процедуры
                string sqlExpression = "update_upmaintenance";
                string connectionString = @"Server=localhost;Database=PulseRigDB;Port=5432;User Id=postgres;Password=12K345i678R9;";
                using (NpgsqlConnection connection = new NpgsqlConnection(connectionString))
                {
                    connection.Open();
                    NpgsqlCommand command = new NpgsqlCommand(sqlExpression, connection);
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var result = command.ExecuteScalar();

                    Console.WriteLine("Успешный вызов процедуры", result);
                }
                await Task.Delay(86400000);
            }
        }
    }
}
