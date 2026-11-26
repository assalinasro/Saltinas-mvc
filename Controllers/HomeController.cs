using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Data;
using SaltinasMvc.Models;

namespace SaltinasMvc.Controllers;

public class HomeController : Controller
{
    private readonly SaltinasDbContext _db;

    public HomeController(SaltinasDbContext db)
    {
        _db = db;
    }

    // GET: /
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var modelo = new HomeViewModel
        {
            // SELECT * FROM Categorias ORDER BY Id
            Categorias = await _db.Categorias
                .AsNoTracking()
                .OrderBy(c => c.Id)
                .ToListAsync(),

            // SELECT ... FROM Productos JOIN Categorias WHERE Destacado = 1
            Destacados = await _db.Productos
                .AsNoTracking()
                .Include(p => p.Categoria)
                .Where(p => p.Destacado)
                .OrderBy(p => p.Id)
                .ToListAsync()
        };

        return View(modelo);
    }

    // GET: /Home/Promociones
    [HttpGet]
    public IActionResult Promociones()
    {
        return View();
    }

    // GET: /Home/Error?codigo=404
    [HttpGet]
    public IActionResult Error(int? codigo)
    {
        ViewBag.Codigo = codigo ?? 500;
        return View();
    }
}
