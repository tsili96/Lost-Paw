using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LostPaw.Migrations
{
    /// <inheritdoc />
    public partial class ShowPhoneNumberChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowPhone",
                table: "AspNetUsers",
                newName: "ShowPhoneNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShowPhoneNumber",
                table: "AspNetUsers",
                newName: "ShowPhone");
        }
    }
}
