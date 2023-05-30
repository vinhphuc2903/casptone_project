using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.CinemaRoom.Models.Schemas;
using CapstoneProject.Areas.CinemaRoom.Models;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Areas.Customer.Models;

namespace CapstoneProject.Areas.CinemeRoom.Controllers
{
    [Route(("api/{api_version:apiVersion}/cinemaRoom"))]
    public class CinemaRoomController : CinemeRoomAreaController
	{
        private readonly ICinemaRoomModel _cinemaRoomModel;

        public CinemaRoomController(
            IServiceProvider provider,
            ICinemaRoomModel cinemaRoomModel
        ) : base(provider)
        {
            _cinemaRoomModel = cinemaRoomModel ?? throw new ArgumentNullException(nameof(cinemaRoomModel));
        }
        /// <summary>
        /// Lấy tất cả cinemaroom theo điều kiện tìm kiếm
        /// <para>Created at: 25/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-cinemaRoom")]
        public async Task<ActionResult> GetAllCinemaRoom([FromQuery] SearchCondition searchConditon)
        {
            try
            {
                return Ok(await _cinemaRoomModel.GetAllCinemaRoom(searchConditon));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

