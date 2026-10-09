// ASP.NET MVC 5 bound "contentType: application/json" bodies into ordinary action parameters through
// JsonValueProviderFactory (the JSON object was flattened to "Prop", "List[0].Prop" keys). ASP.NET Core only
// binds JSON with [FromBody]. Rather than touch 39 controllers and 750 AJAX calls, this middleware flattens the
// JSON body into Request.Form exactly the way JsonValueProviderFactory did, so model binding is unchanged.
// Legacy quirk preserved: many pages send contentType json with a form-encoded body (jQuery data:{...} without
// JSON.stringify); a body that is not JSON is parsed as an ordinary form body.
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace New_Accounts.Compat
{
    public class LegacyJsonBodyMiddleware
    {
        private readonly RequestDelegate _next;
        public LegacyJsonBodyMiddleware(RequestDelegate next) { _next = next; }

        public async Task Invoke(Microsoft.AspNetCore.Http.HttpContext context)
        {
            var req = context.Request;
            if (req.ContentType != null && req.ContentType.StartsWith("application/json", StringComparison.OrdinalIgnoreCase)
                && (req.ContentLength ?? 1) != 0)
            {
                req.EnableBuffering();
                string body;
                using (var reader = new StreamReader(req.Body, System.Text.Encoding.UTF8, false, 1024, leaveOpen: true))
                {
                    body = await reader.ReadToEndAsync();
                }
                req.Body.Position = 0;

                var form = new Dictionary<string, StringValues>(StringComparer.OrdinalIgnoreCase);
                bool isJson = false;
                string trimmed = body.TrimStart();
                if (trimmed.StartsWith("{") || trimmed.StartsWith("["))
                {
                    try { Flatten(JToken.Parse(body), "", form); isJson = true; }
                    catch { isJson = false; }
                }
                if (!isJson)
                {
                    foreach (var pair in Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(body))
                        form[pair.Key] = pair.Value;
                }
                req.ContentType = "application/x-www-form-urlencoded";
                req.Form = new FormCollection(form);
            }
            await _next(context);
        }

        private static void Flatten(JToken token, string prefix, Dictionary<string, StringValues> form)
        {
            switch (token)
            {
                case JObject obj:
                    foreach (var prop in obj.Properties())
                        Flatten(prop.Value, prefix.Length == 0 ? prop.Name : prefix + "." + prop.Name, form);
                    break;
                case JArray arr:
                    for (int i = 0; i < arr.Count; i++)
                        Flatten(arr[i], prefix + "[" + i + "]", form);
                    break;
                case JValue val:
                    if (val.Type == JTokenType.Null) return;              // JsonValueProvider: null -> not bound
                    string s = val.Type == JTokenType.Boolean ? ((bool)val ? "true" : "false")
                             : val.Type == JTokenType.Date ? ((DateTime)val).ToString("o")
                             : val.ToString(System.Globalization.CultureInfo.InvariantCulture);
                    form[prefix] = s;
                    break;
            }
        }
    }
}
