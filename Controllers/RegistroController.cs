using Microsoft.AspNetCore.Mvc;
using SaltinasMvc.Models;
using SaltinasMvc.Services;

namespace SaltinasMvc.Controllers;

public class RegistroController : Controller
{
    private readonly ClienteService _clientes;

    public RegistroController(ClienteService clientes)
    {
        _clientes = clientes;
    }

    // GET: /Registro  -> muestra el formulario
    [HttpGet]
    public IActionResult Index()
    {
        return View(new RegistroViewModel());
    }

    // POST: /Registro  -> valida, procesa y guarda la información
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(RegistroViewModel modelo)
    {
        // Reglas que no se pueden expresar solo con anotaciones
        if (!modelo.AceptaTerminos)
        {
            ModelState.AddModelError(nameof(modelo.AceptaTerminos), "Debes aceptar los términos y condiciones.");
        }

        if (modelo.FechaNacimiento.HasValue)
        {
            var hoy = DateTime.Today;
            var fecha = modelo.FechaNacimiento.Value.Date;
            int edad = hoy.Year - fecha.Year;
            if (fecha > hoy.AddYears(-edad)) edad--;

            if (fecha > hoy)
            {
                ModelState.AddModelError(nameof(modelo.FechaNacimiento), "La fecha de nacimiento no puede ser futura.");
            }
            else if (edad < 13)
            {
                ModelState.AddModelError(nameof(modelo.FechaNacimiento), "Debes tener al menos 13 años para registrarte.");
            }
        }

        if (!string.IsNullOrWhiteSpace(modelo.Correo) && _clientes.ExisteCorreo(modelo.Correo))
        {
            ModelState.AddModelError(nameof(modelo.Correo), "Este correo ya está registrado.");
        }

        if (!ModelState.IsValid)
        {
            // Se devuelve la misma vista con los errores; por seguridad no se reenvían las contraseñas
            modelo.Clave = string.Empty;
            modelo.ConfirmarClave = string.Empty;
            ModelState.Remove(nameof(modelo.Clave));
            ModelState.Remove(nameof(modelo.ConfirmarClave));
            return View(modelo);
        }

        var cliente = _clientes.Registrar(modelo);

        // Patrón Post-Redirect-Get: evita reenviar el formulario si se recarga la página
        TempData["Nombre"] = cliente.NombreCompleto;
        TempData["Numero"] = cliente.Id;
        return RedirectToAction(nameof(Gracias));
    }

    // GET: /Registro/Gracias
    [HttpGet]
    public IActionResult Gracias()
    {
        var nombre = TempData["Nombre"] as string;
        if (nombre is null)
        {
            return RedirectToAction(nameof(Index));
        }

        ViewBag.Nombre = nombre;
        ViewBag.Numero = TempData["Numero"];
        return View();
    }
}
