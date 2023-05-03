using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Areas.WeatherForecast.Models.Schemas;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CapstoneProject.Models.Schemas;
using Employee = CapstoneProject.Databases.Schemas.System.Employee.Employees;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CapstoneProject.Areas.WeatherForecast.Models
{
   
    public interface IWeatherForecastModel
    {
        /// <summary>
        /// Get Weather Forecast
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<WeatherForecasts>> Get(SearchConditon searchConditon);
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<Employee>> GetEmployees();
    }
    public class WeatherForecastModel : CapstoneProjectModels, IWeatherForecastModel
    {
        private readonly ILogger<WeatherForecastModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastModel(
            IConfiguration configuration,
            ILogger<WeatherForecastModel> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        public async Task<IEnumerable<WeatherForecasts>> Get(SearchConditon searchConditon)
        {
            string method = GetActualAsyncMethodName();
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                var ListWeatherForecast = Enumerable.Range(1, 5).Select(index => new WeatherForecasts
                {
                    Date = DateTime.Now.AddDays(index),
                    TemperatureC = Random.Shared.Next(-20, 55),
                    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                }).ToArray();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return ListWeatherForecast;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get Weather Forecast Error: {ex}");
                throw ex;
            }
        }
        public async Task<IEnumerable<Employee>> GetEmployees()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                IEnumerable<Employee> employees = await _context.Employee.Where(x => !x.DelFlag).ToListAsync();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get Employee Forecast Error: {ex}");
                throw ex;
            }
        }
    }
}
