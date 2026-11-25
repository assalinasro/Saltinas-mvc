using System.Security.Cryptography;
using SaltinasMvc.Models;

namespace SaltinasMvc.Services;

/// <summary>
/// Guarda las clientas registradas en memoria y aplica la lógica del registro:
/// detectar correos repetidos y proteger la contraseña con un hash PBKDF2.
/// </summary>
public class ClienteService
{
    private readonly List<Cliente> _clientes = new();
    private readonly object _candado = new();

    public bool ExisteCorreo(string correo)
    {
        lock (_candado)
        {
            return _clientes.Any(c => c.Correo.Equals(correo, StringComparison.OrdinalIgnoreCase));
        }
    }

    public Cliente Registrar(RegistroViewModel datos)
    {
        lock (_candado)
        {
            var cliente = new Cliente
            {
                Id = _clientes.Count + 1,
                NombreCompleto = datos.NombreCompleto.Trim(),
                Correo = datos.Correo.Trim().ToLowerInvariant(),
                Telefono = datos.Telefono,
                ClaveHash = CrearHash(datos.Clave),
                FechaNacimiento = datos.FechaNacimiento!.Value,
                Genero = datos.Genero,
                FechaRegistro = DateTime.UtcNow
            };

            _clientes.Add(cliente);
            return cliente;
        }
    }

    private static string CrearHash(string clave)
    {
        var sal = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(clave, sal, 100_000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(sal) + ":" + Convert.ToBase64String(hash);
    }
}
