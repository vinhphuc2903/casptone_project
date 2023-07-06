using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Film.Models;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Areas.Film.Models.FilmAdminModels.Schemas;
using CapstoneProject.Areas.Film.Models.FilmAdminModels;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Film.Controllers
{
    [Auth]
    [Route(("api/{api_version:apiVersion}/films"))]
    public class FilmAdminController : FilmAreaController
    {
        private readonly IFilmAdminModels _filmAdminModel;

        public FilmAdminController(
            IFilmAdminModels filmAdminModels,
            IServiceProvider provider
        ) : base(provider)
        {
            _filmAdminModel = filmAdminModels ?? throw new ArgumentNullException(nameof(filmAdminModels));
        }
        /// <summary>
        /// Lấy chi tiết film
        /// <para>Created at: 14/06/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("admin-detail-film")]
        public async Task<ActionResult> GetDetailFilm([FromQuery] int id)
        {
            try
            {
                return Ok(await _filmAdminModel.GetDetailFilm(id));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Thêm film mới
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost()]
        public async Task<ActionResult> AddNewFilmData([FromForm] NewFilmData newFilmData)
        {
            try
            {
                return Ok(await _filmAdminModel.AddNewFilmData(newFilmData));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Cập nhật phim cũ
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPut("update-film")]
        public async Task<ActionResult> UpdateFilmData([FromForm] NewFilmData newFilmData)
        {
            try
            {
                return Ok(await _filmAdminModel.UpdateFilmData(newFilmData));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Xóa phim
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpDelete("delete-film")]
        public async Task<ActionResult> DeleteFilmData([FromQuery] int FilmId)
        {
            try
            {
                return Ok(await _filmAdminModel.DeleteFilm(FilmId));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

