﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Film.Models.FilmModels.Schemas;
using CapstoneProject.Areas.Film.Models.FilmModels;
using CapstoneProject.Areas.Film.Models;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Databases.Schemas.System.Film;

namespace CapstoneProject.Areas.Film.Controllers
{
    [Route(("api/{api_version:apiVersion}/films"))]
    public class FilmController : FilmAreaController
    {
        private readonly IFilmModel _filmModel;

        public FilmController(
            IFilmModel filmModel,
            IServiceProvider provider
        ) : base(provider)
        {
            _filmModel = filmModel ?? throw new ArgumentNullException(nameof(filmModel));
        }
        /// <summary>
        /// Lấy tất cả bộ film theo điều kiện tìm kiếm
        /// <para>Created at: 14/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet()]
        public async Task<ActionResult> GetListFilm([FromQuery]SearchCondition searchCondition)
        {
            try
            {
                return Ok(await _filmModel.GetListFilms(searchCondition));
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
        //[HttpPost()]
        //public async Task<ActionResult> AddNewFilmData([FromBody] NewFilmData newFilmData)
        //{
        //    try
        //    {
        //        return Ok(await _filmModel.AddNewFilmData(newFilmData));
        //    }
        //    catch (Exception e)
        //    {
        //        //await _logService.SaveLogException(e);
        //        return StatusCode(500);
        //    }
        //}
    }
}
