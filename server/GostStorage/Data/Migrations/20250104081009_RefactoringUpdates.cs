using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GostStorage.Data.Migrations
{
    /// <inheritdoc />
    public partial class RefactoringUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CodeOKS",
                table: "Fields",
                newName: "CodeOks");

            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "Fields",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ActualFieldId",
                table: "Docs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Designation",
                table: "Docs",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Designation",
                table: "Docs");

            migrationBuilder.RenameColumn(
                name: "CodeOks",
                table: "Fields",
                newName: "CodeOKS");

            migrationBuilder.AlterColumn<string>(
                name: "Designation",
                table: "Fields",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<long>(
                name: "ActualFieldId",
                table: "Docs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");
        }
    }
}
