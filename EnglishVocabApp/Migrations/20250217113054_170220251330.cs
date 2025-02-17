using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EnglishVocabApp.Migrations
{
    /// <inheritdoc />
    public partial class _170220251330 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FoldersUsers",
                columns: table => new
                {
                    FolderId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoldersUsers", x => new { x.UserId, x.FolderId });
                    table.ForeignKey(
                        name: "FK_FoldersUsers_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FoldersUsers_Folders_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Folders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FoldersUsers_FolderId",
                table: "FoldersUsers",
                column: "FolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoldersUsers");
        }
    }
}
