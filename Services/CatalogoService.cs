using SaltinasMvc.Models;

namespace SaltinasMvc.Services;

/// <summary>
/// Entrega los productos del catálogo. Los datos viven en memoria
/// (el mismo catálogo que tenía la tienda original en JavaScript).
/// </summary>
public class CatalogoService
{
    private readonly List<Producto> _productos = new()
    {
        new Producto { Id = 1,  Nombre = "Jean cadera baja azul",    Categoria = "Jeans tiro bajo",   Precio = 129000, Imagen = "Imagenes/jean1.jpeg" },
        new Producto { Id = 2,  Nombre = "Jean cargo tiro bajo",     Categoria = "Jeans tiro bajo",   Precio = 145000, Imagen = "Imagenes/jean2.jpeg" },
        new Producto { Id = 3,  Nombre = "Top brillante halter",     Categoria = "Tops brillantes",   Precio = 68000,  Imagen = "Imagenes/top1.jpeg" },
        new Producto { Id = 4,  Nombre = "Top lentejuelas plateado", Categoria = "Tops brillantes",   Precio = 79000,  Imagen = "Imagenes/top2.jpeg" },
        new Producto { Id = 5,  Nombre = "Falda mini denim",         Categoria = "Faldas mini",       Precio = 89000,  Imagen = "Imagenes/falda1.jpeg" },
        new Producto { Id = 6,  Nombre = "Falda mini plisada",       Categoria = "Faldas mini",       Precio = 75000,  Imagen = "Imagenes/falda2.jpeg" },
        new Producto { Id = 7,  Nombre = "Chaqueta denim oversize",  Categoria = "Chaquetas denim",   Precio = 159000, Imagen = "Imagenes/chaqueta1.jpeg" },
        new Producto { Id = 8,  Nombre = "Chaqueta denim corta",     Categoria = "Chaquetas denim",   Precio = 139000, Imagen = "Imagenes/chaqueta2.jpeg" },
        new Producto { Id = 9,  Nombre = "Vestido de fiesta Y2K",    Categoria = "Vestidos",          Precio = 135000, Imagen = "Imagenes/vestido1.jpeg" },
        new Producto { Id = 10, Nombre = "Vestido satinado corto",   Categoria = "Vestidos",          Precio = 119000, Imagen = "Imagenes/vestido2.jpeg" },
        new Producto { Id = 11, Nombre = "Gafas retro Y2K",          Categoria = "Accesorios",        Precio = 45000,  Imagen = "Imagenes/accesorios1.jpeg" },
        new Producto { Id = 12, Nombre = "Cinturón metálico",        Categoria = "Accesorios",        Precio = 39000,  Imagen = "Imagenes/accesorios2.jpeg" }
    };

    private static readonly Dictionary<string, string> Iconos = new()
    {
        ["Jeans tiro bajo"] = "👖",
        ["Tops brillantes"] = "✨",
        ["Faldas mini"] = "🎀",
        ["Chaquetas denim"] = "🧥",
        ["Vestidos"] = "👗",
        ["Accesorios"] = "🕶️"
    };

    public IReadOnlyList<CategoriaItem> ObtenerCategorias() =>
        _productos
            .Select(p => p.Categoria)
            .Distinct()
            .Select(c => new CategoriaItem(c, Iconos.GetValueOrDefault(c, "💗")))
            .ToList();

    public IReadOnlyList<Producto> ObtenerPorCategoria(string? categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria) || categoria.Equals("todas", StringComparison.OrdinalIgnoreCase))
        {
            return _productos;
        }

        return _productos
            .Where(p => p.Categoria.Equals(categoria, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    public IReadOnlyList<Producto> ObtenerDestacados() =>
        _productos.Where(p => p.Id is 1 or 3 or 7 or 9).ToList();
}
