// Base controller: exposes the legacy "Session[...]" indexer so the ported MVC 5 controllers compile unchanged.
using Microsoft.AspNetCore.Mvc;

namespace New_Accounts.Compat
{
    public abstract class LegacyController : Controller
    {
        private LegacySession _session;
        public LegacySession Session => _session ??= new LegacySession(HttpContext.Session);
    }
}
