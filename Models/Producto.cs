using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaltinasMvc.Models;

/// <summary>
/// Prenda del catálogo de Saltinas (lado "muchos" de la relación 1:N).
/// Cada producto pertenece a una sola categoría.
/// </summary>
public class Producto
{
    // Clave primaria
    public int Id { get; set; }

    [Required(ErrorMessage = "Escribe el nombre de la prenda.")]
    [StringLength(80, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 80 caracteres.")]
    [Display(Name = "Nombre")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1000, 5000000, ErrorMessage = "El precio debe estar entre $1.000 y $5.000.000.")]
    [Display(Name = "Precio (COP)")]
    public int Precio { get; set; }

    [Required(ErrorMessage = "Escribe la ruta de la imagen, por ejemplo Imagenes/jean1.jpeg.")]
    [StringLength(200)]
    [Display(Name = "Imagen (ruta)")]
    public string Imagen { get; set; } = string.Empty;

    [Display(Name = "Prenda destacada")]
    public bool Destacado { get; set; }

    // Clave foránea hacia Categoria
    [Range(1, int.MaxValue, ErrorMessage = "Selecciona una categoría.")]
    [Display(Name = "Categoría")]
    public int CategoriaId { get; set; }

    // Propiedad de navegación: la categoría a la que pertenece
    public Categoria? Categoria { get; set; }

    /// <summary>Precio con formato colombiano, por ejemplo $129.000 (no se guarda en la base de datos).</summary>
    [NotMapped]
    public string PrecioFormateado =>
        "$" + Precio.ToString("N0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", ".");
}
