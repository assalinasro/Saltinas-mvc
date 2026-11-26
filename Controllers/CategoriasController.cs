using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Data;
using SaltinasMvc.Models;

namespace SaltinasMvc.Controllers;

/// <summary>CRUD de categorías (el lado "uno" de la relación 1:N con Productos).</summary>
public class CategoriasController : Controller
{
    private readonly SaltinasDbContext _db;

    public CategoriasController(SaltinasDbContext db)
    {
        _db = db;
    }

    // GET: /Categorias
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        // Include trae los productos de cada categoría para poder contarlos en la vista
        var categorias = await _db.Categorias
            .AsNoTracking()
            .Include(c => c.Productos)
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        return View(categorias);
    }

    // GET: /Categorias/Details/3  -> categoría con la lista de sus productos
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var categoria = await _db.Categorias
            .AsNoTracking()
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null) return NotFound();

        return View(categoria);
    }

    // GET: /Categorias/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View(new Categoria());
    }

    // POST: /Categorias/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Icono")] Categoria categoria)
    {
        if (await _db.Categorias.AnyAsync(c => c.Nombre == categoria.Nombre))
        {
            ModelState.AddModelError(nameof(categoria.Nombre), "Ya existe una categoría con ese nombre.");
        }

        if (ModelState.IsValid)
        {
            _db.Categorias.Add(categoria);
            await _db.SaveChangesAsync();

            TempData["Mensaje"] = $"Se creó la categoría «{categoria.Nombre}».";
            return RedirectToAction(nameof(Index));
        }

        return View(categoria);
    }

    // GET: /Categorias/Edit/3
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var categoria = await _db.Categorias.FindAsync(id);
        if (categoria is null) return NotFound();

        return View(categoria);
    }

    // POST: /Categorias/Edit/3
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Icono")] Categoria categoria)
    {
        if (id != categoria.Id) return NotFound();

        if (await _db.Categorias.AnyAsync(c => c.Nombre == categoria.Nombre && c.Id != categoria.Id))
        {
            ModelState.AddModelError(nameof(categoria.Nombre), "Ya existe otra categoría con ese nombre.");
        }

        if (ModelState.IsValid)
        {
            try
            {
                _db.Categorias.Update(categoria);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _db.Categorias.AnyAsync(c => c.Id == id)) return NotFound();
                throw;
            }

            TempData["Mensaje"] = $"Se actualizó la categoría «{categoria.Nombre}».";
            return RedirectToAction(nameof(Index));
        }

        return View(categoria);
    }

    // GET: /Categorias/Delete/3
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var categoria = await _db.Categorias
            .AsNoTracking()
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null) return NotFound();

        return View(categoria);
    }

    // POST: /Categorias/Delete/3
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var categoria = await _db.Categorias.FindAsync(id);
        if (categoria is null) return RedirectToAction(nameof(Index));

        // Integridad referencial: no se elimina una categoría que todavía tenga productos
        if (await _db.Productos.AnyAsync(p => p.CategoriaId == id))
        {
            TempData["Error"] = $"No se puede eliminar «{categoria.Nombre}» porque tiene productos asociados. " +
                                "Elimina o cambia de categoría esos productos primero.";
            return RedirectToAction(nameof(Index));
        }

        _db.Categorias.Remove(categoria);
        await _db.SaveChangesAsync();

        TempData["Mensaje"] = $"Se eliminó la categoría «{categoria.Nombre}».";
        return RedirectToAction(nameof(Index));
    }
}
