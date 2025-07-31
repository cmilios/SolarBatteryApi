using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SPCS.Data.Migrations
{
    /// <inheritdoc />
    public partial class FileInfra : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "PowerTimestamp",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PowerTimestamp_FileId",
                table: "PowerTimestamp",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_PowerTimestamp_Files_FileId",
                table: "PowerTimestamp",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PowerTimestamp_Files_FileId",
                table: "PowerTimestamp");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Files");

            migrationBuilder.DropIndex(
                name: "IX_PowerTimestamp_FileId",
                table: "PowerTimestamp");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "PowerTimestamp");
        }
    }
}
