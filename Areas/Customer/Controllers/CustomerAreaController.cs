using System;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Customer.Controllers
{
    [Auth]
    [Area("Customer")]
    public class CustomerAreaController : BaseController
    {
        public CustomerAreaController(IServiceProvider provider) : base(provider)
        {
		}
	}
}

