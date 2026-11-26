using System.ComponentModel.DataAnnotations;

namespace SaltinasMvc.Models;

/// <summary>
/// Categoría de ropa (lado "uno" de la relación 1:N).
/// Una categoría tiene muchos productos.
/// </summary>
public class Categoria
{
    // Clave primaria (por convención, "Id" es la PK)
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe el nombre de la categoría.")]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 50 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(10, ErrorMessage = "Usa un solo emoji o símbolo.")]
    [Display(Name = "Ícono")]
    public string Icono { get; set; } = "💗";

    // Propiedad de navegación: los productos de esta categoría
    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
}
