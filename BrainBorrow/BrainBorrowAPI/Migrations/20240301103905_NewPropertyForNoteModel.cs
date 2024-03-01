using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrainBorrowAPI.Migrations
{
    /// <inheritdoc />
    public partial class NewPropertyForNoteModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "University",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "University",
                table: "Notes");
        }
    }
}
