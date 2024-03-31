using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gost_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDocIdToFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DocId",
                table: "Fields",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocId",
                table: "Fields");
        }
    }
}
