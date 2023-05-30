
using System;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Orders.Controllers
{
    [Area("Order")]
    public class OrderAreaController : BaseController
    {
        public OrderAreaController(IServiceProvider provider) : base(provider)
		{
		}
	}
}

