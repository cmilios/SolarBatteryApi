using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConcurrencyCalculations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConcurrencyMetric = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConcurrencyCalculations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PowerTimestamp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConcurrencyCalculationId = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PowerTimestamp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PowerTimestamp_ConcurrencyCalculations_ConcurrencyCalculationId",
                        column: x => x.ConcurrencyCalculationId,
                        principalTable: "ConcurrencyCalculations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerTimestamp_ConcurrencyCalculationId",
                table: "PowerTimestamp",
                column: "ConcurrencyCalculationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PowerTimestamp");

            migrationBuilder.DropTable(
                name: "ConcurrencyCalculations");
        }
    }
}
