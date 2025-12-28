using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Core.Lib.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Crypto");

            migrationBuilder.CreateTable(
                name: "MarketHistory",
                schema: "Crypto",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Coin = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,8)", precision: 18, scale: 8, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketHistory", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketHistory_Coin",
                schema: "Crypto",
                table: "MarketHistory",
                column: "Coin");

            migrationBuilder.CreateIndex(
                name: "IX_MarketHistory_Timestamp",
                schema: "Crypto",
                table: "MarketHistory",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketHistory",
                schema: "Crypto");
        }
    }
}
