using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVocabApp.Migrations
{
    /// <inheritdoc />
    public partial class EnableCascadeDeleteFoldersUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoldersUsers_Folders_FolderId",
                table: "FoldersUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_FoldersUsers_Folders_FolderId",
                table: "FoldersUsers",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoldersUsers_Folders_FolderId",
                table: "FoldersUsers");

            migrationBuilder.AddForeignKey(
                name: "FK_FoldersUsers_Folders_FolderId",
                table: "FoldersUsers",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }
    }
}
