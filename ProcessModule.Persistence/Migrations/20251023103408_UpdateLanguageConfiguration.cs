using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProcessModule.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLanguageConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Languages_Code",
                table: "Languages",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Languages_Code",
                table: "Languages");
        }
    }
}
