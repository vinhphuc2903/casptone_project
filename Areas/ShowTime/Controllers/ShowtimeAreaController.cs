using System;
using CapstoneProject.Areas.Employee.Models.Schemas;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;
namespace CapstoneProject.Areas.ShowTime.Controllers
{
    [Area("showtime")]
    public class ShowtimeAreaController : BaseController
    {
        public ShowtimeAreaController(IServiceProvider provider) : base(provider)
        {
		}
	}
}

