using System;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.CinemeRoom.Controllers
{
    [Area("CinemaRoom")]
    public class CinemeRoomAreaController : BaseController
    {
		public CinemeRoomAreaController(IServiceProvider provider) : base(provider)
        {
		}
	}
}

