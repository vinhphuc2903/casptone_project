using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using CapstoneProject.Areas.Users.Models.UserModel;

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
        /// Lấy tất cả nhân viên theo điều kiện tìm kiếm
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpGet("user-detail")]
        public async Task<ActionResult> GetDetailUser([FromQuery] string id)
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
    }
}