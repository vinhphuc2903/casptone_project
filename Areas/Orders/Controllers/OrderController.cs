using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Orders.Models.Schemas;
using CapstoneProject.Areas.Orders.Models;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Databases.Schemas.System.Film;

namespace CapstoneProject.Areas.Orders.Controllers
{
    [Route(("api/{api_version:apiVersion}/order"))]
    public class OrderController : OrderAreaController
    {
		private readonly IOrderModels _orderModels;

		public OrderController(
            IOrderModels orderModels,
            IServiceProvider provider
        ) : base(provider)
        {
            _orderModels = orderModels ?? throw new ArgumentNullException(nameof(orderModels));
        }
        // <summary>
        /// Lấy danh sách đồ ăn
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-food")]
        public async Task<ActionResult> GetListFood()
        {
            try
            {
                return Ok(await _orderModels.GetListFood());
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        // <summary>
        /// Tạo đơn hàng mới
        /// <para>Created at: 01/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost()]
        public async Task<ActionResult> CreateOrder([FromBody] OrderData orderData)
        {
            try
            {
                return Ok(await _orderModels.CreateOrder(orderData));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy danh sách đơn hàng
        /// <para>Created at: 01/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet()]
        public async Task<ActionResult> GetListOrderData([FromQuery] SearchCondition searchCondition)
        {
            try
            {
                return Ok(await _orderModels.GetListOrderData(searchCondition));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy chi tiết đơn hàng theo id
        /// <para>Created at: 01/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDetailOrderData([FromRoute] int id)
        {
            try
            {
                return Ok(await _orderModels.GetOrderDetail(id));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy link payment theo order Id
        /// <para>Created at: 01/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("get-link-payment")]
        public async Task<ActionResult> GetLinkPaymentOrder([FromQuery] int OrderId)
        {
            try
            {
                return Ok(await _orderModels.GetLinkPaymentOrder(OrderId));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Cập nhật lại đơn hàng đã thanh toán
        /// <para>Created at: 01/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPut("payment-order")]
        public async Task<ActionResult> PaymentOrder([FromBody] PaymentInfo paymentInfo)
        {
            try
            {
                return Ok(await _orderModels.PaymentOrder(paymentInfo));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Cập nhật trạng thái đơn hàng khi thanh toán
        /// <para>Created at: 01/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        //[HttpGet("payment")]
        //public async Task<ActionResult> UpdateStatusPayment([FromRoute] int id)
        //{
        //    try
        //    {
        //        return Ok(await _orderModels.GetOrderDetail(id));
        //    }
        //    catch (Exception e)
        //    {
        //        //await _logService.SaveLogException(e);
        //        return StatusCode(500);
        //    }
        //}
    }
}

