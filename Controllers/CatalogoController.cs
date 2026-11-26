using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Data;
using SaltinasMvc.Models;

namespace SaltinasMvc.Controllers;

public class CatalogoController : Controller
{
    private readonly SaltinasDbContext _db;

    public CatalogoController(SaltinasDbContext db)
    {
        _db = db;
    }

    // GET: /Catalogo  o  /Catalogo?categoria=Vestidos
    [HttpGet]
    public async Task<IActionResult> Index(string? categoria)
    {
        // Include trae también la categoría de cada producto (JOIN)
        var consulta = _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .AsQueryable();

        bool todas = string.IsNullOrWhiteSpace(categoria) ||
                     categoria.Equals("todas", StringComparison.OrdinalIgnoreCase);

        if (!todas)
        {
            // WHERE Categorias.Nombre = @categoria
            consulta = consulta.Where(p => p.Categoria!.Nombre == categoria);
        }

        var modelo = new CatalogoViewModel
        {
            CategoriaActiva = todas ? "todas" : categoria!,
            Categorias = await _db.Categorias.AsNoTracking().OrderBy(c => c.Id).ToListAsync(),
            Productos = await consulta.OrderBy(p => p.Id).ToListAsync()
        };

        return View(modelo);
    }
}
