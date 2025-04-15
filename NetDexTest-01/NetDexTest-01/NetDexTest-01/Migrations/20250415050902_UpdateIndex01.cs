using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetDexTest_01.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIndex01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Person_Nickname",
                table: "Person");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Nickname_DexHolderId",
                table: "Person",
                columns: new[] { "Nickname", "DexHolderId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Person_Nickname_DexHolderId",
                table: "Person");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Nickname",
                table: "Person",
                column: "Nickname",
                unique: true);
        }
    }
}
