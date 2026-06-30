using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YchebProejkt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFileHashToInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileHash",
                table: "Instructions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileHash",
                table: "Instructions");
        }
    }
}
