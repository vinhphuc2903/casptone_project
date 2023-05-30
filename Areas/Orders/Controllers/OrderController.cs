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
    }
}

