using Microsoft.EntityFrameworkCore.Migrations;

namespace Mentor.Migrations
{
    public partial class removeNoOfRoomsFromHostelTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfRoom",
                table: "Hostels");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfRoom",
                table: "Hostels",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
