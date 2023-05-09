using System;
using CapstoneProject.Areas.Employee.Models.Schemas;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Users.Controllers
{
    [Area("Users")]
    public class UsersAreaController : BaseController
    {
        public UsersAreaController(IServiceProvider provider) : base(provider)
        {

        }
    }
}

