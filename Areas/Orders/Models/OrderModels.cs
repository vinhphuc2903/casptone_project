
//using System;
using CapstoneProject.Models;
using CapstoneProject.Areas.Orders.Models.Schemas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Commons.Schemas;
using ShowTimeDb = CapstoneProject.Databases.Schemas.System.Ticket.ShowTime;
using OrderDb = CapstoneProject.Databases.Schemas.System.Orders.Orders;
using OrderFoodDetailDb = CapstoneProject.Databases.Schemas.System.Orders.OrderFoodDetail;
using OrderTicketDetailDb = CapstoneProject.Databases.Schemas.System.Orders.OrderTicketDetail;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using CapstoneProject.Commons.Enum;
//using System.Drawing;
using System.Drawing.Imaging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;
using CapstoneProject.Models.Schemas;

namespace CapstoneProject.Areas.Orders.Models
{
    public interface IOrderModels
    {
        Task<List<FoodData>> GetListFood();
        //Task<ResponseInfo> CreateOrder(OrderData orderData);
        //Task<string> UploadImageToS3(IFormFile imageFile);
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

        //public async Task<ResponseInfo> CreateOrder(OrderData orderData)
        //{
        //    string method = GetActualAsyncMethodName();
        //    ResponseInfo response = new ResponseInfo();
        //    IDbContextTransaction transaction = null;
        //    try
        //    {
        //        ShowTimeDb showTime = await _context.ShowTime
        //            .Where(x => !x.DelFlag && x.Id == orderData.ShowTimeId)
        //            .FirstOrDefaultAsync();

        //        foreach(Order)
        //        return response;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation($"Get List Film Error: {ex}");
        //        throw ex;
        //    }
        //}
  
    }
}

