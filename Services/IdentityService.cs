using System;
using System.Web;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Azure.Core;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using CapstoneProject.Commons;
using CapstoneProject.Commons.CodeMaster;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Users;
using CapstoneProject.Models.Schemas;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Net;

namespace CapstoneProject.Services
{
    public interface IIdentityService
    {
        bool CheckPermission(string areaName = "", string controllerName = "", string functionName = "", string accountId = "0");
        string GetUserId();
        string GetToken();
        //string GetApiKey();
        string GetClientIpAddress();
        string GetSerial();
        Task<bool> CheckIndentifyUser(string Role);
        Task<UserInfo> GetUserIsLogin();
    }

    public class IdentityService : IIdentityService
    {

        private readonly IHttpContextAccessor _httpContext;
        private readonly DataContext _context;

        public IdentityService(IHttpContextAccessor httpContext, DataContext context)
        {
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetClientIpAddress()
        {
            var ipAddress = _httpContext.HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return ipAddress ?? String.Empty;
        }

        /// <summary>
        /// Lấy thông tin tài khoản đang đăng nhập
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserInfo> GetUserIsLogin()
        {
            IDbContextTransaction transaction = null;
            try
            {
                //UserData userInfo = await _context.Users.Where(x => !x.DelFlag && x.Id == id).FirstOrDefaultAsync();
                string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"];
                UserInfo userInfo = new UserInfo();
                if (!String.IsNullOrEmpty(accessToken))
                {
                    accessToken = accessToken.Replace("Bearer", "").Trim();
                    DbConnection _connection = _context.GetConnection();
                    string getCurrentUserSQL = $@"
                            SELECT
                                UserTokens.Id,
                                UserTokens.UserId
                            FROM UserTokens
                            WHERE
                                UserTokens.DelFlag = 0
                                -- AND UserTokens.UserId = @AccountId
                                AND UserTokens.JwtToken = @AccessToken
                                -- AND UserTokens.Timeout >= @CurrentTime;
                        ";
                    UserToken userToken = _connection.QueryFirstOrDefault<UserToken>(getCurrentUserSQL, new { AccessToken = accessToken });
                    _connection.Close();
                    if (userToken == null)
                    {
                        return null;
                    }
                    else
                    {
                        userInfo = await _context.Users
                            .Where(x => !x.DelFlag && x.Id == userToken.UserId)
                            .Select(x => new UserInfo()
                            {
                                Id = x.Id,
                                Username = x.Username,
                                Email = x.Email,
                                Phone = Security.Base64Decode(x.Phone),
                                DateOfBirth = x.DateOfBirth,
                                Name = x.Name,
                                Gender = x.Gender,
                                Address = x.Address,
                                DistrictId = x.DistrictId,
                                ProvinceId = x.ProvinceId,
                                CommuneId = x.CommuneId,
                                FirstSecurityString = x.FirstSecurityString,
                                LastSecurityString = x.LastSecurityString,
                                RoleId = x.Roles.Select(x => x.RoleId).ToList()
                            })
                            .FirstOrDefaultAsync();
                        if (userInfo == null)
                        {
                            return null;
                        }
                    }
                }
                return userInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Kiểm tra tài khoản đang đăng nhập có quyền không
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> CheckIndentifyUser(string Role)
        {
            IDbContextTransaction transaction = null;
            try
            {
                //UserData userInfo = await _context.Users.Where(x => !x.DelFlag && x.Id == id).FirstOrDefaultAsync();
                string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"];
                if (!String.IsNullOrEmpty(accessToken))
                {
                    accessToken = accessToken.Replace("Bearer", "").Trim();
                    DbConnection _connection = _context.GetConnection();
                    string getCurrentUserSQL = $@"
                            SELECT
                                UserTokens.Id,
                                UserTokens.UserId
                            FROM UserTokens
                            WHERE
                                UserTokens.DelFlag = 0
                                -- AND UserTokens.UserId = @AccountId
                                AND UserTokens.JwtToken = @AccessToken
                                -- AND UserTokens.Timeout >= @CurrentTime;
                        ";
                    UserToken userToken = _connection.QueryFirstOrDefault<UserToken>(getCurrentUserSQL, new { AccessToken = accessToken });
                    _connection.Close();
                    if (userToken == null)
                    {
                        return false;
                    }
                    else
                    {
                        var user  = await _context.Users
                            .Where(x => !x.DelFlag && x.Id == userToken.UserId
                                && x.Roles.Any(x => x.RoleId == Role)
                            )
                            .FirstOrDefaultAsync();
                        if (user == null)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetUserId()
        {
            try
            {
                string accountId = "0";
                ClaimsPrincipal user = null;
                if (_httpContext != null && _httpContext.HttpContext != null)
                {
                    user = _httpContext.HttpContext.User;
                }
                if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                {
                    var identity = user.Identity as ClaimsIdentity;
                    accountId = identity.Claims.Where(p => p.Type == "UserId").FirstOrDefault()?.Value;
                }
                if (accountId != "0")
                {
                    string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"];
                    if (!String.IsNullOrEmpty(accessToken))
                    {
                        accessToken = accessToken.Replace("Bearer", "").Trim();
                        DbConnection _connection = _context.GetConnection();
                        //string getCurrentUserSQL = $@"
                        //        SELECT
                        //            ""UserTokens"".""Id""
                        //        FROM ""UserTokens""
                        //        WHERE
                        //            ""UserTokens"".""DelFlag""          = false
                        //        -- AND ""UserTokens"".""UserId""           = {accountId}
                        //        AND ""UserTokens"".""JwtToken""         = '{accessToken}'
                        //        -- AND ""UserTokens"".""Timeout""          >= '{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")}';
                        //    ";
                        string getCurrentUserSQL = $@"
                            SELECT
                                UserTokens.Id
                            FROM UserTokens
                            WHERE
                                UserTokens.DelFlag = 0
                                -- AND UserTokens.UserId = @AccountId
                                AND UserTokens.JwtToken = @AccessToken
                                -- AND UserTokens.Timeout >= @CurrentTime;
                        ";
                        UserToken userToken = _connection.QueryFirstOrDefault<UserToken>(getCurrentUserSQL, new { AccessToken = accessToken });
                        _connection.Close();
                        if (userToken == null)
                        {
                            accountId = "0";
                        }
                    }
                    else
                    {
                        accountId = "0";
                    }
                }
                return accountId;
            }
            catch(Exception e)
            {
                return "0";
            }
        }

        public string GetToken()
        {
            try
            {
                string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"];
                if (!String.IsNullOrEmpty(accessToken))
                {
                    accessToken = accessToken.Replace("Bearer", "").Trim();
                }
                return accessToken;
            }
            catch
            {
                return "";
            }
        }

        //public string GetApiKey()
        //{
        //    try
        //    {
        //        string apiKey = "";
        //        string apiKeyRequest = _httpContext.HttpContext.Request.Headers["x-apikey"];
        //        if (String.IsNullOrEmpty(apiKeyRequest))
        //        {
        //            apiKeyRequest = _httpContext.HttpContext.Request.Query["ApiKey"];
        //        }
        //        if (!String.IsNullOrEmpty(apiKeyRequest))
        //        {
        //            apiKeyRequest = apiKeyRequest.Trim();
        //            DbConnection _connection = _context.GetConnection();
        //            string getCurrentApiKeySQL = $@"
        //                SELECT
        //                    ""ApiKeys"".""Key""
        //                ,   ""ApiKeys"".""Aud""
        //                FROM ""ApiKeys""
        //                WHERE
        //                    ""ApiKeys"".""DelFlag""             = false
        //                AND ""ApiKeys"".""Key""                 = '{apiKeyRequest}'
        //                AND (
        //                    ""ApiKeys"".""From""                IS NULL
        //                OR  ""ApiKeys"".""From""                <= '{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")}'
        //                )
        //                AND (
        //                    ""ApiKeys"".""To""                  IS NULL
        //                OR  ""ApiKeys"".""To""                  >= '{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")}'
        //                );
        //            ";
        //            ApiKey apiKeyDB = _connection.QueryFirstOrDefault<ApiKey>(getCurrentApiKeySQL);
        //            _connection.Close();
        //            if (apiKeyDB != null)
        //            {
        //                apiKey = apiKeyDB.Key;
        //            }
        //        }
        //        return apiKey;
        //    }
        //    catch
        //    {
        //        return "";
        //    }
        //}

        public string GetSerial()
        {
            try
            {
                string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"];
                if (!String.IsNullOrEmpty(accessToken))
                {
                    accessToken = accessToken.Replace("Bearer", "").Trim();
                    if (String.IsNullOrEmpty(accessToken))
                    {
                        return "";
                    }
                    DbConnection _connection = _context.GetConnection();
                    string getCurrentUserSQL = $@"
                        SELECT
                            ""UserTokens"".""Id""
                        ,   ""UserTokens"".""Serial""
                        FROM ""UserTokens""
                        WHERE
                            ""UserTokens"".""DelFlag""          = false
                        AND ""UserTokens"".""JwtToken""         = '{accessToken}'
                        AND ""UserTokens"".""Timeout""          >= '{DateTimeOffset.Now.ToString("yyyy-MM-ddTHH:mm:ss.fffffffK")}'
                        ORDER BY ""UserTokens"".""Id"" DESC;
                    ";
                    UserToken userToken = _connection.QueryFirstOrDefault<UserToken>(getCurrentUserSQL);
                    _connection.Close();
                    if (userToken == null)
                    {
                        return "";
                    }
                    else
                    {
                        return userToken.Serial;
                    }
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public bool CheckPermission(string areaName = "", string controllerName = "", string functionName = "", string accountId = "0")
        {
            try
            {
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
