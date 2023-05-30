using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Customer.Models.Schemas;
using CapstoneProject.Areas.Customer.Models;
using CapstoneProject.Commons.Schemas;

namespace CapstoneProject.Areas.Customer.Controllers
{
    [Route(("api/{api_version:apiVersion}/customer"))]
    public class CustomerController : CustomerAreaController
	{
        private readonly ICustomerModel _customerModel;

        public CustomerController(
            IServiceProvider provider,
            ICustomerModel customerModel
        ) : base(provider)
        {
            _customerModel = customerModel ?? throw new ArgumentNullException(nameof(customerModel));
        }
        /// <summary>
        /// Lấy tất cả customer theo điều kiện tìm kiếm
        /// <para>Created at: 25/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-customer")]
        public async Task<ActionResult> GetEmployee([FromQuery] SearchCondition searchConditon)
        {
            try
            {
                return Ok(await _customerModel.GetCustomer(searchConditon));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

