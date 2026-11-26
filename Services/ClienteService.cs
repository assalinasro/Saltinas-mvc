using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Data;
using SaltinasMvc.Models;

namespace SaltinasMvc.Services;

/// <summary>
/// Guarda las clientas registradas en SQL Server (tabla Clientes) y aplica la lógica del registro:
/// detectar correos repetidos y proteger la contraseña con un hash PBKDF2.
/// </summary>
public class ClienteService
{
    private readonly SaltinasDbContext _db;

    public ClienteService(SaltinasDbContext db)
    {
        _db = db;
    }

    public Task<bool> ExisteCorreoAsync(string correo)
    {
        var correoNormalizado = correo.Trim().ToLower();
        return _db.Clientes.AnyAsync(c => c.Correo == correoNormalizado);
    }

    public async Task<Cliente> RegistrarAsync(RegistroViewModel datos)
    {
        var cliente = new Cliente
        {
            NombreCompleto = datos.NombreCompleto.Trim(),
            Correo = datos.Correo.Trim().ToLowerInvariant(),
            Telefono = datos.Telefono,
            ClaveHash = CrearHash(datos.Clave),
            FechaNacimiento = datos.FechaNacimiento!.Value.Date,
            Genero = datos.Genero,
            FechaRegistro = DateTime.UtcNow
        };

        _db.Clientes.Add(cliente);        // INSERT INTO Clientes ...
        await _db.SaveChangesAsync();
        return cliente;
    }

    private static string CrearHash(string clave)
    {
        var sal = RandomNumberGenerator.GetBytes(16);
        var hash = Rfc2898DeriveBytes.Pbkdf2(clave, sal, 100_000, HashAlgorithmName.SHA256, 32);
        return Convert.ToBase64String(sal) + ":" + Convert.ToBase64String(hash);
    }
}
