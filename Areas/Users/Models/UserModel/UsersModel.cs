using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CapstoneProject.Models.Schemas;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using UserData = CapstoneProject.Databases.Schemas.System.Users.User;
using UserToken = CapstoneProject.Databases.Schemas.System.Users.UserToken;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Identity.Client;
using System.Data.Common;
using Dapper;

namespace CapstoneProject.Areas.Users.Models.UserModel
{

    public interface IUsersModel
    {
        /// <summary>
        /// Lấy thông tin chi tiết tài khoản
        /// </summary>
        /// <returns></returns>
        Task<UserData> GetDetailUser(int id);
        /// <summary>
        /// Lấy thông tin tài khoản đang đăng nhập
        /// </summary>
        /// <returns></returns>
        Task<UserData> GetUserIsLogin();
    }
    public class UserModel : CapstoneProjectModels, IUsersModel
    {
        private readonly ILogger<UserModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        private readonly IHttpContextAccessor _httpContext;

        public UserModel(
            IConfiguration configuration,
            ILogger<UserModel> logger,
            IHttpContextAccessor httpContextAccessor,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _httpContext = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<UserData> GetDetailUser(int id)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                UserData userInfo = await _context.Users.Where(x => !x.DelFlag && x.Id == id).FirstOrDefaultAsync();
                if(userInfo == null)
                {
                    return null;
                }
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return userInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get detail user Error: {ex}");
                throw ex;
            }
        }
        /// <summary>
        /// Lấy thông tin tài khoản đang đăng nhập
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserData> GetUserIsLogin()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                //UserData userInfo = await _context.Users.Where(x => !x.DelFlag && x.Id == id).FirstOrDefaultAsync();
                string accessToken = _httpContext.HttpContext.Request.Headers["Authorization"];
                UserData userInfo = new UserData();
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
                        userInfo = await _context.Users.Where(x => !x.DelFlag && x.Id == userToken.UserId).FirstOrDefaultAsync();
                        if (userInfo == null)
                        {
                            return null;
                        }
                    }
                }
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return userInfo;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get detail user Error: {ex}");
                throw ex;
            }
        }
    }
}
