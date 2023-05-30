
using System;
using CapstoneProject.Models;
using CapstoneProject.Models.Schemas;
using CapstoneProject.Areas.Orders.Models.Schemas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Linq;

namespace CapstoneProject.Areas.Orders.Models
{
    public interface IOrderModels
    {
        Task<List<FoodData>> GetListFood();
    }
    public class OrderModels : CapstoneProjectModels, IOrderModels
    {
        private readonly ILogger<OrderModels> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";

        public OrderModels (
            IConfiguration configuration,
            ILogger<OrderModels> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<List<FoodData>> GetListFood()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                List<FoodData> list = await _context.Foods
                    .Where(x => !x.DelFlag)
                    .Select(x => new FoodData()
                    {
                        NameOption1 = x.NameOption1,
                        NameOption2 = x.NameOption2,
                        Price = x.Price
                    }).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
    }
}

