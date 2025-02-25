using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVocabApp.Migrations
{
    /// <inheritdoc />
    public partial class _250220251416 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Synonyms",
                table: "Words",
                newName: "SynonymsString");

            migrationBuilder.RenameColumn(
                name: "Antonyms",
                table: "Words",
                newName: "AntonymsString");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SynonymsString",
                table: "Words",
                newName: "Synonyms");

            migrationBuilder.RenameColumn(
                name: "AntonymsString",
                table: "Words",
                newName: "Antonyms");
        }
    }
}
