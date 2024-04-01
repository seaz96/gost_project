using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GostStorage.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrgBranchDocStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrgBranch",
                table: "DocStatistics",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrgBranch",
                table: "DocStatistics");
        }
    }
}
