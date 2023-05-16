using System;
using Autofac;
using CapstoneProject.Areas.Film.Models.FilmModels;
using CapstoneProject.Areas.Film.Models.FilmAdminModels;

namespace CapstoneProject.AutofacModules
{
    /// <summary>
    /// Đăng ký các dịch vụ dùng cho việc tạo và xử lý sự kiện thông qua autofac
    /// <para>Created at: </para>
    /// <para>Created by: VP</para>
    /// </summary>
    public class FilmModule : Autofac.Module
    {
        /// <summary>
        /// Ghi đè phương thức load của autofac để đăng ký dịch vụ
        /// </summary>
        /// <param name="builder">builder dùng để đăng ký dịch vụ</param>
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FilmModel>().As<IFilmModel>().InstancePerLifetimeScope();
            builder.RegisterType<FilmAdminModel>().As<IFilmAdminModels>().InstancePerLifetimeScope();
        }
    }
}

