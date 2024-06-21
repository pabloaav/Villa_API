using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Villa_API.Migrations
{
    /// <inheritdoc />
    public partial class AddTablaNumeroVillas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumeroVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetalleEspecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumeroVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumeroVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Created_at", "Updated_at" },
                values: new object[] { new DateTime(2024, 6, 20, 20, 43, 9, 103, DateTimeKind.Local).AddTicks(284), new DateTime(2024, 6, 20, 20, 43, 9, 103, DateTimeKind.Local).AddTicks(325) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Created_at", "Updated_at" },
                values: new object[] { new DateTime(2024, 6, 20, 20, 43, 9, 103, DateTimeKind.Local).AddTicks(328), new DateTime(2024, 6, 20, 20, 43, 9, 103, DateTimeKind.Local).AddTicks(329) });

            migrationBuilder.CreateIndex(
                name: "IX_NumeroVillas_VillaId",
                table: "NumeroVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumeroVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Created_at", "Updated_at" },
                values: new object[] { new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7454), new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7467) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Created_at", "Updated_at" },
                values: new object[] { new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7471), new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7471) });
        }
    }
}
