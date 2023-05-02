using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMedicalRecordAPISystemTask.Migrations
{
    public partial class RemovingUnnecessaryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicUser");

            migrationBuilder.RenameColumn(
                name: "HumanInformationId",
                table: "Users",
                newName: "HumanInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HumanInfoId",
                table: "Users",
                newName: "HumanInformationId");

            migrationBuilder.CreateTable(
                name: "ClinicUser",
                columns: table => new
                {
                    ClinicsID = table.Column<int>(type: "int", nullable: false),
                    UsersId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicUser", x => new { x.ClinicsID, x.UsersId });
                    table.ForeignKey(
                        name: "FK_ClinicUser_Clinics_ClinicsID",
                        column: x => x.ClinicsID,
                        principalTable: "Clinics",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClinicUser_Users_UsersId",
                        column: x => x.UsersId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClinicUser_UsersId",
                table: "ClinicUser",
                column: "UsersId");
        }
    }
}
