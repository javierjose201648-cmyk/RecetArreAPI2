using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecetArreAPI2.Migrations
{
    /// <inheritdoc />
    public partial class IngredienteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IdIngredientes",
                table: "Ingredientes",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ingredientes",
                newName: "IdIngredientes");
        }
    }
}
