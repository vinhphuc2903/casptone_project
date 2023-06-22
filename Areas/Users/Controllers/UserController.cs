using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Users.Models.UserModel;
using CapstoneProject.Auths;
using CapstoneProject.Databases.Schemas.Setting;
using System.ComponentModel.DataAnnotations;

namespace CapstoneProject.Areas.Users.Controllers
{
    [Route(("api/{api_version:apiVersion}/users"))]
    public class UsersController : UsersAreaController
    {
        private readonly IUsersModel _userModel;
        public UsersController(
             IUsersModel usersModel,
             IServiceProvider provider
        ) : base(provider)
        {
            _userModel = usersModel ?? throw new ArgumentNullException(nameof(usersModel));
        }

        /// <summary>
        /// Lấy chi tiết nhân viên theo Id
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("{id}")]
        public async Task<ActionResult> GetDetailUser([FromRoute] int id)
        {
            try
            {
                return Ok(await _userModel.GetDetailUser(id));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy thông tin user login
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet()]
        [Auth]
        public async Task<ActionResult> GetDetailUserLogin()
        {
            try
            {
                return Ok(await _userModel.GetUserIsLogin());
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy danh sách các tỉnh
        /// <para>Created at: 19/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("province")]
        public async Task<ActionResult> GetProvinces()
        {
            try
            {
                return Ok(await _userModel.GetProvinces());
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy danh sách các huyện
        /// <para>Created at: 19/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("district")]
        public async Task<ActionResult> GetDistrict([FromQuery][Required] string provinceId)
        {
            try
            {
                if (string.IsNullOrEmpty(provinceId))
                {
                    // Trường hợp không truyền provinceId
                    return BadRequest("Missing provinceId");
                }
                return Ok(await _userModel.GetDistrict(provinceId));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy danh sách các thị xã
        /// <para>Created at: 19/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("commune")]
        public async Task<ActionResult> GetCommune([FromQuery][Required] string districtId)
        {
            try
            {
                if (string.IsNullOrEmpty(districtId))
                {
                    // Trường hợp không truyền districtId
                    return BadRequest("Missing districtId");
                }
                return Ok(await _userModel.GetCommune(districtId));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy danh sách các chi nhánh
        /// <para>Created at: 19/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("branch")]
        public async Task<ActionResult> GetBranches([FromQuery]int? id)
        {
            try
            {
                return Ok(await _userModel.GetBranches(id));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Lấy danh sách các 
        /// <para>Created at: 19/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("position")]
        public async Task<ActionResult> GetPositions()
        {
            try
            {
                return Ok(await _userModel.GetPositions());
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}