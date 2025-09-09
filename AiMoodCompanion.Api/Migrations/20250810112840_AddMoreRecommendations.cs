using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AiMoodCompanion.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreRecommendations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(515));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(523));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(525));

            migrationBuilder.InsertData(
                table: "Recommendations",
                columns: new[] { "Id", "CreatedAt", "Description", "ExternalId", "Genre", "ImageUrl", "Title", "Type", "UserId", "Year" },
                values: new object[,]
                {
                    { 5, new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(527), "A legendary concierge and his young protégé embark on a series of adventures.", null, "Comedy", "https://example.com/grand-budapest.jpg", "The Grand Budapest Hotel", "Movie", null, 2014 },
                    { 6, new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(532), "A jazz pianist falls for an aspiring actress in Los Angeles.", null, "Romance", "https://example.com/lalaland.jpg", "La La Land", "Movie", null, 2016 },
                    { 7, new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(534), "A woman rebels against a tyrannical ruler in post-apocalyptic Australia.", null, "Action", "https://example.com/madmax.jpg", "Mad Max: Fury Road", "Movie", null, 2015 },
                    { 8, new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(536), "Paranormal investigators Ed and Lorraine Warren work to help a family terrorized by a dark presence.", null, "Horror", "https://example.com/conjuring.jpg", "The Conjuring", "Movie", null, 2013 },
                    { 9, new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(537), "Documentary series exploring the natural world and its wonders.", null, "Documentary", "https://example.com/planetearth.jpg", "Planet Earth II", "Series", null, 2016 },
                    { 10, new DateTime(2025, 8, 10, 11, 28, 40, 11, DateTimeKind.Utc).AddTicks(541), "An astronaut becomes stranded on Mars and must find a way to survive.", null, "Adventure", "https://example.com/martian.jpg", "The Martian", "Movie", null, 2015 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 20, 33, 458, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 20, 33, 458, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 20, 33, 458, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "Recommendations",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 10, 11, 20, 33, 458, DateTimeKind.Utc).AddTicks(9951));
        }
    }
}
