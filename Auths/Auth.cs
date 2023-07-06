using System;
using CapstoneProject.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace CapstoneProject.Auths
{
    /// <summary>
    /// Filter dùng để xác thực quyền truy caapf của user đối ới api.
    /// <para>Author: VinhPhuc</para>
    /// <para>Created: 08/06/2023</para>
    /// </summary>
    public class Auth : AuthorizeAttribute, IAuthorizationFilter
    {
        /// <summary>
        /// Ghi đè phương thức dùng để lọc request.
        /// <para>Author: VinhPhuc</para>
        /// <para>Created: 08/08/2023</para>
        /// </summary>
        /// <param name="filterContext">Data của 1 request.</param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                //var user = context.HttpContext.User;

                //if (!user.Identity.IsAuthenticated)
                //{
                //    context.Result = new UnauthorizedResult();
                //    return;
                //}
                //else
                //{
                IIdentityService _identityService = context.HttpContext.RequestServices.GetService<IIdentityService>();
                if (_identityService == null)
                {
                    context.Result = new BadRequestObjectResult("Service identity is requried");
                    return;
                }
                else
                {
                    if (_identityService.GetUserId() == "0")
                    {
                        context.Result = new UnauthorizedResult();
                        return;
                    }
                }
                //}
            }
            catch (Exception e)
            {
                context.Result = new BadRequestObjectResult(e.Message);
                return;
            }
        }
    }
}
