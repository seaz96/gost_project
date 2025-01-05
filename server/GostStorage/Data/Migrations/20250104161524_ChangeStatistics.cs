using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GostStorage.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Action",
                table: "UserActions",
                newName: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "UserActions",
                newName: "Action");
        }
    }
}
