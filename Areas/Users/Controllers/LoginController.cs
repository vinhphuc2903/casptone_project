using CapstoneProject.Areas.Users.Models.LoginModel;
using CapstoneProject.Areas.Users.Models.LoginModel.Schemas;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace CapstoneProject.Areas.Users.Controllers
{
    [Route(("api/{api_version:apiVersion}/login"))]
    public class LoginController : UsersAreaController
    {
        private readonly ILoginModel _loginModel;
        public LoginController(
             ILoginModel loginModel,
             IServiceProvider provider
        ) : base(provider)
        {
            _loginModel = loginModel ?? throw new ArgumentNullException(nameof(loginModel));
        }
        /// <summary>
        /// Login
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost()]
        public async Task<ActionResult> CheckAccount([FromBody] LoginInfo user)
        {
            try
            {
                return Ok(await _loginModel.CheckAccount(user));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Tạo tài khoản
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPost("create-user")]
        public async Task<ActionResult> CreateUser([FromBody] AccountInfo accountInfo)
        {
            try
            {
                return Ok(await _loginModel.CreateUser(accountInfo));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Tạo tài khoản
        /// <para>Created at: 04/05/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <response code="401">Chưa đăng nhập</response>
        /// <response code="500">Lỗi khi có exception</response>
        [HttpPut("update-user")]
        public async Task<ActionResult> UpdateUser([FromBody] AccountInfo accountInfo)
        {
            try
            {
                return Ok(await _loginModel.UpdateAccount(accountInfo));
            }
            catch (Exception e)
            {
                //await _logService.SaveLogException(e);
                return StatusCode(500);
            }
        }
    }
}

