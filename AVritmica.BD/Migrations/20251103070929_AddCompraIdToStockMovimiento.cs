using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVritmica.BD.Migrations
{
    /// <inheritdoc />
    public partial class AddCompraIdToStockMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CompraId",
                table: "StockMovimientos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovimientos_CompraId",
                table: "StockMovimientos",
                column: "CompraId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovimientos_Compras_CompraId",
                table: "StockMovimientos",
                column: "CompraId",
                principalTable: "Compras",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovimientos_Compras_CompraId",
                table: "StockMovimientos");

            migrationBuilder.DropIndex(
                name: "IX_StockMovimientos_CompraId",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "CompraId",
                table: "StockMovimientos");
        }
    }
}
