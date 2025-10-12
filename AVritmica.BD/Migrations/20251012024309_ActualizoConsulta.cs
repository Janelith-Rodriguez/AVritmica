using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVritmica.BD.Migrations
{
    /// <inheritdoc />
    public partial class ActualizoConsulta : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Leida",
                table: "Consultas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Leida",
                table: "Consultas");
        }
    }
}
