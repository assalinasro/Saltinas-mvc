using Microsoft.AspNetCore.Mvc;
using SaltinasMvc.Models;
using SaltinasMvc.Services;

namespace SaltinasMvc.Controllers;

public class HomeController : Controller
{
    private readonly CatalogoService _catalogo;

    public HomeController(CatalogoService catalogo)
    {
        _catalogo = catalogo;
    }

    // GET: /
    [HttpGet]
    public IActionResult Index()
    {
        var modelo = new HomeViewModel
        {
            Categorias = _catalogo.ObtenerCategorias(),
            Destacados = _catalogo.ObtenerDestacados()
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
