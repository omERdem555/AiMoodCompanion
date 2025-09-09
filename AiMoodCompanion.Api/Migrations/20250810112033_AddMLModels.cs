using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace AiMoodCompanion.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddMLModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ModelVersions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Version = table.Column<string>(type: "text", nullable: false),
                    ModelPath = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Accuracy = table.Column<double>(type: "double precision", nullable: false),
                    Precision = table.Column<double>(type: "double precision", nullable: false),
                    Recall = table.Column<double>(type: "double precision", nullable: false),
                    TrainingDataCount = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeployedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModelVersions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainingData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    InputText = table.Column<string>(type: "text", nullable: false),
                    DetectedMood = table.Column<string>(type: "text", nullable: false),
                    MoodScore = table.Column<double>(type: "double precision", nullable: false),
                    Keywords = table.Column<string>(type: "text", nullable: true),
                    UserFeedback = table.Column<string>(type: "text", nullable: true),
                    IsUsedForTraining = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UsedForTrainingAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainingData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrainingData_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_TrainingData_UserId",
                table: "TrainingData",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModelVersions");

            migrationBuilder.DropTable(
                name: "TrainingData");

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
    }
}
