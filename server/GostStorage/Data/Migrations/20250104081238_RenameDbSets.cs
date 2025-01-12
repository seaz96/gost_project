using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GostStorage.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameDbSets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_DocStatistics",
                table: "DocStatistics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DocsReferences",
                table: "DocsReferences");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Docs",
                table: "Docs");

            migrationBuilder.RenameTable(
                name: "DocStatistics",
                newName: "UserActions");

            migrationBuilder.RenameTable(
                name: "DocsReferences",
                newName: "References");

            migrationBuilder.RenameTable(
                name: "Docs",
                newName: "Documents");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserActions",
                table: "UserActions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_References",
                table: "References",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                table: "Documents",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserActions",
                table: "UserActions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_References",
                table: "References");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "UserActions",
                newName: "DocStatistics");

            migrationBuilder.RenameTable(
                name: "References",
                newName: "DocsReferences");

            migrationBuilder.RenameTable(
                name: "Documents",
                newName: "Docs");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocStatistics",
                table: "DocStatistics",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DocsReferences",
                table: "DocsReferences",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Docs",
                table: "Docs",
                column: "Id");
        }
    }
}
