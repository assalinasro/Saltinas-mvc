using System.ComponentModel.DataAnnotations;

namespace SaltinasMvc.Models;

/// <summary>Una fila de la guía de tallas (estatura y cintura en cm).</summary>
public record RangoTalla(int EstaturaMin, int EstaturaMax, int CinturaMin, int CinturaMax, int TallaJean, string TallaTop);

/// <summary>Resultado calculado en C# a partir de las medidas de la clienta.</summary>
public class ResultadoTalla
{
    public int TallaJean { get; set; }
    public string TallaTop { get; set; } = string.Empty;
    public int IndiceFila { get; set; }
    public bool FueraDeRango { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}

/// <summary>Modelo del formulario "Calcula tu talla" y de la tabla de referencia.</summary>
public class TallaViewModel
{
    [Required(ErrorMessage = "Escribe tu estatura en centímetros.")]
    [Range(120, 220, ErrorMessage = "Ingresa una estatura entre 120 y 220 cm.")]
    [Display(Name = "Estatura (cm)")]
    public int? Estatura { get; set; }

    [Required(ErrorMessage = "Escribe el contorno de tu cintura en centímetros.")]
    [Range(40, 140, ErrorMessage = "Ingresa una cintura entre 40 y 140 cm.")]
    [Display(Name = "Cintura (cm)")]
    public int? Cintura { get; set; }

    public ResultadoTalla? Resultado { get; set; }

    public IReadOnlyList<RangoTalla> Tabla { get; set; } = new List<RangoTalla>();
}
