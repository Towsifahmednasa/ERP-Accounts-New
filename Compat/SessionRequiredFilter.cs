// Global authentication guard. Legacy only checked Session["LoginUserId"] on some page actions (30 of 39
// controllers, and almost never on JSON/partial actions - security finding S4). Here every action requires a
// logged-in session unless marked [AllowAnonymous] (login/logout/error). Page requests redirect to the login
// page exactly as the legacy guard did; AJAX requests get 401 JSON instead of a redirected HTML page.
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace New_Accounts.Compat
{
    public class SessionRequiredFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any()) return;
            if (context.HttpContext.Session.GetString("loginuserid") != null) return;

            var req = context.HttpContext.Request;
            bool ajax = req.Headers["X-Requested-With"] == "XMLHttpRequest"
                        || req.Headers["Accept"].ToString().Contains("application/json");
            if (ajax)
            {
                context.Result = new JsonResult(new { isSuccess = false, msg = "Session expired. Please sign in again.", sessionExpired = true })
                { StatusCode = StatusCodes.Status401Unauthorized };
            }
            else
            {
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
        }
    }
}
