using System;
using CapstoneProject.Areas.Employee.Models.Schemas;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Employee.Controllers
{
    [Auth]
    [Area("Employees")]
    public class EmployeesAreaController : BaseController
    {
        public EmployeesAreaController(IServiceProvider provider) : base(provider)
        {

        }
    }
}

