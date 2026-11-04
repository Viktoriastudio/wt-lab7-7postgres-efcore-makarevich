using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace wt_lab6_6minimalapi_makarevich.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Project = table.Column<string>(type: "text", nullable: false),
                    Assignee = table.Column<string>(type: "text", nullable: false),
                    EstimatedHours = table.Column<int>(type: "integer", nullable: false),
                    IsCompleted = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Assignee", "CreatedAt", "EstimatedHours", "IsCompleted", "Project", "Title" },
                values: new object[,]
                {
                    { 1, "Ольга Смирнова", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 16, true, "CRM для банка", "Сбор требований" },
                    { 2, "Иван Петров", new DateTime(2026, 1, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), 24, true, "CRM для банка", "Проектирование БД" },
                    { 3, "Иван Петров", new DateTime(2026, 1, 3, 0, 0, 0, 0, DateTimeKind.Unspecified), 40, false, "CRM для банка", "Разработка API" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Tasks");
        }
    }
}
