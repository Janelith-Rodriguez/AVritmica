using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVritmica.BD.Migrations
{
    /// <inheritdoc />
    public partial class AddRelationsToStockMovimiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CarritoId",
                table: "StockMovimientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaDocumento",
                table: "StockMovimientos",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroFactura",
                table: "StockMovimientos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroOrdenCompra",
                table: "StockMovimientos",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductoId1",
                table: "StockMovimientos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Proveedor",
                table: "StockMovimientos",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UsuarioRegistro",
                table: "StockMovimientos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovimientos_CarritoId",
                table: "StockMovimientos",
                column: "CarritoId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovimientos_ProductoId1",
                table: "StockMovimientos",
                column: "ProductoId1");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovimientos_Proveedor",
                table: "StockMovimientos",
                column: "Proveedor");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovimientos_Carritos_CarritoId",
                table: "StockMovimientos",
                column: "CarritoId",
                principalTable: "Carritos",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovimientos_Productos_ProductoId1",
                table: "StockMovimientos",
                column: "ProductoId1",
                principalTable: "Productos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovimientos_Carritos_CarritoId",
                table: "StockMovimientos");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovimientos_Productos_ProductoId1",
                table: "StockMovimientos");

            migrationBuilder.DropIndex(
                name: "IX_StockMovimientos_CarritoId",
                table: "StockMovimientos");

            migrationBuilder.DropIndex(
                name: "IX_StockMovimientos_ProductoId1",
                table: "StockMovimientos");

            migrationBuilder.DropIndex(
                name: "IX_StockMovimientos_Proveedor",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "CarritoId",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "FechaDocumento",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "NumeroFactura",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "NumeroOrdenCompra",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "ProductoId1",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "Proveedor",
                table: "StockMovimientos");

            migrationBuilder.DropColumn(
                name: "UsuarioRegistro",
                table: "StockMovimientos");
        }
    }
}
