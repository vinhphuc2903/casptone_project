using System;
using Autofac;
using CapstoneProject.Areas.Users.Models;
using CapstoneProject.Areas.Users.Models.LoginModel;
using CapstoneProject.Areas.Users.Models.UserModel;

namespace CapstoneProject.AutofacModules
{
    /// <summary>
    /// Đăng ký các dịch vụ dùng cho việc tạo và xử lý sự kiện thông qua autofac
    /// <para>Created at: </para>
    /// <para>Created by: VP</para>
    /// </summary>
    public class UserModule : Autofac.Module
    {
        /// <summary>
        /// Ghi đè phương thức load của autofac để đăng ký dịch vụ
        /// </summary>
        /// <param name="builder">builder dùng để đăng ký dịch vụ</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserModel>().As<IUsersModel>().InstancePerLifetimeScope();
            builder.RegisterType<LoginModel>().As<ILoginModel>().InstancePerLifetimeScope();
        }
    }
}

