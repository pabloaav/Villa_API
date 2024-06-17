using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Villa_API.Migrations
{
    /// <inheritdoc />
    public partial class AlimentarTablaVilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Created_at", "Detalle", "ImagenUrl", "MetrosCuadrados", "Name", "Ocupantes", "Tarifa", "Updated_at" },
                values: new object[] { 1, "Piscina, Wi-Fi, Aire acondicionado", new DateTime(2024, 6, 17, 11, 49, 57, 945, DateTimeKind.Local).AddTicks(9358), "Una hermosa villa con vista al mar", "http://ejemplo.com/imagen.jpg", 150, "Villa Maravilla", 4, 300.5, new DateTime(2024, 6, 17, 11, 49, 57, 945, DateTimeKind.Local).AddTicks(9373) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Created_at", "Detalle", "ImagenUrl", "MetrosCuadrados", "Name", "Ocupantes", "Tarifa", "Updated_at" },
                values: new object[] { -1, "Piscina, Wi-Fi, Aire acondicionado", new DateTime(2024, 6, 17, 11, 40, 51, 852, DateTimeKind.Local).AddTicks(9064), "Una hermosa villa con vista al mar", "http://ejemplo.com/imagen.jpg", 150, "Villa Maravilla", 4, 300.5, new DateTime(2024, 6, 17, 11, 40, 51, 852, DateTimeKind.Local).AddTicks(9078) });
        }
    }
}
