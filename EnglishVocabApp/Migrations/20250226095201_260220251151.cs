using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVocabApp.Migrations
{
    /// <inheritdoc />
    public partial class _260220251151 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamplesString",
                table: "Words",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Meaning",
                table: "Words",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamplesString",
                table: "Words");

            migrationBuilder.DropColumn(
                name: "Meaning",
                table: "Words");
        }
    }
}
