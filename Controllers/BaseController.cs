using System;
using System.Net;
//using FMStyle.ApiCommons.Filters;
//using FMStyle.ApiCommons.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace CapstoneProject.Controllers
{
    //[ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
    [ProducesResponseType((int)HttpStatusCode.NotAcceptable)]
    // [CheckRequestId(Order = 1)]

    public class BaseController : ControllerBase
    {
        //protected readonly ILogService _logService;
        protected readonly ILogger<BaseController> _logger;

        public BaseController()
        {
        }

        public BaseController(
            IServiceProvider provider
        )
        {
            //ILogService logService = provider.GetService<ILogService>();
            //_logService = logService ?? throw new ArgumentNullException(nameof(logService));
        }
    }
}
