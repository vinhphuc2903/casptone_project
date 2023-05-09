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

namespace CapstoneProject.Areas.Users.Models.UserModel
{

    public interface IUsersModel
    {
        /// <summary>
        /// Lấy thông tin chi tiết tài khoản
        /// </summary>
        /// <returns></returns>
        Task<UserData> GetDetailUser(string id);
    }
    public class UserModel : CapstoneProjectModels, IUsersModel
    {
        private readonly ILogger<UserModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";
        

        public UserModel(
            IConfiguration configuration,
            ILogger<UserModel> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<UserData> GetDetailUser(string id)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                UserData userInfo = await _context.Users.Where(x => !x.DelFlag && x.Id.ToString() == id).FirstOrDefaultAsync();
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
