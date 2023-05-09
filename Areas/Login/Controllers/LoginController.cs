// using System;
// using System.Net;
// using System.Threading.Tasks;
// using FMStyle.API.Areas.Auth.Models.Login;
// using FMStyle.API.Areas.Auth.Models.Login.Schemas;
// using Microsoft.AspNetCore.Mvc;
// using Outfiz.Commons.Enums;
// using Outfiz.Commons.Schemas;
// using Outfiz.Validate;

// namespace FMStyle.API.Areas.Auth.Controllers
// {
//     [Route("api/{api_version:apiVersion}/auth/login")]
//     public class LoginController : AuthAreaController
//     {
//         private readonly ILoginModel _loginModel;

//         public LoginController(
//             ILoginModel loginModel,
//             IServiceProvider provider
//         ) : base(provider)
//         {
//             _loginModel = loginModel ?? throw new ArgumentNullException(nameof(loginModel));
//         }

//         /// <summary>
//         /// Kiểm tra thông tin đăng nhập để lấy token
//         /// <para>Created at: 18/04/2021</para>
//         /// <para>Created by: QuyPN</para>
//         /// </summary>
//         /// <param name="user">Dữ liệu đăng nhập từ màn hình</param>
//         /// <returns>Thông tin sau khi kiểm tra đăng nhập</returns>
//         /// <remarks>
//         /// Code
//         /// 
//         ///     200 - Thành công
//         ///     201 - Lỗi dữ liệu nhập vào
//         ///     202 - Có lỗi khác phát sinh
//         ///     403 - Không có quyền truy cập
//         ///     500 - Lỗi server
//         ///
//         /// Thông tin trả về cho SignalR
//         ///
//         ///     /hub/notification
//         ///     "OnNewNotification"
//         ///     {
//         ///         "Id": 1,
//         ///         "Title": "tỉle",
//         ///         "Content": "content",
//         ///         "Type": 1,
//         ///         "KeyReffer": "{"Id": 1}"
//         ///     }
//         ///
//         ///     /hub/security-alert
//         ///     "MultiLoginError"
//         ///     {
//         ///         "LatOfMap": 16.0862274,
//         ///         "LongOfMap": "108.1500744",
//         ///         "Browser": 1000,
//         ///         "Ip": "192.168.1.1",
//         ///         "Position": "Liên Chiểu, Đà Nẵng, Việt Nam",
//         ///         "TimeLogin": "2021-01-01  23:01:01+07",
//         ///         "LockToken": "139d32f9-c878-444c-8fc9-7e30eabd6867"
//         ///     }
//         ///     
//         /// </remarks>
//         /// <response code="200">
//         /// Thành công
//         /// 
//         ///     {
//         ///         "Code": 200,
//         ///         "MsgNo": "",
//         ///         "ListError": null,
//         ///         "Data": {
//         ///             "Token": "Token",
//         ///             "RefreshToken": "RefreshToken",
//         ///             "Expires": "20",
//         ///             "IsActive": "1",
//         ///             "IsAddSecurityQuestions": "1",
//         ///             "LoginedInOtherDevice": "0"
//         ///         }
//         ///     }
//         ///     
//         /// Lỗi validate
//         /// 
//         ///     {
//         ///         "Code": 201
//         ///         "MsgNo": "",
//         ///         "ListError": {
//         ///             "Username": "E001",
//         ///             "Passwoed": "E005",
//         ///         },
//         ///         "Data": null
//         ///     }
//         ///     
//         /// Lỗi khác
//         /// 
//         ///     {
//         ///         "Code": 202
//         ///         "MsgNo": "E014",
//         ///         "ListError": null,
//         ///         "Data": null
//         ///     }
//         ///     
//         /// Exception
//         /// 
//         ///     {
//         ///         "Code": 500,
//         ///         "MsgNo": "E500",
//         ///         "ListError": null,
//         ///         "Data": {
//         ///             "Error": "Message"
//         ///     }
//         ///     
//         /// </response>
//         /// <response code="500">Lỗi khi có exception</response>
//         [HttpPost]
//         [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
//         public async Task<IActionResult> CheckAccount([FromBody] User user, [FromHeader(Name = "x-fromWeb")] bool fromWeb)
//         {
//             ResponseInfo response = new ResponseInfo();
//             try
//             {
//                 if (ModelState.IsValid)
//                 {
//                     response = await _loginModel.CheckAccount(user, fromWeb);
//                 }
//                 else
//                 {
//                     response.Code = CodeResponse.NOT_VALIDATE;
//                     response.ListError = ModelState.GetModelErrors();
//                 }
//             }
//             catch (Exception e)
//             {
//                 await _logService.SaveLogException(e);

//                 response.Code = CodeResponse.SERVER_ERROR;
//                 response.MsgNo = MSG_NO.SERVER_ERROR;
//                 response.Data.Add("Error", e.Message);
//             }
//             return Ok(response);
//         }

//         /// <summary>
//         /// Lấy lại token mới trước khi token cũ hết hạn
//         /// <para>Created at: 18/04/2021</para>
//         /// <para>Created by: QuyPN</para>
//         /// </summary>
//         /// <param name="token">Thông tin token cũ</param>
//         /// <returns>Dữ liệu token mới</returns>
//         /// <remarks>
//         /// Code
//         /// 
//         ///     200 - Thành công
//         ///     201 - Lỗi dữ liệu nhập vào
//         ///     202 - Có lỗi khác phát sinh
//         ///     403 - Không có quyền truy cập
//         ///     500 - Lỗi server
//         ///             
//         /// </remarks>
//         /// <response code="200">
//         /// Thành công
//         /// 
//         ///     {
//         ///         "Code": 200,
//         ///         "MsgNo": "",
//         ///         "ListError": null,
//         ///         "Data": {
//         ///             "Token": "Token",
//         ///             "RefreshToken": "RefreshToken",
//         ///             "Expires": "20",
//         ///             "IsActive": "1",
//         ///             "IsAddSecurityQuestions": "1"
//         ///         }
//         ///     }
//         ///     
//         /// Exception
//         /// 
//         ///     {
//         ///         "Code": 500,
//         ///         "MsgNo": "E500",
//         ///         "ListError": null,
//         ///         "Data": {
//         ///             "Error": "Message"
//         ///     }
//         ///     
//         /// </response>
//         /// <response code="401">Chưa đăng nhập</response>
//         /// <response code="500">Lỗi khi có exception</response>
//         [Filters.Auth]
//         [HttpPost("refresh-token")]
//         [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
//         public async Task<IActionResult> RefreshToken([FromBody] Token token)
//         {
//             ResponseInfo response = new ResponseInfo();
//             try
//             {
//                 response = await _loginModel.RefreshToken(token);
//                 if (response == null)
//                 {
//                     return Forbid();
//                 }
//             }
//             catch (Exception e)
//             {
//                 await _logService.SaveLogException(e);

//                 response.Code = CodeResponse.SERVER_ERROR;
//                 response.MsgNo = MSG_NO.SERVER_ERROR;
//                 response.Data.Add("Error", e.Message);
//             }
//             return Ok(response);
//         }

//         /// <summary>
//         /// Xoá thông tin token khi logout
//         /// <para>Created at: 18/04/2021</para>
//         /// <para>Created by: QuyPN</para>
//         /// </summary>
//         /// <param name="token">Thông tin token hiện tại</param>
//         /// <returns>Kết quả xoá token</returns>
//         /// <remarks>
//         /// Code
//         /// 
//         ///     200 - Thành công
//         ///     201 - Lỗi dữ liệu nhập vào
//         ///     202 - Có lỗi khác phát sinh
//         ///     403 - Không có quyền truy cập
//         ///     500 - Lỗi server
//         ///             
//         /// </remarks>
//         /// <response code="200">
//         /// Thành công
//         /// 
//         ///     {
//         ///         "Code": 200,
//         ///         "MsgNo": "",
//         ///         "ListError": null,
//         ///         "Data": null
//         ///     }
//         ///     
//         /// Exception
//         /// 
//         ///     {
//         ///         "Code": 500,
//         ///         "MsgNo": "E500",
//         ///         "ListError": null,
//         ///         "Data": {
//         ///             "Error": "Message"
//         ///     }
//         ///     
//         /// </response>
//         /// <response code="401">Chưa đăng nhập</response>
//         /// <response code="500">Lỗi khi có exception</response>
//         [Filters.Auth]
//         [HttpGet("logout")]
//         [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
//         public async Task<IActionResult> Logout()
//         {
//             ResponseInfo response = new ResponseInfo();
//             try
//             {
//                 response = await _loginModel.RemoveToken();
//             }
//             catch (Exception e)
//             {
//                 await _logService.SaveLogException(e);

//                 response.Code = CodeResponse.SERVER_ERROR;
//                 response.MsgNo = MSG_NO.SERVER_ERROR;
//                 response.Data.Add("Error", e.Message);
//             }
//             return Ok(response);
//         }

//         /// <summary>
//         /// Khoá tạm thời tài khoản
//         /// <para>Created at: 18/05/2021</para>
//         /// <para>Created by: QuyPN</para>
//         /// </summary>
//         /// <param name="token">Token khoá tài khoản</param>
//         /// <returns>Kết quả khoá</returns>
//         /// <remarks>
//         /// Code
//         /// 
//         ///     200 - Thành công
//         ///     201 - Lỗi dữ liệu nhập vào
//         ///     202 - Có lỗi khác phát sinh
//         ///     403 - Không có quyền truy cập
//         ///     500 - Lỗi server
//         ///
//         /// Thông tin trả về cho SignalR
//         ///
//         ///     /hub/security-alert
//         ///     "AccountLocked"
//         ///     {
//         ///         "Status": 10
//         ///     }
//         ///             
//         /// </remarks>
//         /// <response code="200">
//         /// Thành công
//         /// 
//         ///     {
//         ///         "Code": 200,
//         ///         "MsgNo": "",
//         ///         "ListError": null,
//         ///         "Data": null
//         ///     }
//         ///     
//         /// Exception
//         /// 
//         ///     {
//         ///         "Code": 500,
//         ///         "MsgNo": "E500",
//         ///         "ListError": null,
//         ///         "Data": {
//         ///             "Error": "Message"
//         ///         }
//         ///     }
//         ///     
//         /// </response>
//         /// <response code="403">Token không chính xác</response>
//         /// <response code="500">Lỗi khi có exception</response>
//         [HttpGet("lock-account")]
//         [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
//         public async Task<IActionResult> LockAccount([FromQuery] string Token)
//         {
//             ResponseInfo response = new ResponseInfo();
//             try
//             {
//                 response = await _loginModel.TempLockAccount(Token);
//                 if (response == null)
//                 {
//                     return Forbid();
//                 }
//             }
//             catch (Exception e)
//             {
//                 await _logService.SaveLogException(e);

//                 response.Code = CodeResponse.SERVER_ERROR;
//                 response.MsgNo = MSG_NO.SERVER_ERROR;
//                 response.Data.Add("Error", e.Message);
//             }
//             return Ok(response);
//         }


//         [HttpPost("social")]
//         [ProducesResponseType(typeof(ResponseInfo), (int)HttpStatusCode.OK)]
//         public async Task<IActionResult> Social([FromBody] UserSocial user, [FromHeader(Name = "x-fromWeb")] bool fromWeb)
//         {
//             ResponseInfo response = new ResponseInfo();
//             try
//             {
//                 if (ModelState.IsValid)
//                 {
//                     if (user.Type == "google")
//                     {
//                         response = await _loginModel.AuthGoogle(user, fromWeb);
//                     }
//                     else if (user.Type == "facebook")
//                     {
//                         response = await _loginModel.AuthFacebook(user, fromWeb);

//                     }
//                     else if (user.Type == "appleid")
//                     {
//                         response = await _loginModel.AuthAppleId(user, fromWeb);
//                     }
//                 }
//                 else
//                 {
//                     response.Code = CodeResponse.NOT_VALIDATE;
//                     response.ListError = ModelState.GetModelErrors();
//                 }


//             }
//             catch (Exception e)
//             {
//                 await _logService.SaveLogException(e);

//                 response.Code = CodeResponse.SERVER_ERROR;
//                 response.MsgNo = MSG_NO.SERVER_ERROR;
//                 response.Data.Add("Error", e.Message);
//             }
//             return Ok(response);
//         }
//     }
// }
