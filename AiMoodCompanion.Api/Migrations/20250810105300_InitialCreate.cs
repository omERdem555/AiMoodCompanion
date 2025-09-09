using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AiMoodCompanion.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    ProfilePicture = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MoodAnalyses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    InputText = table.Column<string>(type: "text", nullable: false),
                    DetectedMood = table.Column<string>(type: "text", nullable: false),
                    MoodScore = table.Column<double>(type: "double precision", nullable: false),
                    Keywords = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MoodAnalyses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MoodAnalyses_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Recommendations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Genre = table.Column<string>(type: "text", nullable: true),
                    Year = table.Column<int>(type: "integer", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    ExternalId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recommendations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recommendations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserReactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RecommendationId = table.Column<int>(type: "integer", nullable: false),
                    ReactionType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserReactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserReactions_Recommendations_RecommendationId",
                        column: x => x.RecommendationId,
                        principalTable: "Recommendations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserReactions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Recommendations",
                columns: new[] { "Id", "CreatedAt", "Description", "ExternalId", "Genre", "ImageUrl", "Title", "Type", "UserId", "Year" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6417), "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.", null, "Drama", "https://example.com/shawshank.jpg", "The Shawshank Redemption", "Movie", null, 1994 },
                    { 2, new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6423), "A meek Hobbit from the Shire and eight companions set out on a journey to destroy the powerful One Ring.", null, "Fantasy", "https://example.com/lotr.jpg", "The Lord of the Rings", "Movie", null, 2001 },
                    { 3, new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6497), "A dystopian novel about a totalitarian society where Big Brother is always watching.", null, "Dystopian", "https://example.com/1984.jpg", "1984", "Book", null, 1949 },
                    { 4, new DateTime(2025, 8, 10, 10, 53, 0, 18, DateTimeKind.Utc).AddTicks(6500), "A high school chemistry teacher turned methamphetamine manufacturer partners with a former student.", null, "Crime", "https://example.com/breaking-bad.jpg", "Breaking Bad", "Series", null, 2008 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_MoodAnalyses_UserId",
                table: "MoodAnalyses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Recommendations_UserId",
                table: "Recommendations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReactions_RecommendationId",
                table: "UserReactions",
                column: "RecommendationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReactions_UserId",
                table: "UserReactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MoodAnalyses");

            migrationBuilder.DropTable(
                name: "UserReactions");

            migrationBuilder.DropTable(
                name: "Recommendations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
