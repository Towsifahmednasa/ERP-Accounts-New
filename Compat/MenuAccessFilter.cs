// Server-side page authorization by the DB menu (legacy intent: AccessAuthoraization.CheckAccess ->
// sp_CheckMenuAccess, never invoked). Opt-in via Security:EnforceMenuAccess because the legacy never enforced it
// and sp_CheckMenuAccess is not present in the local databases. When enabled, HTML page requests (not AJAX)
// are allowed only if the controller is in the user's menu tree or the user is "admin" (legacy rule).
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using New_Accounts.Models;

namespace New_Accounts.Compat
{
    public class MenuAccessFilter : IAuthorizationFilter
    {
        private readonly bool _enabled;
        private static readonly HashSet<string> Exempt = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            { "Home", "Dashboard", "Utility", "CrystalViewer" };

        public MenuAccessFilter(IConfiguration config) { _enabled = config.GetValue<bool>("Security:EnforceMenuAccess"); }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_enabled) return;
            if (context.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any()) return;
            if (!(context.ActionDescriptor is ControllerActionDescriptor cad)) return;
            if (Exempt.Contains(cad.ControllerName)) return;
            var req = context.HttpContext.Request;
            if (req.Headers["X-Requested-With"] == "XMLHttpRequest") return; // data calls are page-scoped
            if (!req.Headers["Accept"].ToString().Contains("text/html")) return;

            var session = new LegacySession(context.HttpContext.Session);
            if (string.Equals(session["username"] as string, "admin", StringComparison.OrdinalIgnoreCase)) return;
            var menu = session.Get<List<MenuStepOne>>("MenuList") ?? new List<MenuStepOne>();
            if (MenuMap.Leaves(menu).Any(l => l.Controller.Equals(cad.ControllerName, StringComparison.OrdinalIgnoreCase))) return;
            context.Result = new ViewResult { ViewName = "AccessDenied", StatusCode = 403 };
        }
    }
}
