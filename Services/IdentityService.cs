using System;
using System.Data.Common;
using System.Linq;
using System.Security.Claims;
using System.Web;
using Azure.Core;
using CapstoneProject.Databases;
using CapstoneProject.Databases.Schemas.System.Users;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace CapstoneProject.Services
{
    public interface IIdentityService
    {
        bool CheckPermission(string areaName = "", string controllerName = "", string functionName = "", string accountId = "0");
        string GetUserId();
        string GetToken();
        //string GetApiKey();
        string GetSerial();
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
