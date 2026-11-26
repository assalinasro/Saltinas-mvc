using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Data;
using SaltinasMvc.Models;

namespace SaltinasMvc.Controllers;

/// <summary>
/// CRUD de productos con Entity Framework Core.
/// Create = Add + SaveChanges (INSERT), Read = consultas LINQ (SELECT),
/// Update = Update + SaveChanges (UPDATE), Delete = Remove + SaveChanges (DELETE).
/// </summary>
public class ProductosController : Controller
{
    private readonly SaltinasDbContext _db;

    public ProductosController(SaltinasDbContext db)
    {
        _db = db;
    }

    // ---------- READ (lista) ----------
    // GET: /Productos  o  /Productos?buscar=jean
    [HttpGet]
    public async Task<IActionResult> Index(string? buscar)
    {
        var consulta = _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)   // datos de la entidad relacionada
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(buscar))
        {
            consulta = consulta.Where(p => p.Nombre.Contains(buscar));
        }

        ViewData["Buscar"] = buscar;

        var productos = await consulta
            .OrderBy(p => p.Categoria!.Nombre)
            .ThenBy(p => p.Nombre)
            .ToListAsync();

        return View(productos);
    }

    // ---------- READ (detalle) ----------
    // GET: /Productos/Details/5
    [HttpGet]
    public async Task<IActionResult> Details(int? id)
    {
        if (id is null) return NotFound();

        var producto = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null) return NotFound();

        return View(producto);
    }

    // ---------- CREATE ----------
    // GET: /Productos/Create  -> formulario con lista desplegable de categorías
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await CargarCategoriasAsync();
        return View(new Producto());
    }

    // POST: /Productos/Create  -> inserta el registro
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nombre,Precio,Imagen,Destacado,CategoriaId")] Producto producto)
    {
        await ValidarCategoriaAsync(producto);

        if (ModelState.IsValid)
        {
            _db.Productos.Add(producto);          // marca el producto para INSERT
            await _db.SaveChangesAsync();         // ejecuta el INSERT en SQL Server

            TempData["Mensaje"] = $"Se creó «{producto.Nombre}» correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // ---------- UPDATE ----------
    // GET: /Productos/Edit/5
    [HttpGet]
    public async Task<IActionResult> Edit(int? id)
    {
        if (id is null) return NotFound();

        var producto = await _db.Productos.FindAsync(id);   // búsqueda por clave primaria
        if (producto is null) return NotFound();

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // POST: /Productos/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Precio,Imagen,Destacado,CategoriaId")] Producto producto)
    {
        if (id != producto.Id) return NotFound();

        await ValidarCategoriaAsync(producto);

        if (ModelState.IsValid)
        {
            try
            {
                _db.Productos.Update(producto);       // marca el producto para UPDATE
                await _db.SaveChangesAsync();         // ejecuta el UPDATE
            }
            catch (DbUpdateConcurrencyException)
            {
                // Otro usuario lo eliminó mientras se editaba
                if (!await _db.Productos.AnyAsync(p => p.Id == id)) return NotFound();
                throw;
            }

            TempData["Mensaje"] = $"Se actualizó «{producto.Nombre}» correctamente.";
            return RedirectToAction(nameof(Index));
        }

        await CargarCategoriasAsync(producto.CategoriaId);
        return View(producto);
    }

    // ---------- DELETE ----------
    // GET: /Productos/Delete/5  -> página de confirmación
    [HttpGet]
    public async Task<IActionResult> Delete(int? id)
    {
        if (id is null) return NotFound();

        var producto = await _db.Productos
            .AsNoTracking()
            .Include(p => p.Categoria)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (producto is null) return NotFound();

        return View(producto);
    }

    // POST: /Productos/Delete/5  -> elimina el registro
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var producto = await _db.Productos.FindAsync(id);

        if (producto is not null)
        {
            _db.Productos.Remove(producto);       // marca el producto para DELETE
            await _db.SaveChangesAsync();         // ejecuta el DELETE
            TempData["Mensaje"] = $"Se eliminó «{producto.Nombre}».";
        }

        return RedirectToAction(nameof(Index));
    }

    // ---------- Apoyo ----------

    // Llena la lista desplegable (select) con las categorías de la base de datos
    private async Task CargarCategoriasAsync(int? categoriaSeleccionada = null)
    {
        var categorias = await _db.Categorias
            .AsNoTracking()
            .OrderBy(c => c.Nombre)
            .ToListAsync();

        ViewBag.Categorias = new SelectList(categorias, "Id", "Nombre", categoriaSeleccionada);
    }

    // Comprueba que la categoría elegida exista realmente (clave foránea válida)
    private async Task ValidarCategoriaAsync(Producto producto)
    {
        if (producto.CategoriaId > 0 && !await _db.Categorias.AnyAsync(c => c.Id == producto.CategoriaId))
        {
            ModelState.AddModelError(nameof(producto.CategoriaId), "La categoría seleccionada no existe.");
        }
    }
}
