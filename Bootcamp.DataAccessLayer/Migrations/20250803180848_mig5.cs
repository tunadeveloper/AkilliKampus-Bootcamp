using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bootcamp.DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class mig5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "CourseVideos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "CourseVideos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
