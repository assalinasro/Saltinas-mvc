namespace SaltinasMvc.Models;

/// <summary>
/// Entidad que representa una prenda del catálogo de Saltinas.
/// </summary>
public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public int Precio { get; set; }
    public string Imagen { get; set; } = string.Empty;

    /// <summary>Precio con formato colombiano, por ejemplo $129.000</summary>
    public string PrecioFormateado =>
        "$" + Precio.ToString("N0", System.Globalization.CultureInfo.InvariantCulture).Replace(",", ".");
}
