using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class ConfigureEntityRelationshipsAndDecimalPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "PowerTimestamp",
                type: "decimal(18,10)",
                precision: 18,
                scale: 10,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<int>(
                name: "ApplicationId",
                table: "Files",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Applications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Applications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ParsedTimestamps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ProductionValue = table.Column<decimal>(type: "decimal(18,10)", precision: 18, scale: 10, nullable: false),
                    ConsumptionValue = table.Column<decimal>(type: "decimal(18,10)", precision: 18, scale: 10, nullable: false),
                    FileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParsedTimestamps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParsedTimestamps_Files_FileId",
                        column: x => x.FileId,
                        principalTable: "Files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Files_ApplicationId",
                table: "Files",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_ParsedTimestamps_FileId",
                table: "ParsedTimestamps",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Applications_ApplicationId",
                table: "Files",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Applications_ApplicationId",
                table: "Files");

            migrationBuilder.DropTable(
                name: "Applications");

            migrationBuilder.DropTable(
                name: "ParsedTimestamps");

            migrationBuilder.DropIndex(
                name: "IX_Files_ApplicationId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "ApplicationId",
                table: "Files");

            migrationBuilder.AlterColumn<decimal>(
                name: "Value",
                table: "PowerTimestamp",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)",
                oldPrecision: 18,
                oldScale: 10);
        }
    }
}
