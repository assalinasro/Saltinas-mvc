namespace SaltinasMvc.Models;

/// <summary>Datos que el controlador entrega a la vista del catálogo.</summary>
public class CatalogoViewModel
{
    public string CategoriaActiva { get; set; } = "todas";
    public IReadOnlyList<CategoriaItem> Categorias { get; set; } = new List<CategoriaItem>();
    public IReadOnlyList<Producto> Productos { get; set; } = new List<Producto>();
}
