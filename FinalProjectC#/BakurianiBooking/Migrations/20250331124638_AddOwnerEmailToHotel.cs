using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BakurianiBooking.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerEmailToHotel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OwnerEmail",
                table: "Hotels",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OwnerEmail",
                table: "Hotels");
        }
    }
}
