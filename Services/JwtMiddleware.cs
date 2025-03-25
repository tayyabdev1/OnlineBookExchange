using System;
using System.Collections.Generic;
using System.Linq;
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

            var request = filterContext.HttpContext.Request;

            if (request.Cookies["jwt"] == null)
            {
                filterContext.Result = new HttpStatusCodeResult(401, "Unauthorized");
                return;
            }

            var token = request.Cookies["jwt"].Value;
            var userName = Authentication.ValidateToken(token);

            if (userName == null)
            {
                filterContext.Result = new HttpStatusCodeResult(401, "Invalid token");
                return;
            }

            base.OnActionExecuting(filterContext);
        }
    }
}