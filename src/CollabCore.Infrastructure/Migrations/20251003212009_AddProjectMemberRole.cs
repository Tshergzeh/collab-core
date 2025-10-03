using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CollabCore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProjectMemberRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "ProjectMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "ProjectMembers");
        }
    }
}
