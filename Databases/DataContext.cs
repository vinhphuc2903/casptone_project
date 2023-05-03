using System;
using EnvDTE;
using Microsoft.EntityFrameworkCore;
using CapstoneProject.Databases.Schemas.System.Employee;
using System.Data;
using System.Data.Common;
using System.Security.AccessControl;
using System.Security.Claims;

namespace CapstoneProject.Databases
{
    public class DataContext : DbContext
    {

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


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employees>()
                .HasKey(e => e.Id);
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
    }
}

