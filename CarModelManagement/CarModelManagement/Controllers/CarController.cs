using CarModelManagement.Model;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace CarModelManagement.Controllers
{
    [ApiController]
    [Route("api/cars")]
    public class CarController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CarController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private SqlConnection GetConnection()
        {
            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCars()
        {
            var cars = new List<CarModel>();
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT * FROM CarModels", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cars.Add(new CarModel
                            {
                                Id = reader.GetInt32(0),
                                Brand = reader.GetString(1),
                                Class = reader.GetString(2),
                                ModelName = reader.GetString(3),
                                ModelCode = reader.GetString(4),
                                Description = reader.GetString(5),
                                Features = reader.GetString(6),
                                Price = reader.GetDecimal(7),
                                DateOfManufacturing = reader.GetDateTime(8),
                                Active = reader.GetBoolean(9),
                                SortOrder = reader.GetInt32(10)
                            });
                        }
                    }
                }
            }
            return Ok(cars);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateCar([FromBody] CarModel car)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("INSERT INTO CarModels (Brand, Class, ModelName, ModelCode, Description, Features, Price, DateOfManufacturing, Active, SortOrder) VALUES (@Brand, @Class, @ModelName, @ModelCode, @Description, @Features, @Price, @DateOfManufacturing, @Active, @SortOrder)", connection))
                {
                    command.Parameters.AddWithValue("@Brand", car.Brand);
                    command.Parameters.AddWithValue("@Class", car.Class);
                    command.Parameters.AddWithValue("@ModelName", car.ModelName);
                    command.Parameters.AddWithValue("@ModelCode", car.ModelCode);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Features", car.Features);
                    command.Parameters.AddWithValue("@Price", car.Price);
                    command.Parameters.AddWithValue("@DateOfManufacturing", car.DateOfManufacturing);
                    command.Parameters.AddWithValue("@Active", car.Active);
                    command.Parameters.AddWithValue("@SortOrder", car.SortOrder);

                    command.ExecuteNonQuery();
                }
            }
            return Ok("Car model created successfully.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="car"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateCar(int id, [FromBody] CarModel car)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("UPDATE CarModels SET Brand = @Brand, Class = @Class, ModelName = @ModelName, ModelCode = @ModelCode, Description = @Description, Features = @Features, Price = @Price, DateOfManufacturing = @DateOfManufacturing, Active = @Active, SortOrder = @SortOrder WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.Parameters.AddWithValue("@Brand", car.Brand);
                    command.Parameters.AddWithValue("@Class", car.Class);
                    command.Parameters.AddWithValue("@ModelName", car.ModelName);
                    command.Parameters.AddWithValue("@ModelCode", car.ModelCode);
                    command.Parameters.AddWithValue("@Description", car.Description);
                    command.Parameters.AddWithValue("@Features", car.Features);
                    command.Parameters.AddWithValue("@Price", car.Price);
                    command.Parameters.AddWithValue("@DateOfManufacturing", car.DateOfManufacturing);
                    command.Parameters.AddWithValue("@Active", car.Active);
                    command.Parameters.AddWithValue("@SortOrder", car.SortOrder);

                    command.ExecuteNonQuery();
                }
            }
            return Ok("Car model updated successfully.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteCar(int id)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                using (var command = new SqlCommand("DELETE FROM CarModels WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
            return Ok("Car model deleted successfully.");
        }
    }
}