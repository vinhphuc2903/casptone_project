using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Employee.Models.Schemas;
using CapstoneProject.Areas.Employee.Models;

namespace CapstoneProject.Areas.Employee.Controllers
{
    [Route(("api/{api_version:apiVersion}/employee"))]
    public class EmployesController : EmployeesAreaController
    {
        private readonly IEmployeesModel _employeesModel;
        public EmployesController(
             IEmployeesModel employeesModel,
             IServiceProvider provider
        ) : base(provider)
        {
            _employeesModel = employeesModel ?? throw new ArgumentNullException(nameof(employeesModel));
        }

        /// <summary>
        /// Lấy tất cả nhân viên theo điều kiện tìm kiếm
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-employee")]
        public async Task<ActionResult> GetEmployee()
        {
            try
            {
                return Ok(await _employeesModel.GetEmployees());
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}