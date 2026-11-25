using Microsoft.AspNetCore.Mvc;
using SaltinasMvc.Models;
using SaltinasMvc.Services;

namespace SaltinasMvc.Controllers;

public class CatalogoController : Controller
{
    private readonly CatalogoService _catalogo;

    public CatalogoController(CatalogoService catalogo)
    {
        _catalogo = catalogo;
    }

    // GET: /Catalogo  o  /Catalogo?categoria=Vestidos
    [HttpGet]
    public IActionResult Index(string? categoria)
    {
        var modelo = new CatalogoViewModel
        {
            CategoriaActiva = string.IsNullOrWhiteSpace(categoria) ? "todas" : categoria,
            Categorias = _catalogo.ObtenerCategorias(),
            Productos = _catalogo.ObtenerPorCategoria(categoria)
        };

        return View(modelo);
    }
}
