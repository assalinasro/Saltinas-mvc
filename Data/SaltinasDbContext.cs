using Microsoft.EntityFrameworkCore;
using SaltinasMvc.Models;

namespace SaltinasMvc.Data;

/// <summary>
/// DbContext: es el puente entre las clases de C# y la base de datos SQL Server.
/// Cada DbSet representa una tabla. Las migraciones (Code First) se generan a partir de este archivo.
/// </summary>
public class SaltinasDbContext : DbContext
{
    public SaltinasDbContext(DbContextOptions<SaltinasDbContext> opciones) : base(opciones)
    {
    }

    // Tablas
    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();
    public DbSet<Cliente> Clientes => Set<Cliente>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- Categoria ----------
        modelBuilder.Entity<Categoria>(entidad =>
        {
            entidad.Property(c => c.Nombre).IsRequired().HasMaxLength(50);
            entidad.Property(c => c.Icono).HasMaxLength(10);
            entidad.HasIndex(c => c.Nombre).IsUnique();   // no se repiten nombres de categoría
        });

        // ---------- Producto ----------
        modelBuilder.Entity<Producto>(entidad =>
        {
            entidad.Property(p => p.Nombre).IsRequired().HasMaxLength(80);
            entidad.Property(p => p.Imagen).IsRequired().HasMaxLength(200);

            // Relación uno a muchos: una Categoria tiene muchos Productos.
            // Restrict = no se puede borrar una categoría que todavía tenga productos.
            entidad.HasOne(p => p.Categoria)
                   .WithMany(c => c.Productos)
                   .HasForeignKey(p => p.CategoriaId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        // ---------- Cliente ----------
        modelBuilder.Entity<Cliente>(entidad =>
        {
            entidad.Property(c => c.NombreCompleto).IsRequired().HasMaxLength(80);
            entidad.Property(c => c.Correo).IsRequired().HasMaxLength(120);
            entidad.Property(c => c.Telefono).IsRequired().HasMaxLength(10);
            entidad.Property(c => c.ClaveHash).IsRequired().HasMaxLength(200);
            entidad.Property(c => c.Genero).HasMaxLength(20);
            entidad.Property(c => c.FechaNacimiento).HasColumnType("date");
            entidad.HasIndex(c => c.Correo).IsUnique();
        });

        // ---------- Datos iniciales (se insertan con la migración) ----------
        modelBuilder.Entity<Categoria>().HasData(
            new Categoria { Id = 1, Nombre = "Jeans tiro bajo", Icono = "👖" },
            new Categoria { Id = 2, Nombre = "Tops brillantes", Icono = "✨" },
            new Categoria { Id = 3, Nombre = "Faldas mini", Icono = "🎀" },
            new Categoria { Id = 4, Nombre = "Chaquetas denim", Icono = "🧥" },
            new Categoria { Id = 5, Nombre = "Vestidos", Icono = "👗" },
            new Categoria { Id = 6, Nombre = "Accesorios", Icono = "🕶️" }
        );

        modelBuilder.Entity<Producto>().HasData(
            new Producto { Id = 1,  Nombre = "Jean cadera baja azul",    CategoriaId = 1, Precio = 129000, Imagen = "Imagenes/jean1.jpeg",       Destacado = true },
            new Producto { Id = 2,  Nombre = "Jean cargo tiro bajo",     CategoriaId = 1, Precio = 145000, Imagen = "Imagenes/jean2.jpeg" },
            new Producto { Id = 3,  Nombre = "Top brillante halter",     CategoriaId = 2, Precio = 68000,  Imagen = "Imagenes/top1.jpeg",        Destacado = true },
            new Producto { Id = 4,  Nombre = "Top lentejuelas plateado", CategoriaId = 2, Precio = 79000,  Imagen = "Imagenes/top2.jpeg" },
            new Producto { Id = 5,  Nombre = "Falda mini denim",         CategoriaId = 3, Precio = 89000,  Imagen = "Imagenes/falda1.jpeg" },
            new Producto { Id = 6,  Nombre = "Falda mini plisada",       CategoriaId = 3, Precio = 75000,  Imagen = "Imagenes/falda2.jpeg" },
            new Producto { Id = 7,  Nombre = "Chaqueta denim oversize",  CategoriaId = 4, Precio = 159000, Imagen = "Imagenes/chaqueta1.jpeg",   Destacado = true },
            new Producto { Id = 8,  Nombre = "Chaqueta denim corta",     CategoriaId = 4, Precio = 139000, Imagen = "Imagenes/chaqueta2.jpeg" },
            new Producto { Id = 9,  Nombre = "Vestido de fiesta Y2K",    CategoriaId = 5, Precio = 135000, Imagen = "Imagenes/vestido1.jpeg",    Destacado = true },
            new Producto { Id = 10, Nombre = "Vestido satinado corto",   CategoriaId = 5, Precio = 119000, Imagen = "Imagenes/vestido2.jpeg" },
            new Producto { Id = 11, Nombre = "Gafas retro Y2K",          CategoriaId = 6, Precio = 45000,  Imagen = "Imagenes/accesorios1.jpeg" },
            new Producto { Id = 12, Nombre = "Cinturón metálico",        CategoriaId = 6, Precio = 39000,  Imagen = "Imagenes/accesorios2.jpeg" }
        );
    }
}
