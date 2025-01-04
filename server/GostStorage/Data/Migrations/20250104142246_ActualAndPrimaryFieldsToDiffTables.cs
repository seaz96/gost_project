using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GostStorage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ActualAndPrimaryFieldsToDiffTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fields");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Documents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ActualFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    CodeOks = table.Column<string>(type: "text", nullable: true),
                    ActivityField = table.Column<string>(type: "text", nullable: true),
                    AcceptanceYear = table.Column<int>(type: "integer", nullable: true),
                    CommissionYear = table.Column<int>(type: "integer", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    AcceptedFirstTimeOrReplaced = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    KeyWords = table.Column<string>(type: "text", nullable: true),
                    ApplicationArea = table.Column<string>(type: "text", nullable: true),
                    AdoptionLevel = table.Column<int>(type: "integer", nullable: true),
                    DocumentText = table.Column<string>(type: "text", nullable: true),
                    Changes = table.Column<string>(type: "text", nullable: true),
                    Amendments = table.Column<string>(type: "text", nullable: true),
                    Harmonization = table.Column<int>(type: "integer", nullable: true),
                    DocId = table.Column<long>(type: "bigint", nullable: false),
                    LastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActualFields", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PrimaryFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    CodeOks = table.Column<string>(type: "text", nullable: true),
                    ActivityField = table.Column<string>(type: "text", nullable: true),
                    AcceptanceYear = table.Column<int>(type: "integer", nullable: true),
                    CommissionYear = table.Column<int>(type: "integer", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    AcceptedFirstTimeOrReplaced = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    KeyWords = table.Column<string>(type: "text", nullable: true),
                    ApplicationArea = table.Column<string>(type: "text", nullable: true),
                    AdoptionLevel = table.Column<int>(type: "integer", nullable: true),
                    DocumentText = table.Column<string>(type: "text", nullable: true),
                    Changes = table.Column<string>(type: "text", nullable: true),
                    Amendments = table.Column<string>(type: "text", nullable: true),
                    Harmonization = table.Column<int>(type: "integer", nullable: true),
                    DocId = table.Column<long>(type: "bigint", nullable: false),
                    LastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrimaryFields", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActualFields");

            migrationBuilder.DropTable(
                name: "PrimaryFields");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Documents");

            migrationBuilder.CreateTable(
                name: "Fields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AcceptanceYear = table.Column<int>(type: "integer", nullable: true),
                    AcceptedFirstTimeOrReplaced = table.Column<string>(type: "text", nullable: true),
                    ActivityField = table.Column<string>(type: "text", nullable: true),
                    AdoptionLevel = table.Column<int>(type: "integer", nullable: true),
                    Amendments = table.Column<string>(type: "text", nullable: true),
                    ApplicationArea = table.Column<string>(type: "text", nullable: true),
                    Author = table.Column<string>(type: "text", nullable: true),
                    Changes = table.Column<string>(type: "text", nullable: true),
                    CodeOks = table.Column<string>(type: "text", nullable: true),
                    CommissionYear = table.Column<int>(type: "integer", nullable: true),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Designation = table.Column<string>(type: "text", nullable: false),
                    DocId = table.Column<long>(type: "bigint", nullable: false),
                    DocumentText = table.Column<string>(type: "text", nullable: true),
                    FullName = table.Column<string>(type: "text", nullable: true),
                    Harmonization = table.Column<int>(type: "integer", nullable: true),
                    IsPrimary = table.Column<bool>(type: "boolean", nullable: false),
                    KeyWords = table.Column<string>(type: "text", nullable: true),
                    LastEditTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fields", x => x.Id);
                });
        }
    }
}
