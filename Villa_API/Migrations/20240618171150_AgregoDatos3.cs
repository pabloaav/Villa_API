using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Villa_API.Migrations
{
    /// <inheritdoc />
    public partial class AgregoDatos3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Created_at", "Detalle", "ImagenUrl", "MetrosCuadrados", "Name", "Ocupantes", "Tarifa", "Updated_at" },
                values: new object[,]
                {
                    { 11, "Piscina, Wi-Fi, Aire acondicionado", new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7454), "Una hermosa villa con vista al mar", "http://ejemplo.com/imagen.jpg", 150, "Villa Maravilla", 4, 300.5, new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7467) },
                    { 12, "Jacuzzi, Sauna, Gimnasio, Wi-Fi", new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7471), "Un retiro tranquilo con acceso privado a la playa", "http://ejemplo.com/villa-solyplaya.jpg", 200, "Villa Sol y Playa", 6, 450.75, new DateTime(2024, 6, 18, 14, 11, 50, 220, DateTimeKind.Local).AddTicks(7471) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenidad", "Created_at", "Detalle", "ImagenUrl", "MetrosCuadrados", "Name", "Ocupantes", "Tarifa", "Updated_at" },
                values: new object[] { 1, "Piscina, Wi-Fi, Aire acondicionado", new DateTime(2024, 6, 18, 14, 10, 4, 916, DateTimeKind.Local).AddTicks(4462), "Una hermosa villa con vista al mar", "http://ejemplo.com/imagen.jpg", 150, "Villa Maravilla", 4, 300.5, new DateTime(2024, 6, 18, 14, 10, 4, 916, DateTimeKind.Local).AddTicks(4477) });
        }
    }
}
