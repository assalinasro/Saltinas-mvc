using SaltinasMvc.Models;

namespace SaltinasMvc.Services;

/// <summary>
/// Lógica de negocio de la guía de tallas. Usa la misma tabla de la tienda original
/// y la regla "si tu medida está entre dos tallas, elige siempre la más grande".
/// </summary>
public class TallaService
{
    public IReadOnlyList<RangoTalla> Tabla { get; } = new List<RangoTalla>
    {
        new(150, 155, 60, 64, 6,  "S"),
        new(156, 160, 65, 69, 8,  "S"),
        new(161, 165, 70, 74, 10, "M"),
        new(166, 170, 75, 79, 12, "M"),
        new(171, 175, 80, 84, 14, "L")
    };

    public ResultadoTalla Calcular(int estatura, int cintura)
    {
        int filaEstatura = BuscarFila(estatura, r => r.EstaturaMax);
        int filaCintura = BuscarFila(cintura, r => r.CinturaMax);

        // La talla más grande gana
        int fila = Math.Max(filaEstatura, filaCintura);
        var rango = Tabla[fila];

        bool fueraDeRango =
            estatura < Tabla[0].EstaturaMin || estatura > Tabla[^1].EstaturaMax ||
            cintura < Tabla[0].CinturaMin || cintura > Tabla[^1].CinturaMax;

        string mensaje;
        if (fueraDeRango)
        {
            mensaje = "Tus medidas están fuera de nuestra tabla. Escríbenos por WhatsApp y te asesoramos.";
        }
        else if (filaEstatura != filaCintura)
        {
            mensaje = "Tus medidas están entre dos tallas, así que te recomendamos la más grande.";
        }
        else
        {
            mensaje = "¡Tus medidas coinciden perfecto con esta talla!";
        }

        return new ResultadoTalla
        {
            TallaJean = rango.TallaJean,
            TallaTop = rango.TallaTop,
            IndiceFila = fila,
            FueraDeRango = fueraDeRango,
            Mensaje = mensaje
        };
    }

    private int BuscarFila(int valor, Func<RangoTalla, int> maximo)
    {
        for (int i = 0; i < Tabla.Count; i++)
        {
            if (valor <= maximo(Tabla[i]))
            {
                return i;
            }
        }

        return Tabla.Count - 1;
    }
}
