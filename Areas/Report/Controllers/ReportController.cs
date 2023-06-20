using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Report.Models.Schemas;
using CapstoneProject.Areas.Report.Models;
using CapstoneProject.Commons.Schemas;

namespace CapstoneProject.Areas.Report.Controllers
{
    [Route(("api/{api_version:apiVersion}/report"))]
    public class ReportController : ReportAreaController
	{
		private readonly IReportModel _reportModel;
		public ReportController(
			IReportModel reportModel,
            IServiceProvider provider
        ) : base(provider)
        {
            _reportModel = reportModel ?? throw new ArgumentNullException(nameof(reportModel));
        }
        /// <summary>
        /// Báo cáo doanh thu theo chi nhánh
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("revenue-by-branch")]
        public async Task<ActionResult> GetRevenueByBranch([FromQuery] SearchCondition searchCondition)
        {
            try
            {
                return Ok(await _reportModel.GetRevenueByBranch(searchCondition));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Báo cáo doanh thu theo ngày tháng năm 
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("revenue-by-date")]
        public async Task<ActionResult> GetRevenueByDate([FromQuery] SearchCondition searchCondition)
        {
            try
            {
                return Ok(await _reportModel.GetRevenueByDate(searchCondition));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Báo cáo doanh thu theo ngày tháng năm 
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("revenue-by-order")]
        public async Task<ActionResult> GetRevenueByOrder([FromQuery] SearchCondition searchCondition)
        {
            try
            {
                return Ok(await _reportModel.GetRevenueByOrder(searchCondition));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Báo cáo doanh thu theo film
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("revenue-by-film")]
        public async Task<ActionResult> GetRevenueByFilm([FromQuery] SearchCondition searchCondition)
        {
            try
            {
                return Ok(await _reportModel.GetRevenueByFilm(searchCondition));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

