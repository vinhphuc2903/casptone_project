//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using FMStyle.API.Areas.Auth.Models.Login.Schemas;
//using Microsoft.AspNetCore.SignalR;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Storage;
//using Microsoft.IdentityModel.Tokens;
//using System.Data.Common;
//using Dapper;
//using Microsoft.Extensions.Logging;
//using System.Runtime.CompilerServices;
//using Microsoft.Extensions.Configuration;
//using Newtonsoft.Json.Linq;
//using System.Security.Cryptography;
//using CapstoneProject.Commons.Schemas;
//using CapstoneProject.Models;

//namespace CapstoneProject.Areas.Login.Models
//{
//    public interface ILoginModel
//    {
//        Task<ResponseInfo> CheckAccount(User user, bool fromWeb = false);
//        Task<ResponseInfo> RefreshToken(Token token);
//        Task<ResponseInfo> RemoveToken();
//        Task<ResponseInfo> TempLockAccount(string token);
//        Task<ResponseInfo> AuthGoogle(UserSocial user, bool fromWeb = false);
//        Task<ResponseInfo> AuthFacebook(UserSocial user, bool fromWeb = false);
//        Task<ResponseInfo> AuthAppleId(UserSocial user, bool fromWeb = false);

//    }
//    public class LoginModel : CapstoneProjectModels, ILoginModel
//    {
//        //private readonly IHubContext<SecurityAlertHub> _hubContext;
//        //private readonly ReCaptchaService _reCaptchaService;
//        private readonly ILogger<LoginModel> _logger;
//        private string _className = "";

//        private readonly IConfiguration _configuration;

//        public LoginModel(
//            //IHubContext<SecurityAlertHub> hubContext,
//            IConfiguration configuration,
//            ILogger<LoginModel> logger,
//            //ReCaptchaService reCaptchaService,
//            IServiceProvider provider
//        ) : base(provider)
//        {
//            //_hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
//            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//            _className = GetType().Name;
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//            //_reCaptchaService = reCaptchaService ?? throw new ArgumentNullException(nameof(reCaptchaService));
//        }

//        static string GetActualAsyncMethodName([CallerMemberName] string name = null) => name;

//        public async Task<ResponseInfo> CheckAccount(User user, bool fromWeb = false)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                //_logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                ResponseInfo result = new ResponseInfo();
//                // bool newLogin = false;
//                string phone = Security.Base64Encode(user.Username);
//                TblUser userDB = _context.Users
//                    .Include(x => x.Questions)
//                    .Include(x => x.Information)
//                    .Include(x => x.Point)
//                    .FirstOrDefault(x => (x.Username == user.Username || x.Email == user.Username || x.Phone == phone) && !x.DelFlag && x.IsActive && x.Password != null && x.Password != "");

//                //_logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Kiểm tra thông tin người dùng nhập");

//                if (userDB == null)
//                {
//                    result.MsgNo = MSG_NO.USERNAME_OR_PASSWORD_NOT_INCORRECT;
//                    result.Code = CodeResponse.HAVE_ERROR;
//                    result.Data.Add("LoginFailed", "0");
//                    return result;
//                }
//                if (userDB.Status == A001.TEMP_LOCK.CODE || userDB.Status == A001.LOCK.CODE)
//                {
//                    result.MsgNo = MSG_NO.ACCOUNT_WAS_LOCKED;
//                    result.Code = CodeResponse.HAVE_ERROR;
//                    return result;
//                }
//                if (userDB.Status == A001.BLACKLIST.CODE)
//                {
//                    result.MsgNo = MSG_NO.ACCOUNT_IN_BLACKLIST;
//                    result.Code = CodeResponse.HAVE_ERROR;
//                    return result;
//                }
//                var setting = await GetSetting();
//                if (userDB.LoginFailed >= setting.NumberLoginFailedInDay && !await checkCaptcha(user.CaptchaToken))
//                {
//                    // Nếu chưa có captcha thì báo lỗi
//                    if (String.IsNullOrEmpty(user.CaptchaToken))
//                    {
//                        result.MsgNo = MSG_NO.REQUIRED_CAPTCHA;
//                        result.Code = CodeResponse.HAVE_ERROR;
//                        return result;
//                    }
//                    // Nếu captcha không hợp lệ thì báo lỗi
//                    if (!await checkCaptcha(user.CaptchaToken))
//                    {
//                        result.MsgNo = MSG_NO.CAPTCHA_INVALID;
//                        result.Code = CodeResponse.HAVE_ERROR;
//                        return result;
//                    }
//                }

//                if (string.IsNullOrEmpty(userDB.Password) || userDB.Password != Security.GetMD5(user.Password, userDB.FirstSecurityString, userDB.LastSecurityString))
//                {
//                    result.MsgNo = MSG_NO.USERNAME_OR_PASSWORD_NOT_INCORRECT;
//                    result.Code = CodeResponse.HAVE_ERROR;
//                    // Udpate số lần login lỗi
//                    userDB.LoginFailed = userDB.LoginFailed + 1;
//                    result.Data.Add("LoginFailed", userDB.LoginFailed.ToString());
//                    result.Data.Add("ShowCountdown", userDB.LoginFailed > setting.NumberLoginFailedInDayToShowCountdown ? "true" : "false");
//                    transaction = await _context.Database.BeginTransactionAsync();
//                    await _context.SaveChangesAsync();
//                    await transaction?.CommitAsync();
//                }
//                else
//                {
//                    // Reset số lần login lỗi
//                    userDB.LoginFailed = 0;
//                    // Tạo token và lưu các thông tin cần thiết khi login
//                    string token = GenerationJWTCode(userDB.Id, userDB.Phone);
//                    string refreshToken = Helpers.RenderToken(userDB.Id.ToString(), 180);
//                    string oldPlayerId = "";
//                    string oldSerial = "";
//                    DateTimeOffset now = DateTimeOffset.Now;

//                    TblUserToken userToken = null;
//                    TblUserToken lockToken = null;
//                    bool addNewToken = true;
//                    int loginFrom = fromWeb ? L001.WEB.CODE : L001.APP.CODE;
//                    TblUserToken lastLogin = fromWeb ? null : _context.UserTokens.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => !x.DelFlag && x.UserId == userDB.Id && x.LoginFrom == L001.APP.CODE);
//                    TblUserToken currentToken = fromWeb ? null : _context.UserTokens.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => !x.DelFlag && x.Timeout >= now && x.UserId == userDB.Id && x.LoginFrom == L001.APP.CODE);
//                    if (currentToken != null)
//                    {
//                        oldPlayerId = currentToken.PlayerId;
//                        oldSerial = currentToken.Serial;
//                        if (currentToken.PlayerId == user.PlayerId && currentToken.Serial == user.Serial)
//                        {
//                            currentToken.JwtToken = token;
//                            currentToken.RefreshToken = refreshToken;
//                            currentToken.ValidateToken = "";
//                            currentToken.LatOfMap = user.LatOfMap;
//                            currentToken.LongOfMap = user.LongOfMap;
//                            currentToken.Browser = user.Browser;
//                            currentToken.PlayerId = user.PlayerId;
//                            currentToken.Serial = user.Serial;
//                            currentToken.Timeout = DateTimeOffset.Now.AddMinutes(Constants.API_EXPIRES_MINUTE);
//                            addNewToken = false;
//                        }
//                        else
//                        {
//                            currentToken.Timeout = DateTimeOffset.Now.AddMinutes(-1);
//                        }
//                    }

//                    if (addNewToken)
//                    {
//                        userToken = new TblUserToken()
//                        {
//                            UserId = userDB.Id,
//                            JwtToken = token,
//                            RefreshToken = refreshToken,
//                            ValidateToken = "",
//                            LatOfMap = user.LatOfMap,
//                            LongOfMap = user.LongOfMap,
//                            Browser = user.Browser,
//                            PlayerId = user.PlayerId,
//                            Serial = user.Serial,
//                            LoginFrom = loginFrom,
//                            Timeout = DateTimeOffset.Now.AddMinutes(Constants.API_EXPIRES_MINUTE),
//                        };
//                        _context.UserTokens.Add(userToken);
//                    }

//                    if (!String.IsNullOrEmpty(oldSerial) && user.Serial != oldSerial)
//                    {
//                        lockToken = new TblUserToken()
//                        {
//                            UserId = userDB.Id,
//                            JwtToken = "",
//                            RefreshToken = "",
//                            ValidateToken = Guid.NewGuid().ToString(),
//                            LatOfMap = user.LatOfMap,
//                            LongOfMap = user.LongOfMap,
//                            Browser = user.Browser,
//                            PlayerId = user.PlayerId,
//                            Serial = user.Serial,
//                            LoginFrom = L001.BATCH.CODE,
//                            Timeout = DateTimeOffset.Now.AddMinutes(2),
//                        };
//                        _context.UserTokens.Add(lockToken);
//                    }

//                    userDB.LastLogin = DateTimeOffset.Now;

//                    // DateTime loginDate = DateTime.Today;
//                    // string loginDateStr = loginDate.ToString("yyyyMMdd");
//                    // if (_context.DailyLogins.Count(x => x.LoginDateStr == loginDateStr && x.UserId == userDB.Id) == 0)
//                    // {
//                    //     newLogin = true;
//                    //     _context.DailyLogins.Add(new TblDailyLogin()
//                    //     {
//                    //         Id = Guid.NewGuid(),
//                    //         UserId = userDB.Id,
//                    //         LoginDate = loginDate,
//                    //         LoginDateStr = loginDateStr
//                    //     });
//                    // }

//                    if (String.IsNullOrEmpty(userDB.MyInviteCode))
//                    {
//                        //string InviteCode = Helpers.GenerationInviteCode();
//                        //int loop = 0;
//                        //TblUser InviteCodeUser = _context.Users.FirstOrDefault(x => x.InviteCode == InviteCode);
//                        //while (InviteCodeUser != null && loop < 5)
//                        //{
//                        //    InviteCode = Helpers.GenerationInviteCode();
//                        //    InviteCodeUser = _context.Users.FirstOrDefault(x => x.InviteCode == InviteCode);
//                        //    loop++;
//                        //}
//                        //if (InviteCodeUser != null)
//                        //{
//                        //    InviteCode = Security.Base64Decode(userDB.Phone);
//                        //}
//                        userDB.MyInviteCode = userDB.Information.CustomerId;
//                    }

//                    transaction = await _context.Database.BeginTransactionAsync();
//                    await _context.SaveChangesAsync();
//                    await transaction?.CommitAsync();

//                    result.Data.Add("Token", token);
//                    result.Data.Add("RefreshToken", refreshToken);
//                    result.Data.Add("Expires", Constants.API_EXPIRES_MINUTE.ToString());
//                    result.Data.Add("IsActive", userDB.IsActive ? "1" : "0");
//                    result.Data.Add("IsAddSecurityQuestions", userDB.Questions.Count(x => !x.DelFlag) > 0 ? "1" : "0");

//                    _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Login thành công");

//                    try
//                    {
//                        // if (newLogin)
//                        // {
//                        //     _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Kiểm tra login hằng ngày");
//                        //     DbConnection _connection = _context.GetConnection();
//                        //     string sqlGetPoint = $@"
//                        //         DELETE FROM ""DailyLogins""
//                        //         WHERE
//                        //             ""DelFlag"" = false
//                        //         AND ""UserId"" = {userDB.Id}
//                        //         AND ""LoginDate"" < NOW() - interval '7 days';
//                        //         WITH ""LoginDates"" AS (
//                        //             SELECT DISTINCT
//                        //                 ""LoginDate""
//                        //             FROM ""DailyLogins""
//                        //             WHERE
//                        //                 ""DelFlag"" = false
//                        //             AND ""UserId"" = {userDB.Id}
//                        //             ORDER BY ""LoginDate"" DESC
//                        //             LIMIT 8
//                        //         ),
//                        //         ""LoginDateGroups"" AS (
//                        //             SELECT
//                        //                 ""LoginDate""
//                        //             ,   ""LoginDate"" - CAST(ROW_NUMBER() OVER (ORDER BY ""LoginDate"") as INT) AS ""Group""
//                        //             FROM ""LoginDates""
//                        //         ),
//                        //         ""LoginDateCount"" AS (
//                        //             SELECT
//                        //                 MAX(""LoginDate"") - MIN(""LoginDate"") + 1 AS ""Count""
//                        //             FROM ""LoginDateGroups""
//                        //             GROUP BY ""Group""
//                        //             ORDER BY ""Group"" DESC
//                        //             LIMIT 1
//                        //         )
//                        //         SELECT
//                        //             ""PointForDailyLogin"".""Point""
//                        //         ,   CASE WHEN ""LoginDateCount"".""Count"" > 7 THEN 7 ELSE ""LoginDateCount"".""Count"" END AS ""Count""
//                        //         FROM ""LoginDateCount""
//                        //         LEFT JOIN ""PointForDailyLogin""
//                        //         ON (
//                        //             ""PointForDailyLogin"".""DelFlag"" = false
//                        //         AND ""PointForDailyLogin"".""NumberOfDay"" = CASE WHEN ""LoginDateCount"".""Count"" > 7 THEN 7 ELSE ""LoginDateCount"".""Count"" END
//                        //         )
//                        //         ORDER BY ""PointForDailyLogin"".""Point"" DESC
//                        //         LIMIT 1;
//                        //     ";
//                        //     CountLogin countLogin = await _connection.QueryFirstOrDefaultAsync<CountLogin>(sqlGetPoint);
//                        //     if (countLogin.Point > 0)
//                        //     {
//                        //         _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Cộng điểm login hàng ngày");
//                        //         TblPointHistory pointHistory = new TblPointHistory()
//                        //         {
//                        //             UserId = userDB.Id,
//                        //             RankPoint = 0,
//                        //             NormalPoint = countLogin.Count,
//                        //             GoldPoint = 0,
//                        //             ReasonForAddPoint = P003.DAILY_LOGIN.CODE,
//                        //             ReasonForMinusPoint = 0,
//                        //             Status = P005.FINISHED.CODE
//                        //         };
//                        //         _context.PointHistories.Add(pointHistory);
//                        //         userDB.Point.NormalPoint += countLogin.Count;
//                        //         _context.SaveChanges();
//                        //         await _pointService.NotifyAddNormalPoint(userDB, pointHistory, CodeMasterUtil.GetName(typeof(P003), P003.DAILY_LOGIN.CODE.ToString()));
//                        //         if(countLogin.Count >= 7)
//                        //         {
//                        //             await _connection.ExecuteAsync($@"DELETE FROM ""DailyLogins"" WHERE ""DelFlag"" = false AND ""UserId"" = {userDB.Id};");
//                        //         }
//                        //     }
//                        // }

//                        string position = await GetPosition(user.LatOfMap, user.LongOfMap);

//                        if (!String.IsNullOrEmpty(oldSerial) && user.Serial != oldSerial)
//                        {
//                            _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] 2 thiết bị cùng đăng nhập");

//                            await _hubContext.Clients.User(phone).SendAsync(SecurityAlertHub.MULTI_LOGIN_ERROR, new
//                            {
//                                LatOfMap = user.LatOfMap,
//                                LongOfMap = user.LongOfMap,
//                                Browser = user.Browser,
//                                Ip = userDB.UpdatedIp,
//                                Position = position,
//                                TimeLogin = now,
//                                LockToken = lockToken == null ? "" : lockToken.ValidateToken
//                            });

//                            await _notificationService.SendQueue(
//                                NOTIFICATION_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.Title,
//                                NOTIFICATION_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.GetContent(new {
//                                    Now = now.ToString("dd.MM.yyyy HH:mm:ss"),
//                                    Position = position,
//                                    IP = userDB.UpdatedIp,
//                                    Device = user.Browser
//                                }),
//                                userDB.Id,
//                                T009.COMMON.CODE,
//                                "",
//                                false,
//                                null,
//                                null,
//                                null,
//                                oldPlayerId,
//                                null,
//                                P007.HIGHT.CODE
//                            );

//                            await _mailService.SendQueue(
//                                userDB.Email,
//                                EMAIL_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.Title,
//                                EMAIL_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.Template,
//                                Newtonsoft.Json.JsonConvert.SerializeObject(new
//                                {
//                                    FirstName = userDB.Information.FirstName,
//                                    LastName = userDB.Information.LastName,
//                                    Browser = user.Browser,
//                                    Ip = userDB.UpdatedIp,
//                                    Position = position,
//                                    TimeLogin = now
//                                }),
//                                "",
//                                P007.HIGHT.CODE
//                            );
//                            result.Data.Add("LoginedInOtherDevice", "1");
//                        }
//                        else
//                        {
//                            result.Data.Add("LoginedInOtherDevice", "0");
//                            if (lastLogin != null && !String.IsNullOrEmpty(lastLogin.Serial) && lastLogin.Serial != user.Serial)
//                            {
//                                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Đăng nhập từ thiết bị lạ");
//                                await _mailService.SendQueue(
//                                    userDB.Email,
//                                    EMAIL_TEMPLATE.FM_PLUS_ACCOUNT_LOG_IN_INFORMATION.Title,
//                                    EMAIL_TEMPLATE.FM_PLUS_ACCOUNT_LOG_IN_INFORMATION.Template,
//                                    Newtonsoft.Json.JsonConvert.SerializeObject(new
//                                    {
//                                        FirstName = userDB.Information.FirstName,
//                                        LastName = userDB.Information.LastName,
//                                        Browser = user.Browser,
//                                        Ip = userDB.UpdatedIp,
//                                        Position = position,
//                                        TimeLogin = now
//                                    }),
//                                    "",
//                                    P007.HIGHT.CODE
//                                );
//                            }
//                        }
//                    }
//                    catch { }
//                }

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");

//                throw e;
//            }
//        }

//        public async Task<ResponseInfo> RefreshToken(Token token)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                ResponseInfo result = new ResponseInfo();
//                DateTimeOffset now = DateTimeOffset.Now;
//                TblUserToken userTokenDB = _context.UserTokens.Include(x => x.User).ThenInclude(x => x.Questions)
//                    .FirstOrDefault(x => !x.DelFlag && x.JwtToken == token.JwtToken && x.RefreshToken == token.RefreshToken && x.Timeout >= now);

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Kiểm tra thông tin token cũ");

//                if (userTokenDB != null)
//                {
//                    string newToken = GenerationJWTCode(userTokenDB.UserId, userTokenDB.User.Phone);
//                    string refreshToken = Helpers.RenderToken(userTokenDB.UserId.ToString(), 180);
//                    userTokenDB.JwtToken = newToken;
//                    userTokenDB.RefreshToken = refreshToken;
//                    userTokenDB.Timeout = DateTimeOffset.Now.AddMinutes(Constants.API_EXPIRES_MINUTE);
//                    userTokenDB.User.LastLogin = now;
//                    result.Data.Add("Token", newToken);
//                    result.Data.Add("RefreshToken", refreshToken);
//                    result.Data.Add("Expires", Constants.API_EXPIRES_MINUTE.ToString());
//                    result.Data.Add("IsActive", userTokenDB.User.IsActive ? "1" : "0");
//                    result.Data.Add("IsAddSecurityQuestions", userTokenDB.User.Questions.Count(x => !x.DelFlag) > 0 ? "1" : "0");
//                    transaction = await _context.Database.BeginTransactionAsync();
//                    await _context.SaveChangesAsync();
//                    await transaction?.CommitAsync();
//                }
//                else
//                {
//                    return null;
//                }

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");
//                throw e;
//            }
//        }

//        public async Task<ResponseInfo> RemoveToken()
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
//                ResponseInfo result = new ResponseInfo();
//                string token = _identityService.GetToken();
//                DateTimeOffset now = DateTimeOffset.Now;
//                IEnumerable<TblUserToken> userTokenDBs = _context.UserTokens.Include(x => x.User)
//                    .Where(x => !x.DelFlag && x.JwtToken == token);

//                foreach (TblUserToken userTokenDB in userTokenDBs)
//                {
//                    userTokenDB.JwtToken = "";
//                    userTokenDB.RefreshToken = "";
//                    userTokenDB.PlayerId = "";
//                    userTokenDB.User.LastLogout = now;
//                }

//                transaction = await _context.Database.BeginTransactionAsync();
//                await _context.SaveChangesAsync();
//                await transaction?.CommitAsync();

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");

//                throw e;
//            }
//        }

//        public async Task<ResponseInfo> TempLockAccount(string token)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
//                ResponseInfo result = new ResponseInfo();
//                DateTimeOffset now = DateTimeOffset.Now;
//                TblUserToken userTokenDB = _context.UserTokens
//                .Include(x => x.User).ThenInclude(x => x.Notifications)
//                .Include(x => x.User).ThenInclude(x => x.Information)
//                    .FirstOrDefault(x => !x.DelFlag && x.ValidateToken == token && x.Timeout >= now && x.LoginFrom == L001.BATCH.CODE);

//                if (userTokenDB != null)
//                {
//                    userTokenDB.Timeout = now.AddMinutes(-1);
//                    userTokenDB.User.Status = A001.TEMP_LOCK.CODE;
//                    userTokenDB.User.ChangeStatusAt = DateTimeOffset.Now;
//                    transaction = await _context.Database.BeginTransactionAsync();
//                    await _context.SaveChangesAsync();
//                    await transaction?.CommitAsync();
//                    await _hubContext.Clients.User(userTokenDB.User.Phone).SendAsync(SecurityAlertHub.ACCOUNT_LOCKED, new
//                    {
//                        Status = A001.TEMP_LOCK.CODE
//                    });
//                    await _mailService.SendQueue(
//                        userTokenDB.User.Email,
//                        EMAIL_TEMPLATE.TEP_LOCK_FM_PLUS_ACCOUNT.Title,
//                        EMAIL_TEMPLATE.TEP_LOCK_FM_PLUS_ACCOUNT.Template,
//                        Newtonsoft.Json.JsonConvert.SerializeObject(new
//                        {
//                            FirstName = userTokenDB.User.Information.FirstName,
//                            LastName = userTokenDB.User.Information.LastName
//                        })
//                    );
//                }
//                else
//                {
//                    return null;
//                }

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");

//                throw e;
//            }
//        }

//        private string GenerationJWTCode(int userId, string phone)
//        {
//            string method = GetActualAsyncMethodName();

//            _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//            string C_JWT = "Audience";
//            string C_JWT_SECRET_KEY = "Secret";
//            string C_JWT_ISSUER = "Iss";
//            string C_JWT_AUDIENCE = "Aud";
//            var now = DateTime.Now;
//            var audienceConfig = StartupState.Instance.Configuration.GetSection(C_JWT);
//            // Khởi tạo Claim
//            var claims = new Claim[] {
//                new Claim ("UserId", userId.ToString()),
//                new Claim ("Phone", phone),
//                new Claim(JwtRegisteredClaimNames.Sub, phone),
//                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
//                new Claim (JwtRegisteredClaimNames.Iat, now.ToUniversalTime ().ToString (), ClaimValueTypes.Integer64)
//            };
//            // Khởi tạo SymmetricSecurityKey
//            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig[C_JWT_SECRET_KEY] != null ? audienceConfig[C_JWT_SECRET_KEY] : Constants.JWT_SECRET_KEY));

//            // Khởi tạo JwtSecurityToken
//            var jwt = new JwtSecurityToken(
//                issuer: audienceConfig[C_JWT_ISSUER] != null ? audienceConfig[C_JWT_ISSUER] : Constants.JWT_ISSUER,
//                audience: audienceConfig[C_JWT_AUDIENCE] != null ? audienceConfig[C_JWT_AUDIENCE] : Constants.JWT_AUD,
//                claims: claims,
//                notBefore: now,
//                expires: now.Add(TimeSpan.FromMinutes(Constants.API_EXPIRES_MINUTE)),
//                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
//            );

//            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

//            _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//            return encodedJwt;
//        }

//        private async Task<bool> checkCaptcha(string token)
//        {
//            return await _reCaptchaService.IsValid(token);
//        }

//        private async Task<string> GetPosition(string latitude, string longtitude)
//        {
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                UriBuilder builder = new UriBuilder($"https://api.bigdatacloud.net/data/reverse-geocode-client?latitude={latitude}&longitude={longtitude}&localityLanguage=vi");
//                HttpClient client = new HttpClient();
//                HttpResponseMessage response = await client.GetAsync(builder.ToString());
//                string res = "";
//                string position = "";
//                if (response.IsSuccessStatusCode)
//                {
//                    res = await response.Content.ReadAsStringAsync();
//                }
//                if (!String.IsNullOrEmpty(res) && ((res.StartsWith("{") && res.EndsWith("}")) || (res.StartsWith("[") && res.EndsWith("]"))))
//                {
//                    res = res.Substring(0, res.IndexOf(@$"""localityInfo""")) + "}";
//                    Dictionary<string, string> dic = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(res);
//                    string locality = "";
//                    string principalSubdivision = "";
//                    string countryName = "";
//                    if (dic.TryGetValue("locality", out locality))
//                    {
//                        position += !String.IsNullOrEmpty(locality) ? locality : "";
//                    }
//                    if (dic.TryGetValue("principalSubdivision", out principalSubdivision))
//                    {
//                        position += !String.IsNullOrEmpty(principalSubdivision) ? (", " + principalSubdivision) : "";
//                    }
//                    if (dic.TryGetValue("countryName", out countryName))
//                    {
//                        position += !String.IsNullOrEmpty(countryName) ? (", " + countryName) : "";
//                    }
//                    if (position.StartsWith(","))
//                    {
//                        position = position.Substring(1, position.Length - 1);
//                    }
//                    if (position.EndsWith(","))
//                    {
//                        position = position.Substring(0, position.Length - 1);
//                    }
//                    if (String.IsNullOrEmpty(position))
//                    {
//                        position = latitude + ", " + longtitude; ;
//                    }

//                    return position;
//                }

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return latitude + ", " + longtitude;
//            }
//            catch (Exception e)
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");

//                return latitude + ", " + longtitude;
//            }
//        }

//        private async Task<TblSetting> GetSetting()
//        {
//            string method = GetActualAsyncMethodName();

//            _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//            DbConnection _connection = _context.GetConnection();
//            string sql = $@"
//                            SELECT
//                                *
//                            FROM ""Settings""
//                        ";
//            var setting = await _connection.QueryFirstOrDefaultAsync<TblSetting>(sql);

//            _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//            return setting;
//        }

//        public async Task<ResponseInfo> AuthGoogle(UserSocial user, bool fromWeb = false)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                ResponseInfo result = new ResponseInfo();

//                IConfigurationSection Google = _configuration.GetSection("Google");
//                var setting = await GetSetting();
//                string Endpoint = String.IsNullOrEmpty(setting.GoogleEndpoint) ? setting.GoogleEndpoint : Google["Endpoint"];
//                UriBuilder builder = new UriBuilder($"{Endpoint}/tokeninfo?id_token={user.Token}");
//                HttpClient client = new HttpClient();
//                HttpResponseMessage response = await client.GetAsync(builder.ToString());

//                if (response.IsSuccessStatusCode)
//                {
//                    var jsonData = response.Content.ReadAsStringAsync().Result;
//                    _logger.LogInformation(jsonData);
//                    var jsonObject = JObject.Parse(jsonData);
//                    var UserInput = new UserInput
//                    {
//                        Email = jsonObject.ContainsKey("email") ? (string)jsonObject.GetValue("email") : "",
//                        Phone = jsonObject.ContainsKey("phone") ? (string)jsonObject.GetValue("phone") : "",
//                        Firstname = jsonObject.ContainsKey("given_name") ? (string)jsonObject.GetValue("given_name") : "",
//                        Lastname = jsonObject.ContainsKey("family_name") ? (string)jsonObject.GetValue("family_name") : "",
//                        GoogleId = jsonObject.ContainsKey("email") ? (string)jsonObject.GetValue("email") : "",
//                        Gender = jsonObject.ContainsKey("gender") ? ((string)jsonObject.GetValue("gender") == "male" ? G001.MALE.CODE : ((string)jsonObject.GetValue("gender") == "female" ? G001.FEMALE.CODE : "")) : "",
//                        Avatar = jsonObject.ContainsKey("picture") ? (string)jsonObject.GetValue("picture") : ""
//                    };
//                    if (!String.IsNullOrEmpty((string)jsonObject.GetValue("birthday")))
//                    {
//                        UserInput.Birthday = (DateTime)jsonObject.GetValue("birthday");
//                    }
//                    TblUser userDB = null;
//                    if (userDB == null && !String.IsNullOrEmpty(((string)jsonObject.GetValue("phone"))))
//                    {
//                        userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Phone == Security.Base64Encode((string)jsonObject.GetValue("phone")) && !x.DelFlag);
//                    }
//                    if (userDB == null && !String.IsNullOrEmpty(((string)jsonObject.GetValue("email"))))
//                    {
//                        userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Email == (string)jsonObject.GetValue("email") && !x.DelFlag);
//                    }
//                    if (userDB == null && !String.IsNullOrEmpty(((string)jsonObject.GetValue("email"))))
//                    {
//                        userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.GoogleId == (string)jsonObject.GetValue("email") && !x.DelFlag);
//                    }
//                    // Handle for login
//                    if (userDB != null)
//                    {
//                        userDB.GoogleId = UserInput.GoogleId;
//                        _context.SaveChanges();
//                        return await HandleLoginSocial(user, userDB, fromWeb);
//                    }
//                    else
//                    {
//                        // Handle for signup
//                        return await HandleSignUpBySocial(user, UserInput, fromWeb);
//                    }
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");
//                throw e;
//            }
//        }
//        private async Task<ResponseInfo> HandleLoginSocial(UserSocial user, TblUser userDB, bool fromWeb = false)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                ResponseInfo result = new ResponseInfo();

//                // Reset số lần login lỗi
//                userDB.LoginFailed = 0;
//                // Tạo token và lưu các thông tin cần thiết khi login
//                string token = GenerationJWTCode(userDB.Id, userDB.Phone);
//                string refreshToken = Helpers.RenderToken(userDB.Id.ToString(), 180);
//                string oldPlayerId = "";
//                string oldSerial = "";
//                DateTimeOffset now = DateTimeOffset.Now;

//                TblUserToken userToken = null;
//                TblUserToken lockToken = null;
//                bool addNewToken = true;
//                int loginFrom = fromWeb ? L001.WEB.CODE : L001.APP.CODE;
//                TblUserToken lastLogin = fromWeb ? null : _context.UserTokens.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => !x.DelFlag && x.UserId == userDB.Id && x.LoginFrom == L001.APP.CODE);
//                TblUserToken currentToken = fromWeb ? null : _context.UserTokens.OrderByDescending(x => x.CreatedAt).FirstOrDefault(x => !x.DelFlag && x.Timeout >= now && x.UserId == userDB.Id && x.LoginFrom == L001.APP.CODE);
//                if (currentToken != null)
//                {
//                    oldPlayerId = currentToken.PlayerId;
//                    oldSerial = currentToken.Serial;
//                    if (currentToken.PlayerId == user.PlayerId && currentToken.Serial == user.Serial)
//                    {
//                        currentToken.JwtToken = token;
//                        currentToken.RefreshToken = refreshToken;
//                        currentToken.ValidateToken = "";
//                        currentToken.LatOfMap = user.LatOfMap;
//                        currentToken.LongOfMap = user.LongOfMap;
//                        currentToken.Browser = user.Browser;
//                        currentToken.PlayerId = user.PlayerId;
//                        currentToken.Serial = user.Serial;
//                        currentToken.Timeout = DateTimeOffset.Now.AddMinutes(Constants.API_EXPIRES_MINUTE);
//                        addNewToken = false;
//                    }
//                    else
//                    {
//                        currentToken.Timeout = DateTimeOffset.Now.AddMinutes(-1);
//                    }
//                }

//                if (addNewToken)
//                {
//                    userToken = new TblUserToken()
//                    {
//                        UserId = userDB.Id,
//                        JwtToken = token,
//                        RefreshToken = refreshToken,
//                        ValidateToken = "",
//                        LatOfMap = user.LatOfMap,
//                        LongOfMap = user.LongOfMap,
//                        Browser = user.Browser,
//                        PlayerId = user.PlayerId,
//                        Serial = user.Serial,
//                        LoginFrom = loginFrom,
//                        Timeout = DateTimeOffset.Now.AddMinutes(Constants.API_EXPIRES_MINUTE),
//                    };
//                    _context.UserTokens.Add(userToken);
//                }

//                if (!String.IsNullOrEmpty(oldSerial) && user.Serial != oldSerial)
//                {
//                    lockToken = new TblUserToken()
//                    {
//                        UserId = userDB.Id,
//                        JwtToken = "",
//                        RefreshToken = "",
//                        ValidateToken = Guid.NewGuid().ToString(),
//                        LatOfMap = user.LatOfMap,
//                        LongOfMap = user.LongOfMap,
//                        Browser = user.Browser,
//                        PlayerId = user.PlayerId,
//                        Serial = user.Serial,
//                        LoginFrom = L001.BATCH.CODE,
//                        Timeout = DateTimeOffset.Now.AddMinutes(2),
//                    };
//                    _context.UserTokens.Add(lockToken);
//                }

//                userDB.LastLogin = DateTimeOffset.Now;

//                if (String.IsNullOrEmpty(userDB.MyInviteCode))
//                {
//                    userDB.MyInviteCode = userDB.Information.CustomerId;
//                }

//                transaction = await _context.Database.BeginTransactionAsync();
//                await _context.SaveChangesAsync();
//                await transaction?.CommitAsync();

//                result.Data.Add("Token", token);
//                result.Data.Add("RefreshToken", refreshToken);
//                result.Data.Add("Expires", Constants.API_EXPIRES_MINUTE.ToString());
//                result.Data.Add("IsActive", userDB.IsActive ? "1" : "0");
//                result.Data.Add("IsAddSecurityQuestions", userDB.Questions.Count(x => !x.DelFlag) > 0 ? "1" : "0");

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Login thành công");

//                try
//                {
//                    string position = await GetPosition(user.LatOfMap, user.LongOfMap);

//                    if (!String.IsNullOrEmpty(oldSerial) && user.Serial != oldSerial)
//                    {
//                        _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] 2 thiết bị cùng đăng nhập");

//                        await _hubContext.Clients.User(userDB.Phone).SendAsync(SecurityAlertHub.MULTI_LOGIN_ERROR, new
//                        {
//                            LatOfMap = user.LatOfMap,
//                            LongOfMap = user.LongOfMap,
//                            Browser = user.Browser,
//                            Ip = userDB.UpdatedIp,
//                            Position = position,
//                            TimeLogin = now,
//                            LockToken = lockToken == null ? "" : lockToken.ValidateToken
//                        });

//                        await _notificationService.SendQueue(
//                            NOTIFICATION_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.Title,
//                                NOTIFICATION_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.GetContent(new {
//                                    Now = now.ToString("dd.MM.yyyy HH:mm:ss"),
//                                    Position = position,
//                                    IP = userDB.UpdatedIp,
//                                    Device = user.Browser
//                                }),
//                            userDB.Id,
//                            T009.COMMON.CODE,
//                            "",
//                            false,
//                            null,
//                            null,
//                            null,
//                            oldPlayerId,
//                            null,
//                            P007.HIGHT.CODE
//                        );

//                        await _mailService.SendQueue(
//                            userDB.Email,
//                            EMAIL_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.Title,
//                            EMAIL_TEMPLATE.WARNING_LOG_IN_FROM_A_STRANGE_DEVICE.Template,
//                            Newtonsoft.Json.JsonConvert.SerializeObject(new
//                            {
//                                FirstName = userDB.Information.FirstName,
//                                LastName = userDB.Information.LastName,
//                                Browser = user.Browser,
//                                Ip = userDB.UpdatedIp,
//                                Position = position,
//                                TimeLogin = now
//                            }),
//                            "",
//                            P007.HIGHT.CODE
//                        );
//                        result.Data.Add("LoginedInOtherDevice", "1");
//                    }
//                    else
//                    {
//                        result.Data.Add("LoginedInOtherDevice", "0");
//                        if (lastLogin != null && !String.IsNullOrEmpty(lastLogin.Serial) && lastLogin.Serial != user.Serial)
//                        {
//                            _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Đăng nhập từ thiết bị lạ");
//                            await _mailService.SendQueue(
//                                userDB.Email,
//                                EMAIL_TEMPLATE.FM_PLUS_ACCOUNT_LOG_IN_INFORMATION.Title,
//                                EMAIL_TEMPLATE.FM_PLUS_ACCOUNT_LOG_IN_INFORMATION.Template,
//                                Newtonsoft.Json.JsonConvert.SerializeObject(new
//                                {
//                                    FirstName = userDB.Information.FirstName,
//                                    LastName = userDB.Information.LastName,
//                                    Browser = user.Browser,
//                                    Ip = userDB.UpdatedIp,
//                                    Position = position,
//                                    TimeLogin = now
//                                }),
//                                "",
//                                P007.HIGHT.CODE
//                            );
//                        }
//                    }
//                }
//                catch { }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");
//                throw e;
//            }

//        }

//        private async Task<ResponseInfo> HandleSignUpBySocial(UserSocial user, UserInput userInput, bool fromWeb = false)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
//                ResponseInfo result = new ResponseInfo();

//                // _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Kiểm tra Captcha");
//                // // Kiểm tra Captcha
//                // if (!checkCaptcha(user.CaptchaToken))
//                // {
//                //     result.MsgNo = MSG_NO.CAPTCHA_INVALID;
//                //     result.Code = CodeResponse.HAVE_ERROR;
//                //     return result;
//                // }
//                string phone = "";
//                if (!String.IsNullOrEmpty(userInput.Phone))
//                {
//                    phone = Security.Base64Encode(userInput.Phone);
//                }

//                string Ip = _context.GetRequestIp();
//                string Serial = _context.GetRequestSerial();
//                var setting = await GetSetting();
//                DateTimeOffset DateExpires = DateTimeOffset.Now.AddDays(-1);

//                TblUser newUser = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Email == userInput.Email && !String.IsNullOrEmpty(userInput.Email) && !x.DelFlag && (!x.IsActive || x.Password == null || x.Password == ""));
//                if (newUser == null)
//                {
//                    DbConnection _connection = _context.GetConnection();
//                    string sqlCountUser = $@"
//                        SELECT
//                            Max(""UserInfos"".""CustomerId"")
//                        FROM ""Users""
//                        INNER JOIN ""UserInfos""
//                        ON (
//                            ""UserInfos"".""Id"" = ""Users"".""UserInfoId""
//                        )
//                        WHERE
//                            to_char(""Users"".""CreatedAt"", 'YYYYMMDD') = '{DateTime.Today.ToString("yyyyMMdd")}'
//                        AND ""UserInfos"".""CustomerId"" <> '_';
//                    ";
//                    string CutomerIdMax = await _connection.QueryFirstOrDefaultAsync<string>(sqlCountUser);
//                    int CustomerIdIndex = String.IsNullOrEmpty(CutomerIdMax) ? 0 : Int32.Parse(CutomerIdMax.Substring(CutomerIdMax.Length - 4, 4));
//                    string CustomerId = "0000" + (CustomerIdIndex + 1);
//                    CustomerId = DateTime.Today.ToString("yyMMdd") + CustomerId.Substring(CustomerId.Length - 4, 4);
//                    newUser = new TblUser()
//                    {
//                        Phone = phone,
//                        Email = userInput.Email,
//                        IsVerifyAccount = V002.UN_VERIFY.CODE,
//                        IsActive = true,
//                        Serial = Serial,
//                        LoginFailed = 0,
//                        CountChangeEmail = 0,
//                        CountChangeInfo = 0,
//                        CountChangePhone = 0,
//                        Status = A001.NORMAL.CODE,
//                        Type = T011.PERSONAL.CODE,
//                        DisplayNameOption = D001.SHOW_FULLNAME.CODE,
//                        CountChangeUserName = 0,
//                        CountChangeGender = 0,
//                        CountChangeBirthday = 0,
//                        CountChangeAddress = 0,
//                        FacebookId = userInput.FacebookId,
//                        GoogleId = userInput.GoogleId,
//                        Information = new TblUserInfo()
//                        {
//                            CustomerCode = phone,
//                            CustomerId = CustomerId,
//                            Avatar = userInput.Avatar ?? Constants.AVATAR,
//                            Cover = Constants.COUVER,
//                            VerifyAvatar = V002.VERIFYED.CODE,
//                            VerifyAvatarAt = DateTimeOffset.Now,
//                            FirstName = userInput.Firstname,
//                            LastName = userInput.Lastname,
//                        },
//                        Point = new Databases.Schemas.UserPoint()
//                        {
//                            RankPoint = 0,
//                            NormalPoint = 0,
//                            GoldPoint = 0,
//                            IsLockNormalPoint = false
//                        }
//                    };
//                    newUser.Roles.Add(new TblUserRole()
//                    {
//                        RoleId = R001.USER.CODE
//                    });
//                }
//                else
//                {
//                    result.MsgNo = MSG_NO.EMAIL_HAD_USED;
//                    result.Code = CodeResponse.HAVE_ERROR;
//                    return result;
//                }

//                TblUserToken userToken = new TblUserToken()
//                {
//                    ValidateToken = Helpers.GenerationOTP(),
//                    LatOfMap = user.LatOfMap,
//                    LongOfMap = user.LongOfMap,
//                    Browser = user.Browser,
//                    Timeout = DateTimeOffset.Now.AddSeconds(setting.ResendOTPTime),
//                };
//                newUser.Tokens.Add(userToken);
//                _context.Users.Add(newUser);
//                transaction = await _context.Database.BeginTransactionAsync();
//                await _context.SaveChangesAsync();
//                await transaction?.CommitAsync();
//                // await _otpService.SendBySms(user.Phone, userToken.ValidateToken);

//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
//                return await HandleLoginSocial(user, newUser, fromWeb);
//            }
//            catch (Exception e)
//            {
//                await _context.RollBack(transaction);
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");

//                throw e;
//            }

//        }
//        private string FIELDS = "name,email,first_name,last_name,birthday,gender,picture{url}";
//        public async Task<ResponseInfo> AuthFacebook(UserSocial user, bool fromWeb = false)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                ResponseInfo result = new ResponseInfo();

//                IConfigurationSection Facebook = _configuration.GetSection("Facebook");
//                var setting = await GetSetting();
//                string Endpoint = String.IsNullOrEmpty(setting.FacebookEndpoint) ? setting.FacebookEndpoint : Facebook["Endpoint"];

//                UriBuilder builder = new UriBuilder($"{Endpoint}/me?fields={FIELDS}&access_token={user.Token}");
//                HttpClient client = new HttpClient();
//                HttpResponseMessage response = await client.GetAsync(builder.ToString());

//                if (response.IsSuccessStatusCode)
//                {
//                    var jsonData = response.Content.ReadAsStringAsync().Result;
//                    _logger.LogInformation(jsonData);
//                    var jsonObject = JObject.Parse(jsonData);
//                    var UserInput = new UserInput
//                    {
//                        Email = jsonObject.ContainsKey("email") ? (string)jsonObject.GetValue("email") : "",
//                        Phone = jsonObject.ContainsKey("phone") ? (string)jsonObject.GetValue("phone") : "",
//                        Firstname = jsonObject.ContainsKey("first_name") ? (string)jsonObject.GetValue("first_name") : "",
//                        Lastname = jsonObject.ContainsKey("last_name") ? (string)jsonObject.GetValue("last_name") : "",
//                        FacebookId = jsonObject.ContainsKey("id") ? (string)jsonObject.GetValue("id") : "",
//                        Gender = jsonObject.ContainsKey("email") ? ((string)jsonObject.GetValue("gender") == "male" ? G001.MALE.CODE : ((string)jsonObject.GetValue("gender") == "female" ? G001.FEMALE.CODE : "")) : "",
//                        Avatar = jsonObject.ContainsKey("picture") ? (string)jsonObject.GetValue("picture")["data"]["url"] : ""
//                    };
//                    if (!String.IsNullOrEmpty((string)jsonObject.GetValue("birthday")))
//                    {
//                        UserInput.Birthday = (DateTime)jsonObject.GetValue("birthday");
//                    }
//                    TblUser userDB = null;
//                    if (userDB == null && !String.IsNullOrEmpty((string)jsonObject.GetValue("phone")))
//                    {
//                        userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Phone == Security.Base64Encode((string)jsonObject.GetValue("phone")) && !x.DelFlag);
//                    }
//                    if (userDB == null && !String.IsNullOrEmpty((string)jsonObject.GetValue("email")))
//                    {
//                        userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Email == (string)jsonObject.GetValue("email") && !x.DelFlag);
//                    }
//                    if (userDB == null && !String.IsNullOrEmpty((string)jsonObject.GetValue("id")))
//                    {
//                        userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.FacebookId == UserInput.FacebookId && !x.DelFlag);
//                    }
//                    // Handle for login
//                    if (userDB != null)
//                    {
//                        userDB.FacebookId = UserInput.FacebookId;
//                        _context.SaveChanges();
//                        return await HandleLoginSocial(user, userDB, fromWeb);
//                    }
//                    else
//                    {
//                        // Handle for signup
//                        return await HandleSignUpBySocial(user, UserInput, fromWeb);
//                    }
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");
//                throw e;
//            }
//        }

//        private string GenerateAppleJwtToken(IConfigurationSection AppleId)
//        {
//            string iss = AppleId["TeamId"]; // your account's team ID found in the dev portal
//            string aud = AppleId["Aud"];
//            string sub = AppleId["ClientId"]; // same as client_id
//            string keyId = AppleId["KeyId"];
//            var now = DateTime.UtcNow;

//            // contents of your .p8 file
//            string privateKey = AppleId["PrivateKey"];
//            var ecdsa = ECDsa.Create();
//            ecdsa?.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);

//            var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();

//            var symmetricKey = new ECDsaSecurityKey(ecdsa);
//            symmetricKey.KeyId = keyId;
//            string token = handler.CreateToken(new SecurityTokenDescriptor
//            {
//                Issuer = iss,
//                Audience = aud,
//                Claims = new Dictionary<string, object> { { "sub", sub } },
//                Expires = now.AddMinutes(5), // expiry can be a maximum of 6 months - generate one per request or re-use until expiration
//                IssuedAt = now,
//                NotBefore = now,
//                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.EcdsaSha256),

//            });
//            _logger.LogInformation(token);
//            return token;
//        }
//        public async Task<ResponseInfo> AuthAppleId(UserSocial user, bool fromWeb = false)
//        {
//            IDbContextTransaction transaction = null;
//            string method = GetActualAsyncMethodName();

//            try
//            {
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");

//                ResponseInfo result = new ResponseInfo();

//                IConfigurationSection AppleId = _configuration.GetSection("AppleId");
//                var setting = await GetSetting();
//                string Endpoint = AppleId["Endpoint"];
//                string ClientId = AppleId["ClientId"];
//                string ClientSecret = GenerateAppleJwtToken(AppleId);
//                string Code = user.Token;
//                string GrandType = "authorization_code";
//                UriBuilder builder = new UriBuilder($"{Endpoint}");
//                HttpClient client = new HttpClient();

//                var data = new[]
//                {
//                    new KeyValuePair<string, string>("client_id", ClientId),
//                    new KeyValuePair<string, string>("client_secret", ClientSecret),
//                    new KeyValuePair<string, string>("code", Code),
//                    new KeyValuePair<string, string>("grant_type", GrandType),
//                };
//                // HttpResponseMessage response = await client.PostAsync(builder.ToString(), multiForm);
//                HttpResponseMessage response = await client.PostAsync(builder.ToString(), new FormUrlEncodedContent(data));
//                if (response.IsSuccessStatusCode)
//                {
//                    var jsonData = response.Content.ReadAsStringAsync().Result;
//                    _logger.LogInformation(jsonData);
//                    var jsonObject = JObject.Parse(jsonData);
//                    var idToken = (string)jsonObject.GetValue("id_token");
//                    if (!String.IsNullOrEmpty(idToken))
//                    {
//                        var handler = new Microsoft.IdentityModel.JsonWebTokens.JsonWebTokenHandler();
//                        var claimResult = handler.ReadJsonWebToken(idToken);
//                        var Email = claimResult.Claims.FirstOrDefault(claim => claim.Type == "email")?.Value;
//                        if (!String.IsNullOrEmpty(Email))
//                        {
//                            // TODO
//                            var Firstname = claimResult.Claims.FirstOrDefault(claim => claim.Type == "firstname")?.Value;
//                            var Lastname = claimResult.Claims.FirstOrDefault(claim => claim.Type == "lastname")?.Value;
//                            var Gender = claimResult.Claims.FirstOrDefault(claim => claim.Type == "gender")?.Value;
//                            var Avatar = claimResult.Claims.FirstOrDefault(claim => claim.Type == "avatar")?.Value;
//                            var Phone = claimResult.Claims.FirstOrDefault(claim => claim.Type == "phone")?.Value;
//                            var Birthday = claimResult.Claims.FirstOrDefault(claim => claim.Type == "birthday")?.Value;
//                            var UserInput = new UserInput
//                            {
//                                Email = Email,
//                                Phone = Phone,
//                                Firstname = Firstname,
//                                Lastname = Lastname,
//                                Gender = Gender,
//                                Avatar = Avatar
//                            };
//                            TblUser userDB = null;
//                            if (userDB == null && !String.IsNullOrEmpty(Phone))
//                            {
//                                userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Phone == Security.Base64Encode(Phone) && !x.DelFlag);
//                            }
//                            if (userDB == null && !String.IsNullOrEmpty(Email))
//                            {
//                                userDB = _context.Users.Include(x => x.Information).FirstOrDefault(x => x.Email == Email && !x.DelFlag);
//                            }
//                            // Handle for login
//                            if (userDB != null)
//                            {
//                                _context.SaveChanges();
//                                return await HandleLoginSocial(user, userDB, fromWeb);
//                            }
//                            else
//                            {
//                                // Handle for signup
//                                return await HandleSignUpBySocial(user, UserInput, fromWeb);
//                            }
//                        }
//                    }

//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");

//                return result;
//            }
//            catch (Exception e)
//            {
//                if (transaction != null)
//                {
//                    await transaction.RollbackAsync();
//                }
//                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Exception: {e.Message}");
//                throw e;
//            }
//        }
//    }
//}
