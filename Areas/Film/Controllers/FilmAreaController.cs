using System;
using CapstoneProject.Areas.Employee.Models.Schemas;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Film.Controllers
{
    [Area("Films")]
    public class FilmAreaController : BaseController
    {
		public FilmAreaController(IServiceProvider provider) : base(provider)
        {
		}
	}
}

