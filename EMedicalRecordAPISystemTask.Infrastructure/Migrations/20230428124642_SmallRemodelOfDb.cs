using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMedicalRecordAPISystemTask.Migrations
{
    public partial class SmallRemodelOfDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HumanInfoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "Users");

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePic",
                table: "HumanInfos",
                type: "varbinary(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "HumanInfos");

            migrationBuilder.AddColumn<int>(
                name: "HumanInfoId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePic",
                table: "Users",
                type: "varbinary(max)",
                nullable: true);
        }
    }
}
