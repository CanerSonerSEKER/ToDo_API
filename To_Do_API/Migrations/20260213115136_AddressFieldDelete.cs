using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace To_Do_API.Migrations
{
    /// <inheritdoc />
    public partial class AddressFieldDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
