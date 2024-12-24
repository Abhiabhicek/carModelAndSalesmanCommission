using CarModelManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CarModelManagement.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class SalesmanReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SalesmanReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        [HttpGet("commission")]
        public IActionResult GetCommissionReport()
        {
            var report = new List<SalesmanCommission>();

            using (var connection = GetConnection())
            {
                connection.Open();

                // Fetch sales data
                var salesDataCommand = new SqlCommand("SELECT * FROM SalesData", connection);
                var salesData = new List<SalesData>();
                using (var reader = salesDataCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        salesData.Add(new SalesData
                        {
                            Salesman = reader.GetString(1),
                            Class = reader.GetString(2),
                            Brand = reader.GetString(3),
                            NumberOfCarsSold = reader.GetInt32(4),
                            ModelPrice = reader.GetDecimal(5)
                        });
                    }
                }

                // Fetch previous year sales data
                var previousYearSalesCommand = new SqlCommand("SELECT * FROM PreviousYearSales", connection);
                var previousYearSales = new Dictionary<string, decimal>();
                using (var reader = previousYearSalesCommand.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        previousYearSales[reader.GetString(0)] = reader.GetDecimal(1);
                    }
                }

                // Calculate commission
                foreach (var data in salesData)
                {
                    var fixedCommission = GetFixedCommission(data.Brand, data.ModelPrice);
                    var classCommission = GetClassCommission(data.Brand, data.Class, data.ModelPrice);
                    var additionalCommission = GetAdditionalCommission(data.Salesman, data.Class, previousYearSales);

                    var totalCommission = fixedCommission + classCommission + additionalCommission;
                    report.Add(new SalesmanCommission
                    {
                        Salesman = data.Salesman,
                        Brand = data.Brand,
                        TotalCommission = totalCommission
                    });
                }
            }

            return Ok(report);
        }

        private decimal GetFixedCommission(string brand, decimal modelPrice)
        {
            if (brand == "Audi" && modelPrice > 25000) return 800;
            if (brand == "Jaguar" && modelPrice > 35000) return 750;
            if (brand == "Land Rover" && modelPrice > 30000) return 850;
            if (brand == "Renault" && modelPrice > 20000) return 400;
            return 0;
        }

        private decimal GetClassCommission(string brand, string carClass, decimal modelPrice)
        {
            decimal percentage = carClass switch
            {
                "A" when brand == "Audi" => 0.08m,
                "A" when brand == "Jaguar" => 0.06m,
                "B" => 0.05m,
                "C" => 0.04m,
                _ => 0
            };

            return modelPrice * percentage;
        }

        private decimal GetAdditionalCommission(string salesman, string carClass, Dictionary<string, decimal> previousYearSales)
        {
            if (carClass == "A" && previousYearSales.TryGetValue(salesman, out var totalSales) && totalSales > 500000)
            {
                return totalSales * 0.02m;
            }
            return 0;
        }
    }

}