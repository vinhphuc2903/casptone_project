
using System;
using System.Web;
using CapstoneProject.Models;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Areas.Orders.Models.Schemas;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Commons.Schemas;
using ShowTimeDb = CapstoneProject.Databases.Schemas.System.Ticket.ShowTime;
using OrderDb = CapstoneProject.Databases.Schemas.System.Order.Orders;
using OrderFoodDetailDb = CapstoneProject.Databases.Schemas.System.Order.OrderFoodDetail;
using OrderTicketDetailDb = CapstoneProject.Databases.Schemas.System.Order.OrderTicketDetail;
using TicketDb = CapstoneProject.Databases.Schemas.System.Ticket.Tickets;
using FoodDb = CapstoneProject.Databases.Schemas.System.Food.Foods;
using PaymentDb = CapstoneProject.Databases.Schemas.System.Order.Payment;
//using PaymentDb = CapstoneProject.Databases.Schemas.System.P;
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
using CapstoneProject.Databases.Schemas.System.Order;
using CapstoneProject.Services;
using CapstoneProject.Databases.Schemas.System.Ticket;
using CapstoneProject.Databases.Schemas.System.Food;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using Amazon.Runtime.Internal;
using System.Net;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Principal;
using System.Threading.Tasks;

namespace CapstoneProject.Areas.Orders.Models
{
    public interface IOrderModels
    {
        Task<List<FoodData>> GetListFood();
        Task<ResponseInfo> CreateOrder(OrderData orderData);
        /// <summary>
        /// Lấy danh sách đơn hàng
        /// </summary>
        /// <param name="ShowTimeId"></param>
        /// <returns></returns>
        Task<ListOrderDetail> GetListOrderData(SearchCondition searchCondition);
        /// <summary>
        /// Lấy chi tiết đơn hàng
        /// </summary>
        /// <param name="ShowTimeId"></param>
        /// <returns></returns>
        Task<OrderDetails> GetOrderDetail(int id);
        /// <summary>
        /// Lấy đường dẫn thanh toán cho đơn hàng
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        Task<ResponseInfo> GetLinkPaymentOrder(int OrderId);
        /// <summary>
        /// Cập nhật lại đơn hàng đã thanh toán
        /// </summary>
        /// <param name="OrderId"></param>
        /// <returns></returns>
        Task<ResponseInfo> PaymentOrder(PaymentInfo PaymentInfo);
    }
    public class OrderModels : CapstoneProjectModels, IOrderModels
    {
        private readonly ILogger<OrderModels> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private readonly IIdentityService _indentityService;

        public OrderModels (
            IConfiguration configuration,
            ILogger<OrderModels> logger,
            IIdentityService identityService,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _indentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
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
                        Id = x.Id,
                        NameOption1 = x.NameOption1,
                        NameOption2 = x.NameOption2,
                        Price = x.Price,
                        SalePrice = x.SalePrice,
                        OriginPrice = x.OriginPrice,
                        ImageLink = x.ImageLink,
                        Status = x.Status,
                        Type = x.Type
                    }).ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }

        public async Task<ResponseInfo> CreateOrder(OrderData orderData)
        {
            string method = GetActualAsyncMethodName();
            ResponseInfo responseInfo = new ResponseInfo();
            IDbContextTransaction transaction = null;
            try
            {
                //Kiểm tra có chọn vé chưa
                if(orderData.ListTicketId.Count() == 0)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.CHOSE_TICKET;
                    return responseInfo;
                }    
                // Thông tin user
                string userId = _indentityService.GetUserId();
                if(String.IsNullOrEmpty(userId))
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.LOGIN_IS_FALSE;
                    return responseInfo;
                }    
                // Thông tin xuất chiếu
                ShowTimeDb showTime = await _context.ShowTime
                    .Where(x => !x.DelFlag && x.Id == orderData.ShowTimeId)
                    .FirstOrDefaultAsync();
                if(showTime == null)
                {
                    responseInfo.Code = CodeResponse.HAVE_ERROR;
                    responseInfo.MsgNo = MSG_NO.SHOWTIME_NOT_EXITED;
                    return responseInfo;
                }
                string prefix = "ORD";
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string orderCode = $"{prefix}{showTime.BranchId}{showTime.Id}{timestamp}";

                int originPrice = 0;
                int salePrice = 0;
                int price = 0;
                int total = 0;
                // Tạo order mới
                OrderDb orders = new OrderDb()
                {
                    UserId = Int32.Parse(userId),
                    BranchId = showTime.BranchId,
                    ShowTimeId = showTime.Id,
                    OrderCode = orderCode,
                    Status = "10",
                    PaymentId = 0
                };
                await _context.Orders.AddAsync(orders);
                await _context.SaveChangesAsync();
                //await _context.SaveChangesAsync();
                List<OrderTicketDetailDb> listTicket = new List<OrderTicketDetailDb>();
                // Tạo OrderTicket cho danh sách vé
                foreach(var ticketId in orderData.ListTicketId)
                {
                    TicketDb tickets = await _context.Ticket.Where(x => !x.DelFlag && x.Id == ticketId && x.Type == "10").FirstOrDefaultAsync();
                    //Kiểm tra trạng thái vé
                    if(tickets == null)
                    {
                        responseInfo.Code = CodeResponse.HAVE_ERROR;
                        responseInfo.MsgNo = MSG_NO.TICKET_NOT_DEFINE;
                        orders.DelFlag = true;
                        await _context.SaveChangesAsync();
                        return responseInfo;
                    }
                    tickets.Type = "30";
                    tickets.OrderAt = DateTime.Now;
                    //Tạo orderDetail vé
                    OrderTicketDetailDb orderTicketDetail = new OrderTicketDetailDb()
                    {
                        TicketId = tickets.Id,
                        OrderId = orders.Id,
                        Price = tickets.Price,
                        SalePrice = tickets.Price,
                        DiscountPrice = 0,
                        PaymentPrice = tickets.Price
                    };
                    originPrice += tickets.Price;
                    salePrice += tickets.Price;
                    total += tickets.Price;
                    price += tickets.Price;
                    listTicket.Add(orderTicketDetail);
                }
                await _context.OrderTicketDetail.AddRangeAsync(listTicket);
                List<OrderFoodDetail> orderFoodDetails = new List<OrderFoodDetailDb>();
                // Tạo OrderTicket cho danh sách vé
                foreach (var fooddata in orderData.ListFoodDetail)
                {
                    FoodDb food = await _context.Foods.Where(x => !x.DelFlag && x.Id == fooddata.FoodId  && x.Status == "10").FirstOrDefaultAsync();
                    //Kiểm tra trạng thái đồ ắn
                    if (food == null)
                    {
                        responseInfo.Code = CodeResponse.HAVE_ERROR;
                        responseInfo.MsgNo = MSG_NO.FOOD_IS_PAUSE;
                        orders.DelFlag = true;
                        await _context.SaveChangesAsync();
                        return responseInfo;
                    }
                    //Tạo orderDetail đồ ăn
                    OrderFoodDetailDb orderFoodDetailDb = new OrderFoodDetailDb()
                    {
                        OrderId = orders.Id,
                        FoodId = food.Id,
                        Quantity = fooddata.Quantity,
                        Price = food.Price,
                        OriginPrice = food.OriginPrice,
                        SalePrice = food.SalePrice,
                        PaymentPrice = food.SalePrice * fooddata.Quantity,
                    };
                    originPrice += food.OriginPrice.Value * fooddata.Quantity;
                    salePrice += food.SalePrice.Value * fooddata.Quantity;
                    price += food.Price * fooddata.Quantity;
                    total += food.SalePrice.Value * fooddata.Quantity;
                    orderFoodDetails.Add(orderFoodDetailDb);
                }
                await _context.OrderFoodDetail.AddRangeAsync((IEnumerable<OrderFoodDetailDb>)orderFoodDetails);
                //Tạo payment
                PaymentDb payment = new PaymentDb()
                {
                    OrderId = orders.Id,
                    OriginPrice = originPrice,
                    SalePrice = salePrice,
                    Total = total,
                    Price = price,
                    DiscountPrice = 0,
                    Status = "10",
                    UserId = Int32.Parse(userId),
                };
                orders.PaymentId = payment.Id;
                await _context.Payments.AddAsync(payment);
                await _context.SaveChangesAsync();
                transaction = await _context.Database.BeginTransactionAsync();
                transaction?.CommitAsync();
                //string paymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?";
                //string inputData =
                //    "vnp_Amount=" + (total * 100).ToString()
                //    + "&vnp_BankCode=ATM"
                //    + "&vnp_Command=pay"
                //    + "&vnp_CreateDate=" + payment.CreatedAt.ToString("yyyyMMddHHmmss")
                //    + "&vnp_CurrCode=VND"
                //    + "&vnp_ExpireDate=" + payment.CreatedAt.AddMinutes(15).ToString("yyyyMMddHHmmss")
                //    + "&vnp_IpAddr=" + _indentityService.GetClientIpAddress()
                //    + "&vnp_Locale=vn"
                //    + "&vnp_OrderInfo=Thanh+toan+don+hang+" + orderCode
                //    + "&vnp_OrderType=billpayment"
                //    + "&vnp_ReturnUrl=" + WebUtility.UrlEncode("http://localhost:3000/")
                //    + "&vnp_TmnCode=YMNCTC5Y"
                //    + "&vnp_TxnRef=" + orders.Id.ToString()
                //    + "&vnp_Version=2.1.0";
                //string vnp_secureHash = CreateRequestUrl(inputData);
                //    //+ "&vnp_SecureHash=2dcb3e77d15b26ade1b94b8aeb5b709676914aec7e3adb7aacb386471af16556d274bad5f49db6d045297b302606a1edcda7365a8e731c030bee7f8f056b60ff";
                responseInfo.Data.Add("OrderId", orders.Id.ToString());
                return responseInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }

        public string CreateRequestUrl(string inputData)
        {
            string vnp_HashSecret = "QVYDODQCSAUXNUZYHNDMUDRPROUUWJLN";
            string vnp_SecureHash = HmacSHA512(vnp_HashSecret, inputData);
            inputData += "&vnp_SecureHash=" + vnp_SecureHash;

            return inputData;
        }

        public static String HmacSHA512(string key, string inputData)
        {
            var hash = new StringBuilder();
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);
            byte[] inputBytes = Encoding.UTF8.GetBytes(inputData);
            using (var hmac = new HMACSHA512(keyBytes))
            {
                byte[] hashValue = hmac.ComputeHash(inputBytes);
                foreach (var theByte in hashValue)
                {
                    hash.Append(theByte.ToString("x2"));
                }
            }

            return hash.ToString();
        }
        public async Task<ListOrderDetail> GetListOrderData(SearchCondition searchCondition)
        {
            string method = GetActualAsyncMethodName();
            ResponseInfo responseInfo = new ResponseInfo();
            IDbContextTransaction transaction = null;
            try
            {
                if(searchCondition == null)
                {
                    searchCondition = new SearchCondition();
                }
                // Thông tin user
                string userId = _indentityService.GetUserId();
                if (String.IsNullOrEmpty(userId))
                {
                    return null;
                }
                ListOrderDetail listOrderDetail = new ListOrderDetail();
                var data = _context.Orders
                    .Where(x => !x.DelFlag
                        && (String.IsNullOrEmpty(searchCondition.OrderId.ToString())
                            || searchCondition.OrderId == x.Id)
                        && userId == x.UserId.ToString()
                        )
                    .OrderByDescending(x => x.Id);
                if(data == null)
                {
                    return listOrderDetail;
                }
                var total = data.Count();
                listOrderDetail.Paging = new Commons.Paging(total, searchCondition.CurrentPage, searchCondition.PageSize);
                listOrderDetail.ListOrderDetails = await data.Skip((searchCondition.CurrentPage - 1) * searchCondition.PageSize)
                    .Take(searchCondition.PageSize)
                    .Select(x => new OrderDetails()
                    {
                        OrderId = x.Id,
                        OrderCode = x.OrderCode,
                        FilmName = x.ShowTime.Film.Name,
                        CreatedAt = x.CreatedAt,
                        StatusOrder = x.Status,
                        StatusPayment = x.Payments.Status,
                        Total = x.Payments.Total,
                        PaymentAt = x.Payments.PaymentAt
                    })
                    .ToListAsync();
                return listOrderDetail;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<OrderDetails> GetOrderDetail(int id)
        {
            string method = GetActualAsyncMethodName();
            ResponseInfo responseInfo = new ResponseInfo();
            IDbContextTransaction transaction = null;
            try
            {
                // Thông tin user
                string userId = _indentityService.GetUserId();
                if (String.IsNullOrEmpty(userId))
                {
                    return null;
                }
                OrderDetails data = await _context.Orders
                    .Include(x => x.Payments)
                    .Include(x => x.ShowTime)
                    .Include(x => x.ShowTime.Film)
                    .Where(x => !x.DelFlag
                        && x.Id == id)
                    .Select(x => new OrderDetails()
                    {
                        OrderId = x.Id,
                        OrderCode = x.OrderCode,
                        FilmName = x.ShowTime.Film.Name,
                        CreatedAt = x.CreatedAt,
                        StatusOrder = x.Status,
                        StatusPayment = x.Payments.Status,
                        Total = x.Payments.Total,
                        PaymentAt = x.Payments.PaymentAt,
                        ChairTicket = x.OrderTicketDetails.Where(x => !x.DelFlag)
                            .Select(
                                z => z.Ticket.Name
                            ).ToList(),
                        FoodOrders = x.OrderFoodDetails.Where(x => !x.DelFlag)
                            .Select(
                                f => new FoodOrder()
                                {
                                    Quantity = f.Quantity,
                                    Name = f.Foods.NameOption1
                                }
                            ).ToList(),
                    }).FirstOrDefaultAsync();
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }

        public async Task<ResponseInfo> GetLinkPaymentOrder(int OrderId)
        {
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                var dateTimeNow = DateTimeOffset.Now.AddMinutes(-5);
                OrderDb orders = await _context.Orders
                    .Include(x => x.Payments)
                    .Where(x => !x.DelFlag && x.Id == OrderId && x.CreatedAt >= dateTimeNow)
                    .FirstOrDefaultAsync();
                if (orders == null)
                 {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.OVERDUE_PAYMENT_TIME;
                    await _context.SaveChangesAsync();
                    return response;
                }
                string paymentUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html?";
                string inputData =
                    "vnp_Amount=" + (orders.Payments.Total * 100).ToString()
                    + "&vnp_BankCode=ATM"
                    + "&vnp_Command=pay"
                    + "&vnp_CreateDate=" + orders.CreatedAt.ToString("yyyyMMddHHmmss")
                    + "&vnp_CurrCode=VND"
                    + "&vnp_ExpireDate=" + orders.CreatedAt.AddMinutes(5).ToString("yyyyMMddHHmmss")
                    + "&vnp_IpAddr=" + WebUtility.UrlEncode("171.225.184.148") //_indentityService.GetClientIpAddress())
                    + "&vnp_Locale=vn"
                    + "&vnp_OrderInfo=" + WebUtility.UrlEncode("Thanh toan don hang " + orders.OrderCode)
                    + "&vnp_OrderType=billpayment"
                    + "&vnp_ReturnUrl=" + WebUtility.UrlEncode("http://localhost:3000/pagenull")
                    + "&vnp_TmnCode=YMNCTC5Y"
                    + "&vnp_TxnRef=" + WebUtility.UrlEncode(orders.Id.ToString())
                    + "&vnp_Version=2.1.0";
                //string inputData =
                    //"vnp_Amount=" + (orders.Payments.Total * 100).ToString()
                    //+ "&vnp_BankCode=VNPAYQR"
                    //+ "&vnp_Command=pay"
                    //+ "&vnp_CreateDate=" + DateTimeOffset.Now.ToString("yyyyMMddHHmmss")
                    //+ "&vnp_CurrCode=VND"
                    //+ "&vnp_ExpireDate=" + DateTimeOffset.Now.AddMinutes(15).ToString("yyyyMMddHHmmss")
                    //+ "&vnp_IpAddr=" + WebUtility.UrlEncode(_indentityService.GetClientIpAddress())
                    //+ "&vnp_Locale=vn"
                    //+ "&vnp_OrderInfo=Thanh+toan+don+hang+" + orders.OrderCode
                    //+ "&vnp_OrderType=billpayment"
                    //+ "&vnp_ReturnUrl=" + WebUtility.UrlEncode("http://localhost:3000/")
                    //+ "&vnp_TmnCode=YMNCTC5Y"
                    //+ "&vnp_TxnRef=" + orders.Id.ToString()
                    //+ "&vnp_Version=2.1.0";
                string vnp_secureHash = CreateRequestUrl(inputData);
                response.Data.Add("linkpayment", paymentUrl + vnp_secureHash);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> PaymentOrder(PaymentInfo paymentInfo)
        {
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                OrderDb orders = await _context.Orders
                    .Include(x => x.Payments)
                    .Include(x => x.OrderTicketDetails)
                    .ThenInclude(x => x.Ticket)
                    .Where(x => !x.DelFlag && x.Id.ToString() == paymentInfo.Vnp_TxnRef)
                    .FirstOrDefaultAsync();
                if(paymentInfo.Vnp_ResponseCode != "00" || paymentInfo.Vnp_TransactionStatus != "00")
                {
                    orders.Status = "20";
                    orders.Payments.Status = "20";
                    foreach (var orderTicketDetail in orders.OrderTicketDetails)
                    {
                        // Cập nhật trường Type của Ticket thành 20
                        orderTicketDetail.Ticket.Type = "10";
                    }
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.OVERDUE_PAYMENT_TIME;
                    await _context.SaveChangesAsync();
                    return response;
                }
                foreach (var orderTicketDetail in orders.OrderTicketDetails)
                {
                    // Cập nhật trường Type của Ticket thành 20
                    orderTicketDetail.Ticket.Type = "20";
                }
                orders.Status = "30";
                orders.Payments.Status = "30";
                orders.Payments.PaymentAt = DateTime.Now;
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get List Film Error: {ex}");
                throw ex;
            }
        }
    }
}
