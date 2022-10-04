using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicEF.Migrations
{
    public partial class V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hobbies",
                columns: new[] { "Id", "Description" },
                values: new object[,]
                {
                    { -2, "Kanteklossen" },
                    { -1, "Sigarenbandjes" }
                });

            migrationBuilder.InsertData(
                table: "People",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { -2, "Marieke Klaasen" },
                    { -1, "Jan de Vries" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hobbies",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Hobbies",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "People",
                keyColumn: "Id",
                keyValue: -1);
        }
    }
}
