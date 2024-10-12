using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GostStorage.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAcceptanceCommissionDatesToYears : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptanceDate",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "KeyPhrases",
                table: "Fields");

            migrationBuilder.RenameColumn(
                name: "CommissionDate",
                table: "Fields",
                newName: "LastEditTime");

            migrationBuilder.AddColumn<int>(
                name: "AcceptanceYear",
                table: "Fields",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CommissionYear",
                table: "Fields",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptanceYear",
                table: "Fields");

            migrationBuilder.DropColumn(
                name: "CommissionYear",
                table: "Fields");

            migrationBuilder.RenameColumn(
                name: "LastEditTime",
                table: "Fields",
                newName: "CommissionDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptanceDate",
                table: "Fields",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "KeyPhrases",
                table: "Fields",
                type: "text",
                nullable: true);
        }
    }
}
