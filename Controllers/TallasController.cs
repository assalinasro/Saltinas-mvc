using Microsoft.AspNetCore.Mvc;
using SaltinasMvc.Models;
using SaltinasMvc.Services;

namespace SaltinasMvc.Controllers;

public class TallasController : Controller
{
    private readonly TallaService _tallas;

    public TallasController(TallaService tallas)
    {
        _tallas = tallas;
    }

    // GET: /Tallas  -> muestra la tabla y el formulario vacío
    [HttpGet]
    public IActionResult Index()
    {
        return View(new TallaViewModel { Tabla = _tallas.Tabla });
    }

    // POST: /Tallas  -> recibe las medidas, calcula la talla en C# y vuelve a mostrar la vista
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(TallaViewModel modelo)
    {
        modelo.Tabla = _tallas.Tabla;

        if (!ModelState.IsValid)
        {
            return View(modelo);
        }

        modelo.Resultado = _tallas.Calcular(modelo.Estatura!.Value, modelo.Cintura!.Value);
        return View(modelo);
    }
}
