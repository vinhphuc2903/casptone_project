using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Auths;
using CapstoneProject.Areas.ShowTime.Models;
using CapstoneProject.Areas.ShowTime.Models.Schemas;

namespace CapstoneProject.Areas.ShowTime.Controllers
{
    [Auth]
    [Route(("api/{api_version:apiVersion}/showtime"))]
    public class ShowtimeController : ShowtimeAreaController
    {
        private readonly IShowtimeModels _showtimeModels;

        public ShowtimeController(
            IShowtimeModels showtimeModels,
            IServiceProvider provider 
        ) : base(provider)
		{
            _showtimeModels = showtimeModels ?? throw new ArgumentNullException(nameof(showtimeModels));
        }
        /// <summary>
        /// Lấy tất cả customer theo điều kiện tìm kiếm
        /// <para>Created at: 25/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-showtime")]
        public async Task<ActionResult> GetListShowtime([FromQuery] SearchCondition searchConditon)
        {
            try
            {
                return Ok(await _showtimeModels.GetListShowtime(searchConditon));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Thêm showtime và ticket mới
        /// <para>Created at: 17/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost()]
        public async Task<ActionResult> AddNewFilmData([FromBody] ShowTimeInput showTimeInput)
        {
            try
            {
                return Ok(await _showtimeModels.AddShowTime(showTimeInput));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

