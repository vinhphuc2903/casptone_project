using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Areas.Employee.Models.Schemas;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CapstoneProject.Models.Schemas;
using EmployeeData = CapstoneProject.Databases.Schemas.System.Employee.Employees;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace CapstoneProject.Areas.Employee.Models
{
   
    public interface IEmployeesModel
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EmployeeData>> GetEmployees();
    }
    public class EmployeesModel : CapstoneProjectModels, IEmployeesModel
    {
        private readonly ILogger<EmployeesModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public EmployeesModel(
            IConfiguration configuration,
            ILogger<EmployeesModel> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        
        public async Task<IEnumerable<EmployeeData>> GetEmployees()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                IEnumerable<EmployeeData> employees = await _context.Employee.Where(x => !x.DelFlag).ToListAsync();
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
