// Shim for the eight legacy DAL files that read HttpContext.Current.Session[...] (System.Web) for the audit user.
// Backed by IHttpContextAccessor; same keys, same values (see LegacySession).
using Microsoft.AspNetCore.Http;

namespace New_Accounts.Compat
{
    public static class HttpContext
    {
        internal static IHttpContextAccessor Accessor { get; set; }

        public static CurrentContext Current => Accessor?.HttpContext == null ? null : new CurrentContext(Accessor.HttpContext);
    }

    public class CurrentContext
    {
        private readonly Microsoft.AspNetCore.Http.HttpContext _ctx;
        public CurrentContext(Microsoft.AspNetCore.Http.HttpContext ctx) { _ctx = ctx; }
        public LegacySession Session => new LegacySession(_ctx.Session);
    }
}
