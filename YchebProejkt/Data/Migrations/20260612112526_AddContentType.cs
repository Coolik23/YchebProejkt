using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YchebProejkt.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddContentType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Instructions",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Registries_ManagementId",
                table: "Registries",
                column: "ManagementId");

            migrationBuilder.CreateIndex(
                name: "IX_Instructions_RegistryId",
                table: "Instructions",
                column: "RegistryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Instructions_Registries_RegistryId",
                table: "Instructions",
                column: "RegistryId",
                principalTable: "Registries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Registries_Managements_ManagementId",
                table: "Registries",
                column: "ManagementId",
                principalTable: "Managements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Instructions_Registries_RegistryId",
                table: "Instructions");

            migrationBuilder.DropForeignKey(
                name: "FK_Registries_Managements_ManagementId",
                table: "Registries");

            migrationBuilder.DropIndex(
                name: "IX_Registries_ManagementId",
                table: "Registries");

            migrationBuilder.DropIndex(
                name: "IX_Instructions_RegistryId",
                table: "Instructions");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Instructions");
        }
    }
}
