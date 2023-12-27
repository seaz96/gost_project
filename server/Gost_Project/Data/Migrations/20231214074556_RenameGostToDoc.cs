using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Gost_Project.Data.Migrations
{
    /// <inheritdoc />
    public partial class RenameGostToDoc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentalGostId",
                table: "NormativeReferences",
                newName: "ParentalDocId");

            migrationBuilder.RenameColumn(
                name: "ChildGostId",
                table: "NormativeReferences",
                newName: "ChildDocId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ParentalDocId",
                table: "NormativeReferences",
                newName: "ParentalGostId");

            migrationBuilder.RenameColumn(
                name: "ChildDocId",
                table: "NormativeReferences",
                newName: "ChildGostId");
        }
    }
}
