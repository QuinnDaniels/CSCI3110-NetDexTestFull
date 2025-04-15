using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NetDexTest_01.Migrations
{
    /// <inheritdoc />
    public partial class Initial01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "AspNetRoles",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoles", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUsers",
            //    columns: table => new
            //    {
            //        Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
            //        UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
            //        TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
            //        LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
            //        AccessFailedCount = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUsers", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetRoleClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserClaims",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserClaims_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserLogins",
            //    columns: table => new
            //    {
            //        LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //        ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //        ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserLogins_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserRoles",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
            //            column: x => x.RoleId,
            //            principalTable: "AspNetRoles",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_AspNetUserRoles_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "AspNetUserTokens",
            //    columns: table => new
            //    {
            //        UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
            //        LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //        Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
            //        Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
            //        table.ForeignKey(
            //            name: "FK_AspNetUserTokens_AspNetUsers_UserId",
            //            column: x => x.UserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "DexHolder",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        ApplicationUserId = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
            //        ApplicationUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            //        FirstName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        MiddleName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        LastName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
            //        Gender = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
            //        Pronouns = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_DexHolder", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_DexHolder_AspNetUsers_ApplicationUserId",
            //            column: x => x.ApplicationUserId,
            //            principalTable: "AspNetUsers",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Person",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "int", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Nickname = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
            //        DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        Gender = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
            //        Pronouns = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
            //        Rating = table.Column<int>(type: "int", nullable: false),
            //        Favorite = table.Column<bool>(type: "bit", nullable: false),
            //        DexHolderId = table.Column<int>(type: "int", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Person", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Person_DexHolder_DexHolderId",
            //            column: x => x.DexHolderId,
            //            principalTable: "DexHolder",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteText = table.Column<string>(type: "nvarchar(456)", maxLength: 456, nullable: false),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfo_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FullName",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFirst = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    NameMiddle = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    NameLast = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    PhNameFirst = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    PhNameMiddle = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    PhNameLast = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullName", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FullName_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecordCollector",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordCollector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecordCollector_Person_PersonId",
                        column: x => x.PersonId,
                        principalTable: "Person",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMedia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryField = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false),
                    SocialHandle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ContactInfoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialMedia_ContactInfo_ContactInfoId",
                        column: x => x.ContactInfoId,
                        principalTable: "ContactInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EntryItem",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ShortTitle = table.Column<string>(type: "nvarchar(240)", maxLength: 240, nullable: true),
                    FlavorText = table.Column<string>(type: "nvarchar(456)", maxLength: 456, nullable: false),
                    LogTimestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordCollectorId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntryItem_RecordCollector_RecordCollectorId",
                        column: x => x.RecordCollectorId,
                        principalTable: "RecordCollector",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetRoleClaims_RoleId",
            //    table: "AspNetRoleClaims",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "RoleNameIndex",
            //    table: "AspNetRoles",
            //    column: "NormalizedName",
            //    unique: true,
            //    filter: "[NormalizedName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserClaims_UserId",
            //    table: "AspNetUserClaims",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserLogins_UserId",
            //    table: "AspNetUserLogins",
            //    column: "UserId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUserRoles_RoleId",
            //    table: "AspNetUserRoles",
            //    column: "RoleId");

            //migrationBuilder.CreateIndex(
            //    name: "EmailIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedEmail");

            //migrationBuilder.CreateIndex(
            //    name: "IX_AspNetUsers_UserName",
            //    table: "AspNetUsers",
            //    column: "UserName",
            //    unique: true,
            //    filter: "[UserName] IS NOT NULL");

            //migrationBuilder.CreateIndex(
            //    name: "UserNameIndex",
            //    table: "AspNetUsers",
            //    column: "NormalizedUserName",
            //    unique: true,
            //    filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_PersonId",
                table: "ContactInfo",
                column: "PersonId",
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_DexHolder_ApplicationUserId",
            //    table: "DexHolder",
            //    column: "ApplicationUserId",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_DexHolder_ApplicationUserName",
            //    table: "DexHolder",
            //    column: "ApplicationUserName",
            //    unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EntryItem_RecordCollectorId",
                table: "EntryItem",
                column: "RecordCollectorId");

            migrationBuilder.CreateIndex(
                name: "IX_FullName_PersonId",
                table: "FullName",
                column: "PersonId",
                unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_Person_DexHolderId",
            //    table: "Person",
            //    column: "DexHolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Person_Nickname",
                table: "Person",
                column: "Nickname",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecordCollector_PersonId",
                table: "RecordCollector",
                column: "PersonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SocialMedia_ContactInfoId",
                table: "SocialMedia",
                column: "ContactInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "AspNetRoleClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserClaims");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserLogins");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserRoles");

            //migrationBuilder.DropTable(
            //    name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EntryItem");

            migrationBuilder.DropTable(
                name: "FullName");

            migrationBuilder.DropTable(
                name: "SocialMedia");

            //migrationBuilder.DropTable(
            //    name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "RecordCollector");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            //migrationBuilder.DropTable(
            //    name: "Person");

            //migrationBuilder.DropTable(
            //    name: "DexHolder");

            //migrationBuilder.DropTable(
            //    name: "AspNetUsers");
        }
    }
}
