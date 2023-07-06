using System.Collections.Generic;
using System.Net.NetworkInformation;
using CapstoneProject.Areas.Employee.Models.Schemas;
using CapstoneProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using CapstoneProject.Models.Schemas;
//using EmployeeData = CapstoneProject.Databases.Schemas.System.Employee.Employees;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;
using CapstoneProject.Areas.Film.Models.FilmModels.Schemas;
using CapstoneProject.Areas.Users.Models.UserModel.Schemas;
using CapstoneProject.Areas.Users.Models.LoginModel.Schemas;
using CapstoneProject.Commons;
using CapstoneProject.Commons.CodeMaster;
using CapstoneProject.Commons.Enum;
using CapstoneProject.Commons.Schemas;
using Microsoft.VisualStudio.TextManager.Interop;
using UserData = CapstoneProject.Databases.Schemas.System.Users.User;
using UserRoleData = CapstoneProject.Databases.Schemas.System.Users.UserRole;
using UserTokenData = CapstoneProject.Databases.Schemas.System.Users.UserToken;
using EmployeeDb = CapstoneProject.Databases.Schemas.System.Employee.Employees;
using CapstoneProject.Areas.Users.Models.LoginModel;
using CapstoneProject.Areas.Users.Models.UserModel;
using CapstoneProject.Services;

namespace CapstoneProject.Areas.Employee.Models
{
   
    public interface IEmployeesModel
    {
        /// <summary>
        /// Lấy danh sách nhân viên
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<EmployeeData>> GetEmployees(Schemas.SearchConditon searchConditon);
        /// <summary>
        /// Tạo nhân viên mới
        /// </summary>
        /// <param name="accountInfo"></param>
        /// <returns></returns>
        Task<ResponseInfo> CreateEmployee(EmployeeData accountInfo);
        /// <summary>
        /// Cập nhật tài khoản nhân viên
        /// </summary>
        /// <param name="employeeData"></param>
        /// <returns></returns>
        Task<ResponseInfo> UpdateAccountEmployee(EmployeeData employeeData);
        /// <summary>
        /// Xóa tài khoản nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ResponseInfo> DeleteAccountEmployee(int id);

    }
    public class EmployeesModel : CapstoneProjectModels, IEmployeesModel
    {
        private readonly ILogger<EmployeesModel> _logger;
        private readonly IConfiguration _configuration;
        private readonly ILoginModel _loginModel;
        private readonly IUsersModel _usersModel;
        private readonly IIdentityService _indentityService;
        private string _className = "";
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public EmployeesModel(
            IConfiguration configuration,
            ILogger<EmployeesModel> logger,
            IServiceProvider provider,
            ILoginModel loginModel,
            IUsersModel usersModel,
            IIdentityService identityService
        ) : base(provider)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _className = GetType().Name;
            _loginModel = loginModel ?? throw new ArgumentNullException(nameof(loginModel));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _usersModel = usersModel ?? throw new ArgumentNullException(nameof(usersModel));
            _indentityService = identityService ?? throw new ArgumentNullException(nameof(identityService));
        }

        public async Task<IEnumerable<EmployeeData>> GetEmployees(Schemas.SearchConditon searchCondition)
        {
            string method = GetActualAsyncMethodName();
            IDbContextTransaction transaction = null;
            try
            {
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                IEnumerable<EmployeeData> employees = await _context.Employee
                    .Where(x => !x.DelFlag
                        && x.User.Roles.Any(x => x.RoleId == R001.EMP.CODE || x.RoleId == R001.CSKH.CODE)
                        && (String.IsNullOrEmpty(searchCondition.Name)
                            || EF.Functions.Collate(x.User.Name.Replace(" ", ""), "Latin1_General_CI_AI").Contains(EF.Functions.Collate(searchCondition.Name.Replace(" ", ""), "Latin1_General_CI_AI")))
                        && (searchCondition.DateStart == null
                            || x.DateStart.Date >= searchCondition.DateStart.Value.Date)
                        && (String.IsNullOrEmpty(searchCondition.EmployeeCode)
                            || EF.Functions.Collate(x.EmployeeCode.Replace(" ", ""), "Latin1_General_CI_AI").Contains(EF.Functions.Collate(searchCondition.EmployeeCode.Replace(" ", ""), "Latin1_General_CI_AI")))
                        && (String.IsNullOrEmpty(searchCondition.Id.ToString())
                            || x.Id == searchCondition.Id
                        )
                        && (String.IsNullOrEmpty(searchCondition.BranchId.ToString())
                            || x.BranchId == searchCondition.BranchId
                        )
                        && (String.IsNullOrEmpty(searchCondition.PositionId.ToString())
                            || x.PositionId == searchCondition.PositionId
                        )
                    )
                    .Include(x => x.User)
                    .Include(x => x.Branches)
                    .Include(x => x.User.Roles)
                    .Include(x => x.User.Communes)
                    .Include(x => x.User.Districts)
                    .Include(x => x.User.Provinces)
                    .Select(x => new EmployeeData()
                    {
                        Id = x.Id,
                        Name = x.User.Name,
                        EmployeeCode = x.EmployeeCode,
                        Phone = Security.Base64Decode(x.Phone),
                        Email = x.Email,
                        DateOfBirth = x.User.DateOfBirth,
                        Gender = x.User.Gender,
                        DateStart = x.DateStart,
                        PositionId = x.PositionId,
                        PositionName = x.Positions.Name,
                        BranchId = x.BranchId,
                        BranchName = x.Branches.Name,
                        CommuneName = x.User.Communes.Name,
                        DistrictName = x.User.Districts.Name,
                        ProvinceName = x.User.Provinces.Name,
                        CommuneId = x.User.CommuneId,
                        ProvinceId = x.User.ProvinceId,
                        DistrictId = x.User.DistrictId,
                    })
                    .ToListAsync();
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] End");
                return employees;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Get Employee Forecast Error: {ex}");
                throw ex;
            }
        }
        public async Task<ResponseInfo> CreateEmployee(EmployeeData accountInfo)
        {
            IDbContextTransaction transaction = null;
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return response;
                }
                _logger.LogInformation($"[{AppState.Instance.RequestId}][{_className}][{method}] Start");
                string phone = Security.Base64Encode(accountInfo.Phone);
                // Lấy thông tin tài khoản đang login
                var userLogin = await _usersModel.GetUserIsLogin();
                foreach (var role in userLogin.RoleId)
                {
                    if (role == R001.CSKH.CODE || role == R001.EMP.CODE || role == R001.USER.CODE)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                        return response;
                    }
                }
                //Kiểm tra tên đăng nhập >= 8
                if (accountInfo.Username.Length < 6)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.LENGTH_USERNAME_NOT_OK_6;
                    return response;
                }
                var userPhone = await _context.Employee.Where(x => !x.DelFlag && x.Phone == phone).FirstOrDefaultAsync();
                // Kiểm tra số điện thoại
                if (userPhone != null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.PHONE_IS_USED;
                    return response;
                }
                // Kiểm tra email
                var userEmail = await _context.Employee.Where(x => !x.DelFlag && x.Email == accountInfo.Email).FirstOrDefaultAsync();
                if (userEmail != null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.EMAIL_IS_USED;
                    return response;
                }
                // Tạo employeeCode
                var maxEmployeeCode = await _context.Employee.Where(x => !x.DelFlag).Select(x => x.EmployeeCode).MaxAsync();
                // Lấy số từ EmployeeCode hiện tại bằng cách loại bỏ ký tự đầu tiên (phần "NV") và chuyển đổi thành số nguyên
                var currentNumber = int.Parse(maxEmployeeCode.Substring(2));

                // Tạo mã EmployeeCode mới bằng cách tăng số lên 1 và định dạng lại chuỗi
                var newEmployeeCode = "NV" + (currentNumber + 1).ToString("D4");

                //Kiểm tra username
                var userName = await _context.Users.Where(x => !x.DelFlag && x.Username == accountInfo.Username).FirstOrDefaultAsync();
                if (userEmail != null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.USERNAME_IS_USED;
                    return response;
                }

                if (!String.IsNullOrEmpty(accountInfo.Gender))
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
                    Gender = accountInfo.Gender,
                    CommuneId = accountInfo.CommuneId,
                    DistrictId = accountInfo.DistrictId,
                    ProvinceId = accountInfo.ProvinceId
                };

                //Thêm user vào database
                _context.Users.Add(userData);
                transaction = await _context.Database.BeginTransactionAsync();
                await _context.SaveChangesAsync();
                //Thêm userRoles vào database
                //Thêm quyền cho tài khoản
                UserRoleData userRole = new UserRoleData
                {
                    RoleId = accountInfo.Role,
                    UserId = userData.Id
                };
                _context.UserRoles.Add(userRole);
                // Thêm nhân viên
                EmployeeDb employeeDb = new EmployeeDb()
                {
                    Email = accountInfo.Email,
                    Phone = phone,
                    EmployeeCode = newEmployeeCode,
                    UserId = userData.Id,
                    PositionId = accountInfo.PositionId,
                    DateStart = accountInfo.DateStart.Value,
                    BranchId = accountInfo.BranchId
                };
                _context.Employee.Add(employeeDb);
                await _context.SaveChangesAsync();
                userData.EmployeeId = employeeDb.Id;
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                //Thêm token
                await _loginModel.GetTokenLogin(userData.Id.ToString());
                response.Data.Add("EmployeeId", employeeDb.Id.ToString()); ;
                response.Data.Add("EmployeeCode", employeeDb.EmployeeCode); ;
                return response;
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                throw e;
            }

        }

        public async Task<ResponseInfo> UpdateAccount(AccountInfo accountInfo)
        {

            IDbContextTransaction transaction = null;
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return response;
                }
                // Lấy thông tin tài khoản đang login
                var userLogin = await _usersModel.GetUserIsLogin();
                var user = await _context.Users.Where(x => !x.DelFlag && x.Username == userLogin.Username).FirstOrDefaultAsync();
                // Kiểm tra nhưng thông tin thay đổi
                string phone = Security.Base64Encode(accountInfo.Phone);

                // Kiểm tra số điện thoại
                if (accountInfo.Phone != userLogin.Phone)
                {
                    var userPhone = await _context.Users.Where(x => !x.DelFlag && x.Phone == phone && x.Id == userLogin.Id).FirstOrDefaultAsync();
                    // Kiểm tra số điện thoại
                    if (userPhone != null)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.PHONE_IS_USED;
                        return response;
                    }
                }
                // Kiểm tra tên đăng nhập

                //Kiểm tra username
                if (accountInfo.Username != userLogin.Username)
                {
                    //Kiểm tra username tồn tại chưa
                    var userName = await _context.Users.Where(x => !x.DelFlag && x.Username == accountInfo.Username && x.Id == userLogin.Id).FirstOrDefaultAsync();
                    if (userName != null)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.USERNAME_IS_USED;
                        return response;
                    }
                    //Kiểm tra tên đăng nhập
                    if (accountInfo.Username.Length < 8)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.LENGTH_USERNAME_NOT_OK;
                        return response;
                    }
                }
                // Kiểm tra giới tính
                if (accountInfo.Gender != userLogin.Gender)
                {
                    if (!String.IsNullOrEmpty(accountInfo.Gender))
                    {
                        if (accountInfo.Gender != "M" && accountInfo.Gender != "O" && accountInfo.Gender != "F")
                        {
                            response.Code = CodeResponse.HAVE_ERROR;
                            response.MsgNo = MSG_NO.GENDER_NOT_EXIST;
                            return response;
                        }
                    }
                }
                //string firstSecurityString = Helpers.RenderToken("", 10);
                //string lastSecurityString = Helpers.RenderToken("", 20).Substring(10, 10);

                // Cập nhật tài khoản
                user.Username = accountInfo.Username;
                //user.FirstSecurityString = firstSecurityString;
                //user.LastSecurityString = lastSecurityString;
                user.Email = accountInfo.Email;
                user.Name = accountInfo.Name;
                user.DateOfBirth = accountInfo.DateOfBirth;
                user.Phone = phone;
                user.Gender = accountInfo.Gender;
                user.Address = accountInfo.Address;
                user.DistrictId = accountInfo.DistrictId;
                user.CommuneId = accountInfo.CommuneId;
                user.ProvinceId = accountInfo.ProvinceId;

                //Xóa các token cũ
                UserTokenData userToken = await _context.UserTokens.FirstOrDefaultAsync(x => !x.DelFlag && x.UserId == userLogin.Id); // && x.Timeout != null && x.Timeout > now);
                if (userToken != null)
                {
                    userToken.DelFlag = true;
                }
                await _context.SaveChangesAsync();
                transaction = await _context.Database.BeginTransactionAsync();
                await transaction.CommitAsync();
                //Thêm token
                response.Data.Add("Token", await _loginModel.GetTokenLogin(userLogin.Id.ToString()));
                response.Data.Add("Username", userLogin.Name);
                return response;
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                throw e;
            }
        }
        public async Task<ResponseInfo> UpdateAccountEmployee(EmployeeData employeeData)
        {

            IDbContextTransaction transaction = null;
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return response;
                }
                var user = await _context.Users.Where(x => !x.DelFlag && x.EmployeeId == employeeData.Id).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.EMPLOYEE_NOT_EXITED;
                    return response;
                }
                var employee = await _context.Employee.Where(x => !x.DelFlag && x.Id == user.EmployeeId).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.EMPLOYEE_NOT_EXITED;
                    return response;
                }
                // Kiểm tra nhưng thông tin thay đổi
                string phone = Security.Base64Encode(employeeData.Phone);

                // Kiểm tra số điện thoại
                if (employeeData.Phone != user.Phone)
                {
                    var employeePhone = await _context.Employee.Where(x => !x.DelFlag && x.Phone == phone && x.Id != employeeData.Id).FirstOrDefaultAsync();
                    // Kiểm tra số điện thoại
                    if (employeePhone != null)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.PHONE_IS_USED;
                        return response;
                    }
                }
                // Kiểm tra tên đăng nhập

                //Kiểm tra username
                if (employeeData.Username != user.Username)
                {
                    //Kiểm tra username tồn tại chưa
                    var userName = await _context.Users.Where(x => !x.DelFlag && x.Username == employeeData.Username && x.Id != employeeData.Id).FirstOrDefaultAsync();
                    if (userName != null)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.USERNAME_IS_USED;
                        return response;
                    }
                    //Kiểm tra tên đăng nhập
                    if (employeeData.Username.Length < 8)
                    {
                        response.Code = CodeResponse.HAVE_ERROR;
                        response.MsgNo = MSG_NO.LENGTH_USERNAME_NOT_OK;
                        return response;
                    }
                }
                // Kiểm tra giới tính
                if (employeeData.Gender != user.Gender)
                {
                    if (!String.IsNullOrEmpty(employeeData.Gender))
                    {
                        if (employeeData.Gender != "M" && employeeData.Gender != "O" && employeeData.Gender != "F")
                        {
                            response.Code = CodeResponse.HAVE_ERROR;
                            response.MsgNo = MSG_NO.GENDER_NOT_EXIST;
                            return response;
                        }
                    }
                }
                //string firstSecurityString = Helpers.RenderToken("", 10);
                //string lastSecurityString = Helpers.RenderToken("", 20).Substring(10, 10);

                // Cập nhật tài khoản
                user.Username = employeeData.Username;
                user.Email = employeeData.Email;
                user.Name = employeeData.Name;
                user.DateOfBirth = employeeData.DateOfBirth;
                user.Phone = phone;
                user.Gender = employeeData.Gender;
                user.Address = employeeData.Address;
                user.DistrictId = employeeData.DistrictId;
                user.CommuneId = employeeData.CommuneId;
                user.ProvinceId = employeeData.ProvinceId;

                employee.Phone = phone;
                employee.DateStart = employeeData.DateStart.Value;
                employee.PositionId = employeeData.PositionId;
                employee.BranchId = employeeData.BranchId;
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                throw e;
            }
        }
        public async Task<ResponseInfo> DeleteAccountEmployee(int id)
        {
            IDbContextTransaction transaction = null;
            string method = GetActualAsyncMethodName();
            ResponseInfo response = new ResponseInfo();
            try
            {
                if (!await _indentityService.CheckIndentifyUser(R001.ADMIN.CODE))
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.ACCOUNT_NOT_HAVE_PERMISSION;
                    return response;
                }
                var user = await _context.Users.Where(x => !x.DelFlag && x.EmployeeId == id).FirstOrDefaultAsync();
                if (user == null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.EMPLOYEE_NOT_EXITED;
                    return response;
                }
                var employee = await _context.Employee.Where(x => !x.DelFlag && x.Id == user.EmployeeId).FirstOrDefaultAsync();
                if (employee == null)
                {
                    response.Code = CodeResponse.HAVE_ERROR;
                    response.MsgNo = MSG_NO.EMPLOYEE_NOT_EXITED;
                    return response;
                }
                user.DelFlag = true;
                employee.DelFlag = true;
                await _context.SaveChangesAsync();
                return response;
            }
            catch (Exception e)
            {
                await _context.RollBack(transaction);
                throw e;
            }
        }
    }
}
