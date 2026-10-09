using System;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using New_Accounts.Compat;
using New_Accounts.DataManager;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Legacy IIS site ran under the server culture (en-US on the reference machine): date strings such as
// "18 August, 2026" posted by the views and DateTime.ToString("dd MMMM, yyyy") in views depend on it.
var culture = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = culture;
CultureInfo.DefaultThreadCurrentUICulture = culture;

// Legacy DataManager/DataBase.cs + SqlUserAccess.cs were compile-time constants; same values now come from configuration.
SqlUserAccess.Template = builder.Configuration.GetConnectionString("Template");
DataBase.ControlPanel = builder.Configuration["Db:ControlPanel"] ?? DataBase.ControlPanel;
DataBase.CSTL_ERP_DB = builder.Configuration["Db:CSTL_ERP_DB"] ?? DataBase.CSTL_ERP_DB;

builder.Services.AddHttpContextAccessor();
builder.Services
    .AddControllersWithViews(o =>
    {
        o.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        o.Filters.Add(new SessionRequiredFilter());
        o.Filters.Add<MenuAccessFilter>();
    })
    // MVC 5 serialized with JavaScriptSerializer: property names as declared, DateTime as "\/Date(ms)\/" - the inline
    // page scripts read result[i].GLCode etc., so camelCase would break every list page.
    .AddNewtonsoftJson(o =>
    {
        o.SerializerSettings.ContractResolver = new DefaultContractResolver();
        o.SerializerSettings.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
        o.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    });
builder.Services.AddAntiforgery(o => o.HeaderName = "RequestVerificationToken");
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(o =>
{
    o.IdleTimeout = TimeSpan.FromMinutes(20); // legacy: InProc session, default 20-minute sliding timeout
    o.Cookie.HttpOnly = true;
    o.Cookie.IsEssential = true;
    o.Cookie.Name = ".NewAccounts.Session";
});

var app = builder.Build();
New_Accounts.Compat.HttpContext.Accessor = app.Services.GetRequiredService<IHttpContextAccessor>();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error"); // legacy: HandleErrorAttribute -> Views/Shared/Error.cshtml
}

app.UseStaticFiles();
app.UseMiddleware<LegacyJsonBodyMiddleware>();
app.UseRouting();
app.UseSession();
app.MapControllerRoute("CrystalViewer", "CrystalViewer/{page}.aspx", new { controller = "CrystalViewer", action = "Index" });
app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}"); // legacy RouteConfig: the only route

app.Run();
