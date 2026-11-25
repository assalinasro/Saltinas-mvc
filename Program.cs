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

// Servicios propios (inyección de dependencias). Singleton = una sola instancia en memoria.
builder.Services.AddSingleton<CatalogoService>();
builder.Services.AddSingleton<ClienteService>();
builder.Services.AddSingleton<TallaService>();

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
