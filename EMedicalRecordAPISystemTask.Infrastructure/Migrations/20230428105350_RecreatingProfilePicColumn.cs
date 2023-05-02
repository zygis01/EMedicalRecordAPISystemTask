using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMedicalRecordAPISystemTask.Migrations
{
    public partial class RecreatingProfilePicColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "HumanInfos");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePic",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePic",
                table: "HumanInfos",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
