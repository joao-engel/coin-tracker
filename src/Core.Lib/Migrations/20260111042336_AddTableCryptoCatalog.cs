using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Core.Lib.Migrations
{
    /// <inheritdoc />
    public partial class AddTableCryptoCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CryptoCatalog",
                schema: "Crypto",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Key = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Symbol = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CryptoCatalog", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "Crypto",
                table: "CryptoCatalog",
                columns: new[] { "Id", "CreatedAt", "DisplayName", "IsActive", "Key", "Symbol" },
                values: new object[,]
                {
                    { new Guid("a5e2f7b8-9c1d-4e2a-8f3b-6c4d9e0a1f2b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ethereum", true, "eth", "ETHUSDT" },
                    { new Guid("c1b3d4e5-6f7a-8b9c-0d1e-2f3a4b5c6d7e"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dogecoin", true, "doge", "DOGEUSDT" },
                    { new Guid("d8d64c02-4638-4e6f-80d5-53305047f3b1"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Bitcoin", true, "btc", "BTCUSDT" },
                    { new Guid("e1d2c3b4-a5f6-7e8d-9c0b-1a2f3e4d5c6b"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "XRP", true, "xrp", "XRPUSDT" },
                    { new Guid("f9e8d7c6-b5a4-3f2e-1d0c-9b8a7f6e5d4c"), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Solana", true, "sol", "SOLUSDT" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CryptoCatalog_DisplayName",
                schema: "Crypto",
                table: "CryptoCatalog",
                column: "DisplayName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CryptoCatalog_Key",
                schema: "Crypto",
                table: "CryptoCatalog",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CryptoCatalog_Symbol",
                schema: "Crypto",
                table: "CryptoCatalog",
                column: "Symbol",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CryptoCatalog",
                schema: "Crypto");
        }
    }
}
