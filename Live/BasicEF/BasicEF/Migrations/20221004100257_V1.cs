using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BasicEF.Migrations
{
    public partial class V1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HobbyPerson_Mensen_PeopleId",
                table: "HobbyPerson");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Mensen",
                schema: "dbo",
                table: "Mensen");

            migrationBuilder.RenameTable(
                name: "Mensen",
                schema: "dbo",
                newName: "People");

            migrationBuilder.RenameIndex(
                name: "IX_Mensen_Name",
                table: "People",
                newName: "IX_People_Name");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Hobbies",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_People",
                table: "People",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Hobbies_Description",
                table: "Hobbies",
                column: "Description");

            migrationBuilder.AddForeignKey(
                name: "FK_HobbyPerson_People_PeopleId",
                table: "HobbyPerson",
                column: "PeopleId",
                principalTable: "People",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HobbyPerson_People_PeopleId",
                table: "HobbyPerson");

            migrationBuilder.DropIndex(
                name: "IX_Hobbies_Description",
                table: "Hobbies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_People",
                table: "People");

            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.RenameTable(
                name: "People",
                newName: "Mensen",
                newSchema: "dbo");

            migrationBuilder.RenameIndex(
                name: "IX_People_Name",
                schema: "dbo",
                table: "Mensen",
                newName: "IX_Mensen_Name");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Hobbies",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Mensen",
                schema: "dbo",
                table: "Mensen",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HobbyPerson_Mensen_PeopleId",
                table: "HobbyPerson",
                column: "PeopleId",
                principalSchema: "dbo",
                principalTable: "Mensen",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
