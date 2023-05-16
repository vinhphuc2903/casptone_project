using System;
using EnvDTE;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Databases.Schemas.System.Employee;
using CapstoneProject.Databases.Schemas.System.Users;
using CapstoneProject.Databases.Schemas.System.Food;
using CapstoneProject.Databases.Schemas.System.Orders;
using CapstoneProject.Databases.Schemas.System.Ticket;
using CapstoneProject.Databases.Schemas.System.Film;
using CapstoneProject.Databases.Schemas.System.CinemaRoom;
using System.Data;
using System.Data.Common;
using System.Security.AccessControl;
using System.Security.Claims;
using CapstoneProject.Databases.Schemas.Setting;
using Microsoft.EntityFrameworkCore.Storage;
using CapstoneProject.Commons;
using System.Net.Sockets;
using Dapper;
using Microsoft.Identity.Client;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using System.Web;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using System.Net.Http;

namespace CapstoneProject.Databases
{
    public class DataContext : DbContext
    {
        //private readonly IHttpContextAccessor _httpContext;

        private readonly IHttpContextAccessor _context;

        /// Danh sach Table

        /// <summary>
        /// Table Employees (Employees)
        /// </summary>
        public virtual DbSet<Employees> Employee { set; get; }
        /// <summary>
        /// Table SYSUSRTOK (UserTokens)
        /// </summary>
        public virtual DbSet<UserToken> UserTokens { get; set; }
        /// <summary>
        /// Table Users (User)
        /// </summary>
        public virtual DbSet<User> Users { get; set; }
        /// <summary>
        /// Table UserRole (UserRoles)
        /// </summary>
        public virtual DbSet<UserRole> UserRoles { get; set; }
        /// <summary>
        /// Table Role (Roles)
        /// </summary>
        public virtual DbSet<Role> Roles { get; set; }
        /// <summary>
        /// Table UserPoint (UserPoint)
        /// </summary>
        public virtual DbSet<UserPoint> UserPoint { get; set; }
        /// <summary>
        /// Table Size
        /// </summary>
        /// <value></value>
        public virtual DbSet<Size> Size { get; set; }
        /// <summary>
        /// Table Order Food Detail
        /// </summary>
        /// <value></value>
        public virtual DbSet<OrderFoodDetail> OrderFoodDetail { get; set; }
        /// <summary>
        /// Table Order Ticket Detail
        /// </summary>
        /// <value></value>
        public virtual DbSet<OrderTicketDetail> OrderTicketDetail { get; set; }
        /// <summary>
        /// Table Order
        /// </summary>
        /// <value></value>
        public virtual DbSet<Orders> Orders { get; set; }
        /// <summary>
        /// Table Show time
        /// </summary>
        /// <value></value>
        public virtual DbSet<ShowTime> ShowTime { get; set; }
        /// <summary>
        /// Table Ticket
        /// </summary>
        /// <value></value>
        public virtual DbSet<Tickets> Ticket { get; set; }
        /// <summary>
        /// Table Films
        /// </summary>
        public virtual DbSet<Films> Films { get; set; }
        /// <summary>
        /// Table TypeFilm
        /// </summary>
        public virtual DbSet<TypeFilm> TypeFilms { get; set; }
        /// <summary>
        /// Table Seats
        /// </summary>
        public virtual DbSet<Seat> Seats { get; set; }
        /// <summary>
        /// Table Cinemarooms
        /// </summary>
        public virtual DbSet<CinemaRooms> CinemaRooms { get; set; }
        /// <summary>
        /// Table Foods
        /// </summary>
        public virtual DbSet<Foods> Foods { get; set; }
        /// <summary>
        /// Table Foods
        /// </summary>
        public virtual DbSet<TypeFilmDetail> TypeFilmDetails { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Setting key
            modelBuilder.Entity<Employees>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<User>()
                .HasKey(e => e.Id);
            
            modelBuilder.Entity<UserToken>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Role>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<CinemaRooms>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Seat>()
                .HasKey(e => e.Id);
            
            modelBuilder.Entity<TypeFilm>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Films>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Tickets>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<ShowTime>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Orders>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<OrderTicketDetail>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<OrderFoodDetail>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Size>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<Foods>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<UserPoint>()
                .HasKey(e => e.Id);

            modelBuilder.Entity<UserRole>()
                .HasKey(e => new
                {
                    e.UserId,
                    e.RoleId
                });

            modelBuilder.Entity<TypeFilmDetail>()
                .HasKey(e => new
                {
                    e.FilmId,
                    e.TypeFilmId
                });

            // Setting relationship

            //Employee
            modelBuilder.Entity<Employees>()
                .HasOne(e => e.User)
                .WithOne(e => e.Employees)
                .HasForeignKey<User>(c => c.EmployeeId);

            //User
            modelBuilder.Entity<User>()
                .HasOne(e => e.Employees)
                .WithOne(e => e.User)
                .HasForeignKey<Employees>(c => c.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Tokens)
                .WithOne(t => t.User)
                .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<User>()
               .HasMany(u => u.Roles)
               .WithOne(t => t.User)
               .HasForeignKey(t => t.UserId);

            modelBuilder.Entity<User>()
               .HasOne(e => e.UserPoint)
               .WithOne(e => e.User)
               .HasForeignKey<UserPoint>(c => c.UserId);

            //Role
            modelBuilder.Entity<Role>()
               .HasMany(u => u.Users)
               .WithOne(t => t.Role)
               .HasForeignKey(t => t.RoleId);

            //UserPoint
            modelBuilder.Entity<UserPoint>()
                .HasOne(e => e.User)
                .WithOne(e => e.UserPoint)
                .HasForeignKey<User>(c => c.UserPointId);

            //CinemaRooms
            modelBuilder.Entity<CinemaRooms>()
                .HasMany(u => u.Seats)
                .WithOne(t => t.CinemaRoom)
                .HasForeignKey(t => t.CinemaRoomId);

            //Seat
            modelBuilder.Entity<Seat>()
                .HasOne(u => u.CinemaRoom)
                .WithMany(t => t.Seats)
                .HasForeignKey(t => t.CinemaRoomId);

            //TypeFilmDetail
            modelBuilder.Entity<TypeFilmDetail>()
                .HasOne(u => u.Films)
                .WithMany(t => t.TypeFilmDetail)
                .HasForeignKey(t => t.FilmId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<TypeFilmDetail>()
                .HasOne(u => u.TypeFilms)
                .WithMany(t => t.TypeFilmDetail)
                .HasForeignKey(t => t.TypeFilmId)
                .OnDelete(DeleteBehavior.NoAction);

            //Foods
            modelBuilder.Entity<Foods>()
                .HasOne(u => u.Size)
                .WithMany(t => t.Foods)
                .HasForeignKey(t => t.SizeId);

            //OrderFoodDetail
            modelBuilder.Entity<OrderFoodDetail>()
                .HasOne(u => u.Orders)
                .WithMany(t => t.OrderFoodDetails)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderFoodDetail>()
                .HasOne(u => u.Foods)
                .WithMany(t => t.OrderFoodDetails)
                .HasForeignKey(t => t.FoodId)
                .OnDelete(DeleteBehavior.NoAction);

            //OrderTicketDetail
            modelBuilder.Entity<OrderTicketDetail>()
                .HasOne(u => u.Orders)
                .WithMany(t => t.OrderTicketDetails)
                .HasForeignKey(t => t.OrderId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<OrderTicketDetail>()
                .HasOne(u => u.Ticket)
                .WithMany(t => t.OrderTicketDetail)
                .HasForeignKey(t => t.TicketId)
                .OnDelete(DeleteBehavior.NoAction);

            //Show time
            modelBuilder.Entity<ShowTime>()
                .HasOne(u => u.Film)
                .WithMany(t => t.ShowTime)
                .HasForeignKey(u => u.FilmId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ShowTime>()
                .HasOne(u => u.CinemaRooms)
                .WithMany(t => t.Showtimes)
                .HasForeignKey(u => u.CinemaRoomId)
                .OnDelete(DeleteBehavior.NoAction);

            //Ticket
            //modelBuilder.Entity<Tickets>()
            //    .HasOne(u => u.Film)
            //    .WithMany(u => u.Ticket)
            //    .HasForeignKey(u => u.FilmId);

            modelBuilder.Entity<Tickets>()
                .HasOne(u => u.Seat)
                .WithMany(u => u.Ticket)
                .HasForeignKey(u => u.SeatId)
                .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<Tickets>()
            //    .HasOne(u => u.ShowTime)
            //    .WithMany(u => u.Tickets)
            //    .HasForeignKey(u => u.ShowtimeId)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        public DbConnection GetConnection()
        {
            DbConnection _connection = Database.GetDbConnection();
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            return _connection;
        }

        /// <param name="options">Các cài đặt ban đầu</param>
        public DataContext(DbContextOptions<DataContext> options, IHttpContextAccessor context) : base(options)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task RollBack(IDbContextTransaction transaction = null)
        {
            try
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync();
                }
            }
            catch { }
        }

        /// <summary>
        /// Ghi đè phương thức lưu vào DB để lưu thêm các dữ liệu mặc định cần thiết
        /// <para>Created at: 10/07/2023</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>
        /// Số lượng record ảnh hưởng
        /// </returns>       
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            try
            {
                OnBeforeSaving();
                return base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Lưu thêm các thông tin cần thiết mặc định khi cập nhật dữ diệu vào Database.
        /// <para>Created at: 10/07/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        private void OnBeforeSaving()
        {
            try
            {
                // Nếu có sự thay đổi dữ liệu
                if (ChangeTracker.HasChanges())
                {
                    // Láy các thông tin cơ bản từ hệ thống
                    DateTimeOffset now = DateTimeOffset.Now;
                    int accountId = Convert.ToInt32(GetAccountId());
                    string ip = GetRequestIp();
                    // Duyệt qua hết tất cả dối tượng có thay đổi
                    foreach (var entry in ChangeTracker.Entries())
                    {
                        try
                        {
                            if (entry.Entity is ITable root)
                            {
                                switch (entry.State)
                                {
                                    // Nếu là thêm mới thì cập nhật thông tin thêm mới
                                    case EntityState.Added:
                                        {

                                            root.CreatedAt = root.CreatedAt.ToString("yyyy") == "0001" ? now : root.CreatedAt;
                                            root.CreatedBy = root.CreatedBy > 0 ? root.CreatedBy : accountId;
                                            root.CreatedIp = ip;
                                            root.UpdatedAt = now;
                                            root.UpdatedBy = root.CreatedBy > 0 ? root.CreatedBy : accountId;
                                            root.UpdatedIp = ip;
                                            root.DelFlag = false;
                                            break;
                                        }
                                    // Nếu là update thì cập nhật các trường liên quan đến update
                                    case EntityState.Modified:
                                        {
                                            root.UpdatedAt = now;
                                            root.UpdatedBy = root.UpdatedBy != null && root.UpdatedBy > 0 ? root.UpdatedBy : accountId;
                                            root.UpdatedIp = ip;
                                            break;
                                        }
                                }
                            }
                        }
                        catch { }
                    }
                    //AuditLogs.AddRange(
                    //    AuditTrail.GetLogEntries(
                    //        ChangeTracker.Entries(),
                    //        accountId,
                    //        new string[] {
                    //            "User",
                    //            "UserAddress",
                    //            "UserInfo"
                    //        }
                    //    )
                    //);
                }
            }
            catch (DbUpdateException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Lấy user id đang đăng nhập nếu có
        /// <para>Created at: 08/08/2020</para>
        /// <para>Created by: QuyPN</para>
        /// </summary>
        /// <returns>user id của user đang đăng nhập. Trả về 0 nếu không có thông tin user đăng nhập</returns>
        public string GetAccountId()
        {
            try
            {
                string accountId = "0";
                //ClaimsPrincipal user = null;
                //if (_context != null && _context.HttpContext != null)
                //{
                //    user = _context.HttpContext.User;
                //}
                //if (user != null && user.Identity != null && user.Identity.IsAuthenticated)
                //{
                //    var identity = user.Identity as ClaimsIdentity;
                //    accountId = identity.Claims.Where(p => p.Type == "UserId").FirstOrDefault()?.Value;
                //}
                string accessToken = _context.HttpContext.Request.Headers["Authorization"];
                if (!String.IsNullOrEmpty(accessToken))
                {
                    accessToken = accessToken.Replace("Bearer", "").Trim();
                    DbConnection _connection = GetConnection();
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
                        accountId = "0";
                    } else
                    {
                        accountId = userToken.UserId.ToString();
                    }
                }
                else
                {
                    accountId = "0";
                }
                return accountId;
            }
            catch (Exception e)
            {
                return "0";
            }
        }

        /// <summary>
        /// Lấy Ip của request hiện tại
        /// <para>Created at: 15/05/2020</para>
        /// <para>Created by: DungNT</para>
        /// </summary>
        /// <returns>Ip của request hiện tại</returns>
        public string GetRequestIp()
        {
            try
            {
                string ip = _context?.HttpContext == null ? "::1" : _context.HttpContext.Request.Headers["X-Real-IP"].ToString();
                ip = String.IsNullOrEmpty(ip) ? _context.HttpContext.Request.Headers["x-request-ip"].ToString() : ip;
                ip = String.IsNullOrEmpty(ip) ? _context.HttpContext.Request.Headers["phone_ip"].ToString() : ip;
                ip = String.IsNullOrEmpty(ip) ? _context?.HttpContext.Connection.RemoteIpAddress.ToString() : ip;
                return ip;
            }
            catch
            {
                return "::1";
            }
        }

    }
}

