namespace SaltinasMvc.Models;

/// <summary>
/// Entidad que representa a una clienta registrada en la comunidad Saltinas.
/// La contraseña nunca se guarda en texto plano: solo su hash.
/// </summary>
public class Cliente
{
    public int Id { get; set; }
    public string NombreCompleto { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string ClaveHash { get; set; } = string.Empty;
    public DateTime FechaNacimiento { get; set; }
    public string? Genero { get; set; }
    public DateTime FechaRegistro { get; set; }
}
