using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gost_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Changed",
                table: "DocStatistics");

            migrationBuilder.RenameColumn(
                name: "Views",
                table: "DocStatistics",
                newName: "Action");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "DocStatistics",
                newName: "Date");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "DocStatistics",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "DocStatistics");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "DocStatistics",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "Action",
                table: "DocStatistics",
                newName: "Views");

            migrationBuilder.AddColumn<DateTime>(
                name: "Changed",
                table: "DocStatistics",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
