using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.CinemaRoom.Models.Schemas;
using CapstoneProject.Areas.CinemaRoom.Models;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Areas.Customer.Models;
using CapstoneProject.Auths;
using CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas;

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
        [Auth]
        [HttpGet("all-cinemaRoom")]
        public async Task<ActionResult> GetAllCinemaRoom([FromQuery] SearchCondition searchConditon)
        {
            try
            {
                return Ok(await _cinemaRoomModel.GetAllCinemaRoomData(searchConditon));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy tất cả branch theo điều kiện tìm kiếm
        /// <para>Created at: 25/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("all-branch")]
        public async Task<ActionResult> GetAllBranches([FromQuery] SearchCondition searchConditon)
        {
            try
            {
                return Ok(await _cinemaRoomModel.GetAllBranches(searchConditon));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Thêm cinemaroom mới
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost()]
        public async Task<ActionResult> AddNewFilmData([FromBody] CinemaRoomDataInput cinemaRoomData)
        {
            try
            {
                return Ok(await _cinemaRoomModel.CreateCinemaRoomData(cinemaRoomData));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Cập nhật cinemaroom mới
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPut()]
        public async Task<ActionResult> UpdateCinemaRoomData([FromBody] CinemaRoomDataInput cinemaRoomData)
        {
            try
            {
                return Ok(await _cinemaRoomModel.UpdateCinemaRoomData(cinemaRoomData));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Xóa cinemaroom
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpDelete()]
        public async Task<ActionResult> DeleteCinema([FromQuery] int id)
        {
            try
            {
                return Ok(await _cinemaRoomModel.DeleteCinemaRoomData(id));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

