using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace POE_PART_3_ST0084433.Data.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserData",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    moduleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    moduleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    numberOfCredits = table.Column<int>(type: "int", nullable: false),
                    classHours = table.Column<int>(type: "int", nullable: false),
                    semesterName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    semesterDuration = table.Column<int>(type: "int", nullable: false),
                    semesterDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserData", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserData");
        }
    }
}
