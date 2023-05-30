using System;
using CapstoneProject.Commons.CodeMaster;
using CapstoneProject.Models;
using CapstoneProject.Models.Schemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using CapstoneProject.Areas.Customer.Models.Schemas;
using CapstoneProject.Commons;

namespace CapstoneProject.Areas.Customer.Models
{
	public interface ICustomerModel
	{
        Task<IEnumerable<CustomerInfo>> GetCustomer(SearchCondition searchCondition);
    }
	public class CustomerModel : CapstoneProjectModels, ICustomerModel
    {
        private readonly ILogger<CustomerModel> _logger;
        private string _className = "";
        private readonly IConfiguration _configuration;

        public CustomerModel(
            IConfiguration configuration,
            IServiceProvider provider,
            ILogger<CustomerModel> logger
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        // Lấy danh sách customer
        public async Task<IEnumerable<CustomerInfo>> GetCustomer(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                IEnumerable<CustomerInfo> customers = await _context.Users
                    .Where(x => !x.DelFlag
                        && x.Roles.Any(x => x.RoleId == R001.USER.CODE)
                        && (String.IsNullOrEmpty(searchCondition.Name)
                            || EF.Functions.Collate(x.Name.Replace(" ", ""), "Latin1_General_CI_AI").Contains(EF.Functions.Collate(searchCondition.Name.Replace(" ", ""), "Latin1_General_CI_AI")))
                        && (String.IsNullOrEmpty(searchCondition.Id.ToString())
                            || x.Id == searchCondition.Id
                        )
                    )
                    .Include(x => x.Roles)
                    .Include(x => x.UserPoint)
                    .Select(x => new CustomerInfo()
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Phone = Security.Base64Decode(x.Phone),
                        Email = x.Email,
                        DateOfBirth = x.DateOfBirth,
                        Gender = x.Gender,
                        Point = x.UserPoint.Point,
                        Rank = x.UserPoint.RankName
                    })
                    .ToListAsync();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get Employee Forecast Error: {ex}");
                throw ex;
            }
        }
    }
}

