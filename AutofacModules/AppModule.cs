using System;
using Autofac;

namespace CapstoneProject.AutofacModules
{
    /// <summary>
    /// Đăng ký các dịch vụ dùng cho việc tạo và xử lý sự kiện thông qua autofac
    /// <para>Created at: 10/07/2023</para>
    /// <para>Created by: VinhPhuc</para>
    /// </summary>
    public class AppModule : Autofac.Module
    {
        /// <summary>
        /// Ghi đè phương thức load của autofac để đăng ký dịch vụ
        /// <para>Created at: 10/07/2023</para>
        /// <para>Created by: VinhPhuc</para>
        /// </summary>
        /// <param name="builder">builder dùng để đăng ký dịch vụ</param>
        protected override void Load(ContainerBuilder builder)
        {
            //builder.RegisterType<LoginModel>().As<ILoginModel>().InstancePerLifetimeScope();
            //builder.RegisterType<AppStateModel>().As<IAppStateModel>().InstancePerLifetimeScope();
        }
    }
}

