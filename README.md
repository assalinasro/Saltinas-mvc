# Saltinas — Tienda de ropa (ASP.NET Core MVC + EF Core + SQL Server)

Tienda de ropa femenina inspirada en la moda de los años 2000. Proyecto individual de Programación Web.
Tecnologías: ASP.NET Core MVC, C#, Razor, Bootstrap, JavaScript, Entity Framework Core (Code First) y SQL Server.

## Requisitos para ejecutarlo

- SDK de .NET 10 (`dotnet --version`)
- SQL Server Express y SQL Server Management Studio (SSMS)
- Herramienta de migraciones de EF Core (una sola vez):

```bash
dotnet tool install --global dotnet-ef
```

## Cadena de conexión

Está en `appsettings.json`, en `ConnectionStrings:SaltinasDb`. Usa autenticación de Windows, así que **no contiene usuario ni contraseña**:

```
Server=localhost\SQLEXPRESS;Database=SaltinasDb;Trusted_Connection=True;TrustServerCertificate=True
```

Si tu instancia se llama distinto, cambia solo `Server=`. El nombre es el que aparece en "Server name" al abrir SSMS
(por ejemplo `.\SQLEXPRESS`, `localhost\SQLEXPRESS` o `(localdb)\MSSQLLocalDB`).

## Crear la base de datos (Code First)

Desde la carpeta del proyecto:

```bash
dotnet ef migrations add CrearBaseInicial
dotnet ef database update
dotnet run
```

- `migrations add` lee las clases de `Models/` y el `SaltinasDbContext` y genera la carpeta `Migrations/` con el código que crea las tablas.
- `database update` ejecuta esa migración en SQL Server: crea la base `SaltinasDb`, las tablas, las claves y los datos iniciales (6 categorías y 12 productos).
- Si más adelante cambias un modelo, creas otra migración (`dotnet ef migrations add NombreDelCambio`) y vuelves a ejecutar `dotnet ef database update`.

Abre http://localhost:5080

## Modelo de datos (relación 1:N)

```
Categorias (1) ──────< (N) Productos
   Id (PK)                   Id (PK)
   Nombre                    Nombre
   Icono                     Precio
                             Imagen
                             Destacado
                             CategoriaId (FK → Categorias.Id)

Clientes (registro de la comunidad: Id, NombreCompleto, Correo, Telefono, ClaveHash, ...)
```

- Una **Categoría** tiene muchos **Productos**; cada Producto pertenece a una sola Categoría.
- En C#: `Producto.CategoriaId` (clave foránea), `Producto.Categoria` (navegación al "uno") y `Categoria.Productos` (navegación a los "muchos").
- La relación se configura en `Data/SaltinasDbContext.cs` con `HasOne(...).WithMany(...).HasForeignKey(...)`.
- Al eliminar una categoría con productos, la base de datos lo impide (`DeleteBehavior.Restrict`) y la aplicación muestra un aviso.

## Estructura (MVC)

| Carpeta / archivo | Responsabilidad |
|---|---|
| `Models/` | Entidades (`Categoria`, `Producto`, `Cliente`) y modelos de vista. |
| `Data/SaltinasDbContext.cs` | DbContext: `DbSet` (tablas), relaciones y datos iniciales. |
| `Controllers/` | `Productos` y `Categorias` (CRUD), `Catalogo`, `Home`, `Tallas`, `Registro`. |
| `Views/` | Vistas Razor con Bootstrap. |
| `Services/` | `ClienteService` (guarda clientas en SQL Server) y `TallaService` (cálculo de talla). |
| `Migrations/` | Se genera con `dotnet ef migrations add` (historial de la estructura de la base de datos). |
| `wwwroot/` | CSS, JavaScript del carrito, imágenes y video. |

## CRUD

En el menú **Administrar**:

| Operación | Productos | Código EF Core |
|---|---|---|
| Consultar | `/Productos`, `/Productos/Details/5` | `Include(...)`, `Where(...)`, `ToListAsync()` |
| Crear | `/Productos/Create` | `Add(...)` + `SaveChangesAsync()` → INSERT |
| Modificar | `/Productos/Edit/5` | `Update(...)` + `SaveChangesAsync()` → UPDATE |
| Eliminar | `/Productos/Delete/5` | `Remove(...)` + `SaveChangesAsync()` → DELETE |

Lo mismo existe para **Categorías** (`/Categorias`). El formulario de producto tiene una **lista desplegable** con las
categorías leídas de la base de datos.

Las páginas públicas (Inicio y Catálogo) también leen de SQL Server, así que lo que crees en Administrar aparece en la tienda.

## Verificar en SSMS

1. Conéctate a tu instancia de SQL Server Express.
2. Bases de datos → `SaltinasDb` → Tablas: verás `Categorias`, `Productos`, `Clientes` y `__EFMigrationsHistory`.
3. Clic derecho en una tabla → **Select Top 1000 Rows** para ver los registros.
4. Para ver la relación: `SaltinasDb` → Database Diagrams (o en `Productos` → Keys, la clave foránea `FK_Productos_Categorias_CategoriaId`).
5. Consulta con JOIN para mostrar en el video:

```sql
SELECT p.Id, p.Nombre, p.Precio, c.Nombre AS Categoria
FROM Productos p
INNER JOIN Categorias c ON c.Id = p.CategoriaId
ORDER BY c.Nombre, p.Nombre;
```

## Git

Importante: la aplicación publicada en Render no tiene acceso a tu SQL Server local. Para no dañarla, sube estos
cambios en una **rama aparte**; Render solo despliega `main`.

```bash
git checkout -b bd-sqlserver
git add Models Data
git commit -m "Agrega entidades Categoria y Producto, y DbContext"
git add Migrations
git commit -m "Agrega migraciones de EF Core (Code First)"
git add Controllers Views
git commit -m "Agrega CRUD de productos y categorias con lista desplegable"
git add .
git commit -m "Actualiza catalogo y registro para usar SQL Server"
git push -u origin bd-sqlserver
```

No se debe subir información sensible. La cadena de conexión usa autenticación de Windows, sin contraseñas.
Si algún día usas usuario y contraseña de SQL, guárdalos con `dotnet user-secrets` y no en `appsettings.json`.
