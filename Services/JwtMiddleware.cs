using Microsoft.IdentityModel.JsonWebTokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace OnlineBookExchange.Services
{
    public class JwtAuthenticationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            bool allowAnonymous = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true)
                || filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), inherit: true);

            if (allowAnonymous)
            {
                return;
            }

            var token = filterContext.HttpContext.Request.Cookies["jwt"]?.Value;

            if (string.IsNullOrEmpty(token))
            {
                filterContext.Result = new HttpStatusCodeResult(401, "Unauthorized");
                return;
            }

            var principal = Authentication.ValidateToken(token);

            if (principal == null)
            {
                filterContext.Result = new HttpStatusCodeResult(401, "Invalid token");
                return;
            }

            filterContext.HttpContext.User = principal;
             base.OnActionExecuting(filterContext);
        }
    }
}