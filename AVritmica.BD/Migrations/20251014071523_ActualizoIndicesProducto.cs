using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVritmica.BD.Migrations
{
    /// <inheritdoc />
    public partial class ActualizoIndicesProducto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Productos_Precio",
                table: "Productos");

            migrationBuilder.RenameIndex(
                name: "IX_Productos_IdCategoria",
                table: "Productos",
                newName: "IX_Productos_CategoriaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Productos_CategoriaId",
                table: "Productos",
                newName: "IX_Productos_IdCategoria");

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Precio",
                table: "Productos",
                column: "Precio");
        }
    }
}
