using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetDexTest_01.Migrations
{
    /// <inheritdoc />
    public partial class AddDexHolder02 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "DexHolder",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                ApplicationUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                MiddleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                Gender = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                Pronouns = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_DexHolder", x => x.Id);
                table.ForeignKey(
                    name: "FK_DexHolder_AspNetUsers_ApplicationUserId",
                    column: x => x.ApplicationUserId,
                    principalTable: "AspNetUsers",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserName",
                table: "AspNetUsers",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");


            migrationBuilder.CreateIndex(
                name: "IX_DexHolder_ApplicationUserId",
                table: "DexHolder",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DexHolder_ApplicationUserName",
                table: "DexHolder",
                column: "ApplicationUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Person_DexHolderId",
                table: "Person",
                column: "DexHolderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DexHolder");

        }
    }
}
