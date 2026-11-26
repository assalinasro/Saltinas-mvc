using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SaltinasMvc.Migrations
{
    /// <inheritdoc />
    public partial class CrearBaseInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categorias",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Icono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categorias", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clientes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreCompleto = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Correo = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    Telefono = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ClaveHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FechaNacimiento = table.Column<DateTime>(type: "date", nullable: false),
                    Genero = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    FechaRegistro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clientes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Productos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Precio = table.Column<int>(type: "int", nullable: false),
                    Imagen = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Destacado = table.Column<bool>(type: "bit", nullable: false),
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Productos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Productos_Categorias_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categorias",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categorias",
                columns: new[] { "Id", "Icono", "Nombre" },
                values: new object[,]
                {
                    { 1, "👖", "Jeans tiro bajo" },
                    { 2, "✨", "Tops brillantes" },
                    { 3, "🎀", "Faldas mini" },
                    { 4, "🧥", "Chaquetas denim" },
                    { 5, "👗", "Vestidos" },
                    { 6, "🕶️", "Accesorios" }
                });

            migrationBuilder.InsertData(
                table: "Productos",
                columns: new[] { "Id", "CategoriaId", "Destacado", "Imagen", "Nombre", "Precio" },
                values: new object[,]
                {
                    { 1, 1, true, "Imagenes/jean1.jpeg", "Jean cadera baja azul", 129000 },
                    { 2, 1, false, "Imagenes/jean2.jpeg", "Jean cargo tiro bajo", 145000 },
                    { 3, 2, true, "Imagenes/top1.jpeg", "Top brillante halter", 68000 },
                    { 4, 2, false, "Imagenes/top2.jpeg", "Top lentejuelas plateado", 79000 },
                    { 5, 3, false, "Imagenes/falda1.jpeg", "Falda mini denim", 89000 },
                    { 6, 3, false, "Imagenes/falda2.jpeg", "Falda mini plisada", 75000 },
                    { 7, 4, true, "Imagenes/chaqueta1.jpeg", "Chaqueta denim oversize", 159000 },
                    { 8, 4, false, "Imagenes/chaqueta2.jpeg", "Chaqueta denim corta", 139000 },
                    { 9, 5, true, "Imagenes/vestido1.jpeg", "Vestido de fiesta Y2K", 135000 },
                    { 10, 5, false, "Imagenes/vestido2.jpeg", "Vestido satinado corto", 119000 },
                    { 11, 6, false, "Imagenes/accesorios1.jpeg", "Gafas retro Y2K", 45000 },
                    { 12, 6, false, "Imagenes/accesorios2.jpeg", "Cinturón metálico", 39000 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_Nombre",
                table: "Categorias",
                column: "Nombre",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_Correo",
                table: "Clientes",
                column: "Correo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                column: "CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clientes");

            migrationBuilder.DropTable(
                name: "Productos");

            migrationBuilder.DropTable(
                name: "Categorias");
        }
    }
}
