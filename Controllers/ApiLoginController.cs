using System;
using System.Threading.Tasks;
//using FMStyle.ApiCommons.Models.ApiLogin;
//using FMStyle.ApiCommons.Models.ApiLogin.Schemas;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
//using Outfiz.Commons.Enums;
//using Outfiz.Commons.Schemas;
//using Outfiz.Validate;

namespace FMStyle.RPT.Controllers
{
    [Route("login")]
    public class ApiLoginController : Controller
    {
        //private readonly ILoginModel _loginModel;
        private readonly IConfiguration _configuration;

        public ApiLoginController(
            //ILoginModel loginModel,
            IConfiguration configuration)
        {
            //_loginModel = loginModel ?? throw new ArgumentNullException(nameof(loginModel));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        //[HttpGet]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<IActionResult> Index()
        //{
        //    if (_configuration["Env"] == "Production")
        //    {
        //        return NoContent();
        //    }
        //    //Developer developer = await _loginModel.IsDeveloper();
        //    if (developer.IsDeveloper)
        //    {
        //        return RedirectToAction("Index", "Home");
        //    }
        //    else
        //    {
        //        return View(await _loginModel.GetLoginSetting());
        //    }
        //}

        //[HttpPost("check-login")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<JsonResult> CheckLogin(User user)
        //{
        //    ResponseInfo response = new ResponseInfo();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            response = await _loginModel.CheckAccount(user);
        //        }
        //        else
        //        {
        //            response.Code = CodeResponse.NOT_VALIDATE;
        //            response.ListError = ModelState.GetModelErrors();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        response.Code = CodeResponse.SERVER_ERROR;
        //        response.MsgNo = MSG_NO.SERVER_ERROR;
        //        response.Data.Add("Error", e.Message);
        //    }
        //    return Json(response);
        //}

        //[HttpPost("check-otp")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<JsonResult> ChecOtp(DataOtp data)
        //{
        //    ResponseInfo response = new ResponseInfo();
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            response = await _loginModel.CheckOtp(data);
        //        }
        //        else
        //        {
        //            response.Code = CodeResponse.NOT_VALIDATE;
        //            response.ListError = ModelState.GetModelErrors();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        response.Code = CodeResponse.SERVER_ERROR;
        //        response.MsgNo = MSG_NO.SERVER_ERROR;
        //        response.Data.Add("Error", e.Message);
        //    }
        //    return Json(response);
        //}

        //[HttpPost("check-developer")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<JsonResult> CheckDeveloper()
        //{
        //    ResponseInfo response = new ResponseInfo();
        //    try
        //    {
        //        Developer developer = await _loginModel.IsDeveloper();
        //        if (!developer.IsDeveloper)
        //        {
        //            response.Code = CodeResponse.NOT_ACCESS;
        //        }
        //        else
        //        {
        //            response.Data.Add("IsVTT", developer.IsVTT ? "1" : "0");
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        response.Code = CodeResponse.SERVER_ERROR;
        //        response.MsgNo = MSG_NO.SERVER_ERROR;
        //        response.Data.Add("Error", e.Message);
        //    }
        //    return Json(response);
        //}

        //[HttpPost("lock")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<JsonResult> TempLockAccount(string token)
        //{
        //    ResponseInfo response = new ResponseInfo();
        //    try
        //    {
        //        response = await _loginModel.TempLockAccount(token);
        //    }
        //    catch (Exception e)
        //    {
        //        response.Code = CodeResponse.SERVER_ERROR;
        //        response.MsgNo = MSG_NO.SERVER_ERROR;
        //        response.Data.Add("Error", e.Message);
        //    }
        //    return Json(response);
        //}

        //[HttpGet("logout")]
        //[ApiExplorerSettings(IgnoreApi = true)]
        //public async Task<IActionResult> Logout()
        //{
        //    await _loginModel.RemoveToken();
        //    Response.Cookies.Delete("TokenERP");
        //    return RedirectToAction("Index");
        //}
    }
}
