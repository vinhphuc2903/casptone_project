using System;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneProject.Controllers;
using CapstoneProject.Auths;

namespace CapstoneProject.Areas.Report.Controllers
{
    [Area("Report")]
    [Auth]
    public class ReportAreaController : BaseController
    {
        public ReportAreaController(IServiceProvider provider) : base(provider)
        {
        }
    }
}

