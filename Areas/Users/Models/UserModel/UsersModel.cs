using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CapstoneProject.Models.Schemas;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using UserData = CapstoneProject.Databases.Schemas.System.Users.User;
using UserToken = CapstoneProject.Databases.Schemas.System.Users.UserToken;
using District = CapstoneProject.Databases.Schemas.Setting.Districts;
using Commune = CapstoneProject.Databases.Schemas.Setting.Communes;
using Province = CapstoneProject.Databases.Schemas.Setting.Provinces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using Microsoft.Identity.Client;
using System.Data.Common;
using Dapper;
using System.Reflection;
using CapstoneProject.Commons;
using CapstoneProject.Databases.Schemas.Setting;

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
        Task<UserInfo> GetUserIsLogin();
        /// <summary>
        /// Lấy danh sách các tỉnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Province>> GetProvinces();
        /// <summary>
        /// Lấy danh sách các huyện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<District>> GetDistrict(string provinceId);
        /// <summary>
        /// Lấy danh sách các thị xã
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<Commune>> GetCommune(string districtId);
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
        /// <summary>
        /// Lấy thông tin cá nhân theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
        public async Task<UserInfo> GetUserIsLogin()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
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
                                LastSecurityString = x.LastSecurityString
                            })
                            .FirstOrDefaultAsync();
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
        /// <summary>
        /// Lấy danh sách các tỉnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Province>> GetProvinces()
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                List<Province> provinces = new List<Province>();
                provinces = await _context.Provinces.Where(x => !x.DelFlag).ToListAsync();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return provinces;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get detail user Error: {ex}");
                throw ex;
            }
        }
        // <summary>
        /// Lấy danh sách các huyện
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<District>> GetDistrict(string provinceId)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                List<District> districts = new List<District>();
                districts = await _context.Districts.Where(x => !x.DelFlag && x.ProvinceId == provinceId).ToListAsync();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return districts;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get detail user Error: {ex}");
                throw ex;
            }
        }

        // <summary>
        /// Lấy danh sách các thị xã
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<Commune>> GetCommune(string districtId)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                List<Commune> communes = new List<Commune>();
                communes = await _context.Communes.Where(x => !x.DelFlag && x.DistrictId == districtId).ToListAsync();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return communes;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get detail user Error: {ex}");
                throw ex;
            }
        }
    }
}
