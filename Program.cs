using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using partner_aluro.Data;
using partner_aluro.Models;
using partner_aluro.Services;
using partner_aluro.Services.Interfaces;
using System.Security.Policy;
using partner_aluro.Core;
using partner_aluro.Core.Repositories;
using partner_aluro.Repositories;
using Microsoft.Extensions.DependencyInjection;
using SmartBreadcrumbs.Extensions;
using System.Reflection;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Options;
using ServiceReference1;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DbTestPracaContextConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddSingleton<LanguageService>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

builder.Services.AddMvc()
    .AddViewLocalization()
    .AddDataAnnotationsLocalization(options =>
    {
        options.DataAnnotationLocalizerProvider = (type, factory) =>
        {
            var assemblyName = new AssemblyName(typeof(SharedResource).GetTypeInfo().Assembly.FullName);
            return factory.Create("SharedResource", assemblyName.Name);
        };
    });

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new List<CultureInfo>
    {
        new CultureInfo("pl-POL"),
        new CultureInfo("en-US"),
        new CultureInfo("de-DE"),
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "pl-POL", uiCulture: "pl-POL");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;

    options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
});


builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddResponseCaching(x => x.MaximumBodySize = 1024); //1. dodatkowo do lwykorzystawiania cache

builder.Services.AddBreadcrumbs(Assembly.GetExecutingAssembly(), options =>
{
    options.TagName = "nav";
    options.TagClasses = "";
    options.OlClasses = "breadcrumb";
    options.LiClasses = "breadcrumb-item";
    options.ActiveLiClasses = "breadcrumb-item active";
});

builder.Services.AddDbContext<ApplicationDbContext>(builder =>
{
    //builder.UseSqlServer(@"Data Source=mssql4.webio.pl,2401;Database=siwy55p_siwy55p;Uid=siwy55p_siwy55p;Password=Siiwy1a2!3!4!5!;TrustServerCertificate=True"); //connection string
    builder.UseSqlServer(connectionString); //connection string localbase

});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

#region

AddAuthorizationPolicies();

#endregion

AddScoped();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



var locOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseSession();

app.MapRazorPages();

app.MapControllerRoute(
    name: "StronyStatyczne",
    pattern: "strony/{nazwa}",
    defaults: new { controller = "Home", action = "StronyStatyczne" });

//app.MapControllerRoute(
//    name: "ProduktyList",
//    pattern: "Kategoria/{nazwaKategori}",
//    defaults: new { controller = "Category", action = "Lista" });

//orkiestra na dlugim rowerze.


//Endpoint
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.UseResponseCaching(); //2. dodatkowo do lwykorzystawiania cache

app.Use(async (context, next) =>
{
    context.Response.GetTypedHeaders().CacheControl =
    new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
    {
        Public = true,
        MaxAge = TimeSpan.FromSeconds(60)
    };
    context.Response.Headers[Microsoft.Net.Http.Headers.HeaderNames.Vary] = new string[] { "Accept-Encoding" };
    await next();
}); //CACHE

app.Run();



void AddAuthorizationPolicies()
{
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("EmployeeOnly", policy => policy.RequireClaim("EmployeeNumber"));
    });

    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy(Constants.Policies.RequireAdmin, policy => policy.RequireRole(Constants.Roles.Administrator));
        options.AddPolicy(Constants.Policies.RequireManager, policy => policy.RequireRole(Constants.Roles.Manager));
    });
}

void AddScoped()
{
    builder.Services.AddScoped<IOrderService, OrderService>();

    builder.Services.AddScoped<Cart>(sp => Cart.GetCart(sp));

    builder.Services.AddScoped<IProductService, ProductService>(); //Ktorej implementacji ma uzywac IWarehauseService
    builder.Services.AddScoped<IUnitOfWorkOrder, UnitOfWorkOrder>();

    builder.Services.AddScoped<ICategoryService, CategoryService>(); //Ktorej implementacji ma uzywac IWarehauseServic
    builder.Services.AddScoped<IApiService, ApiService>();
    builder.Services.AddScoped<IUnitOfWorkCategory, UnitOfWorkCategory>();
    builder.Services.AddScoped<IUnitOfWorkProduct, UnitOfWorkProduct>();

    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IRoleRepository, RoleRepository>();
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

    builder.Services.AddScoped<IAdress1rozliczeniowyService, Adress1rozliczeniowyService>();
    builder.Services.AddScoped<IUnitOfWorkAdress1rozliczeniowy, UnitOfWordAdress1rozliczeniowy>();

    builder.Services.AddScoped<IAdress2dostawyService, Adress2dostawyService>();
    builder.Services.AddScoped<IUnitOfWorkAdress2dostawy, UnitOfWordAdress2dostawy>();

    builder.Services.AddScoped<IProfildzialalnosciService, ProfildzialalnosciService>();


    builder.Services.AddScoped<IProfildzialalnosciService, ProfildzialalnosciService>();


}

#region

#endregion
