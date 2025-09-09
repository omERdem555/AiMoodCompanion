using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AiMoodCompanion.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMLTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 18, 29, 625, DateTimeKind.Utc).AddTicks(4618));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 18, 29, 625, DateTimeKind.Utc).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 18, 29, 625, DateTimeKind.Utc).AddTicks(4625));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 18, 29, 625, DateTimeKind.Utc).AddTicks(4626));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6417));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6497));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6500));
        }
    }
}
