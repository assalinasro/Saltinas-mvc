namespace SaltinasMvc.Models;

/// <summary>Datos de la página de inicio: categorías y prendas destacadas.</summary>
public class HomeViewModel
{
    public IReadOnlyList<Categoria> Categorias { get; set; } = new List<Categoria>();
    public IReadOnlyList<Producto> Destacados { get; set; } = new List<Producto>();
}
