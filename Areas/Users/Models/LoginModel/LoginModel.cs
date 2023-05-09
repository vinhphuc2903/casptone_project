using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Areas.Employee.Models.Schemas;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using CapstoneProject.Models.Schemas;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using UserData = CapstoneProject.Databases.Schemas.System.Users.User;
using UserRoleData = CapstoneProject.Databases.Schemas.System.Users.UserRole;
using UserTokenData = CapstoneProject.Databases.Schemas.System.Users.UserToken;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using CapstoneProject.Commons.CodeMaster;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CapstoneProject.Commons;
using CapstoneProject.Commons.Enum;
using CapstoneProject.Commons.Schemas;
using CapstoneProject.Areas.Users.Models.LoginModel.Schemas;
using System.Reflection;
using CapstoneProject.Databases.Schemas.System.Users;

namespace CapstoneProject.Areas.Users.Models.LoginModel
{
	public interface ILoginModel
	{
        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        Task<ResponseInfo> CheckAccount(LoginInfo user);

        /// <summary>
        /// Tạo tài khoản
        /// </summary>
        /// <returns>ResponseInfo</returns>
        Task<ResponseInfo> CreateUser(AccountInfo accountInfo);


    }
	public class LoginModel : CapstoneProjectModels, ILoginModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IConfiguration _configuration;
        private string _className = "";

        public LoginModel(
            IConfiguration configuration,
            ILogger<LoginModel> logger,
            IServiceProvider provider
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Lấy token của tài khoản đang đăng nhập
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetTokenLogin(string userId)
        {
            IDbContextTransaction transaction = null;
            try
            {
                // UserData userDB = _context.Users.Include(x => x.Employees).FirstOrDefault(x => !x.DelFlag && x.Roles.Count(y => !y.DelFlag && y.RoleId == R001.DEVELOPER.CODE) > 0);
                UserData userDB = await _context.Users.Where(x => !x.DelFlag && x.Id.ToString() == userId).FirstOrDefaultAsync();
                if (userDB != null)
                {
                    //DateTimeOffset now = DateTimeOffset.Now.AddMinutes(1);
                    string token = GenerationJWTCode(userDB.Id, userDB.Phone, userDB.Username);
                    UserTokenData userToken = _context.UserTokens.FirstOrDefault(x => !x.DelFlag && x.UserId == userDB.Id); // && x.Timeout != null && x.Timeout > now);
                    if (userToken == null)
                    {
                        userToken = new UserTokenData()
                        {
                            UserId = userDB.Id,
                            JwtToken = GenerationJWTCode(userDB.Id, userDB.Phone, userDB.Username),
                            ValidateToken = "",
                            Timeout = DateTimeOffset.Now.AddMinutes(Constants.API_EXPIRES_MINUTE),
                        };
                        _context.UserTokens.Add(userToken);
                        transaction = await _context.Database.BeginTransactionAsync();
                        await _context.SaveChangesAsync();
                        await transaction?.CommitAsync();
                    }
                    return userToken.JwtToken;
                }
                return "";
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                return "";
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="phone"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        public async Task<ResponseInfo> CheckAccount(LoginInfo user)
        {
            IDbContextTransaction transaction = null;
            try
            {
                ResponseInfo result = new ResponseInfo();
                //UserData userDB = _context.Users.Include(x => x.Employees)
                //    .FirstOrDefault(x => !x.DelFlag && x.Employees != null && (x.Username == user.Username || x.Employees.Email == user.Username)
                //    && x.Roles.Count(y => !y.DelFlag && y.RoleId == R001.DEVELOPER.CODE) > 0);
                //Kiểm tra username
                UserData userNameDB = await _context.Users.Where(
                        x => !x.DelFlag && x.Username == user.Username
                    ).FirstOrDefaultAsync();
                if (userNameDB == null)
                {
                    result.MsgNo = MSG_NO.USERNAME_OR_PASSWORD_NOT_INCORRECT;
                    result.Code = CodeResponse.HAVE_ERROR;
                }
                UserData userDB = await _context.Users.Where(
                        x => !x.DelFlag
                        && x.Username == user.Username
                        && x.Password == Security.GetMD5(user.Password, userNameDB.FirstSecurityString, userNameDB.LastSecurityString)
                    ).FirstOrDefaultAsync();
                if (userDB == null)
                {
                    result.MsgNo = MSG_NO.USERNAME_OR_PASSWORD_NOT_INCORRECT;
                    result.Code = CodeResponse.HAVE_ERROR;
                }
                else
                {
                    result.Data.Add("Token", await GetTokenLogin(userDB.Id.ToString()));
                    result.Data.Add("Name", userDB.Name);
                }
                return result;
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                throw e;
            }
        }

        public async Task<ResponseInfo> CreateUser(AccountInfo accountInfo)
        {
            IDbContextTransaction transaction = null;
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                string phone = Security.Base64Encode(accountInfo.Phone);
                var userPhone = await _context.Users.Where(x => !x.DelFlag && x.Phone == phone).FirstOrDefaultAsync();
                // Kiểm tra số điện thoại
                if(userPhone != null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.PHONE_IS_USED;
                    return response;
                }
                // Kiểm tra email
                var userEmail = await _context.Users.Where(x => !x.DelFlag && x.Email == accountInfo.Email).FirstOrDefaultAsync();
                if(userEmail != null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.EMAIL_IS_USED;
                    return response;
                }
                //Kiểm tra username
                var userName = await _context.Users.Where(x => !x.DelFlag && x.Email == accountInfo.Username).FirstOrDefaultAsync();
                if (userEmail != null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.USERNAME_IS_USED;
                    return response;
                }

                if(!String.IsNullOrEmpty(accountInfo.Gender))
                {
                    if (accountInfo.Gender != "M" && accountInfo.Gender != "O" && accountInfo.Gender != "F")
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.GENDER_NOT_EXIST;
                        return response;
                    }
                }
                string firstSecurityString = Helpers.RenderToken("", 10);
                string lastSecurityString = Helpers.RenderToken("", 20).Substring(10, 10);

                // Tạo tài khoản mới
                UserData userData = new UserData()
                {
                    Username = accountInfo.Username,
                    Password = Security.GetMD5(accountInfo.Password, firstSecurityString, lastSecurityString),
                    FirstSecurityString = firstSecurityString,
                    LastSecurityString = lastSecurityString,
                    Email = accountInfo.Email,
                    Name = accountInfo.Name,
                    DateOfBirth = accountInfo.DateOfBirth,
                    Phone = phone,
                    Gender = accountInfo.Gender
                };
                // Lấy id lớn nhất tài khoản hiện tại
                var idMax = await _context.Users.Where(x => !x.DelFlag).OrderByDescending( x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
                
                //Thêm user vào database
                _context.Users.Add(userData);
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                //Thêm userRoles vào database
                //Thêm quyền cho tài khoản
                UserRoleData userRole = new UserRoleData
                {
                    RoleId = R001.USER.CODE,
                    UserId = userData.Id
                };
                _context.UserRoles.Add(userRole);
                await _context.SaveChangesAsync();

                await transaction?.CommitAsync();
                //Thêm token
                response.Data.Add("Token", Security.Base64Encode(await GetTokenLogin(userData.Id.ToString())));
                return response;
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                throw e;
            }
        }
        private string GenerationJWTCode(int userId, string phone, string username)
        {
            string C_JWT = "Audience";
            string C_JWT_SECRET_KEY = "Secret";
            string C_JWT_ISSUER = "Iss";
            string C_JWT_AUDIENCE = "Aud";
            var now = DateTime.Now;
            var audienceConfig = _configuration.GetSection(C_JWT);
            // Khởi tạo Claim
            var claims = new Claim[] {
                new Claim ("UserId", userId.ToString()),
                new Claim ("Phone", phone),
                new Claim ("Username", username),
                new Claim(JwtRegisteredClaimNames.Sub, phone),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim (JwtRegisteredClaimNames.Iat, now.ToUniversalTime ().ToString (), ClaimValueTypes.Integer64)
            };
            // Khởi tạo SymmetricSecurityKey
            var signingKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(audienceConfig[C_JWT_SECRET_KEY] != null ? audienceConfig[C_JWT_SECRET_KEY] : Constants.JWT_SECRET_KEY));

            // Khởi tạo JwtSecurityToken
            var jwt = new JwtSecurityToken(
                issuer: audienceConfig[C_JWT_ISSUER] != null ? audienceConfig[C_JWT_ISSUER] : Constants.JWT_ISSUER,
                audience: audienceConfig[C_JWT_AUDIENCE] != null ? audienceConfig[C_JWT_AUDIENCE] : Constants.JWT_AUD,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(Constants.API_EXPIRES_MINUTE)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

    }
}

