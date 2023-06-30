using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REACTBIGBANG.Migrations
{
    /// <inheritdoc />
    public partial class Start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admins",
                columns: table => new
                {
                    Admin_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Admin_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Admin_password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_admins", x => x.Admin_id);
                });

            migrationBuilder.CreateTable(
                name: "doctors",
                columns: table => new
                {
                    Doctor_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "101, 1"),
                    Doctor_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Specialization = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Doctor_experience = table.Column<int>(type: "int", nullable: false),
                    Doctor_password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctors", x => x.Doctor_id);
                });

            migrationBuilder.CreateTable(
                name: "patients",
                columns: table => new
                {
                    Patient_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Patient_name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patient_age = table.Column<int>(type: "int", nullable: false),
                    Patient_gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Medical_treatment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Patient_Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phonenumber = table.Column<long>(type: "bigint", nullable: false),
                    Patient_password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DoctorsDoctor_id = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_patients", x => x.Patient_id);
                    table.ForeignKey(
                        name: "FK_patients_doctors_DoctorsDoctor_id",
                        column: x => x.DoctorsDoctor_id,
                        principalTable: "doctors",
                        principalColumn: "Doctor_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_patients_DoctorsDoctor_id",
                table: "patients",
                column: "DoctorsDoctor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admins");

            migrationBuilder.DropTable(
                name: "patients");

            migrationBuilder.DropTable(
                name: "doctors");
        }
    }
}
