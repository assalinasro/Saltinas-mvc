using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Data;
using SaltinasMvc.Services;

var builder = WebApplication.CreateBuilder(args);

// Los servicios de hosting (Render, etc.) entregan el puerto por variable de entorno PORT
var puerto = Environment.GetEnvironmentVariable("PORT");
if (!string.IsNullOrEmpty(puerto))
{
    builder.WebHost.UseUrls($"http://0.0.0.0:{puerto}");
}

// MVC: controladores + vistas Razor
builder.Services.AddControllersWithViews();

// Entity Framework Core: el DbContext se conecta a SQL Server con la cadena "SaltinasDb" (appsettings.json)
builder.Services.AddDbContext<SaltinasDbContext>(opciones =>
    opciones.UseSqlServer(builder.Configuration.GetConnectionString("SaltinasDb")));

// Servicios propios (inyección de dependencias)
builder.Services.AddScoped<ClienteService>();     // Scoped: usa el DbContext, que vive una solicitud
builder.Services.AddSingleton<TallaService>();    // Singleton: no depende de la base de datos

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

// Páginas amigables para 404, 500, etc.
app.UseStatusCodePagesWithReExecute("/Home/Error", "?codigo={0}");

app.UseStaticFiles();   // sirve wwwroot (css, js, Imagenes, Videos)
app.UseRouting();

// Ruta por defecto: /Controlador/Accion/Id
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
