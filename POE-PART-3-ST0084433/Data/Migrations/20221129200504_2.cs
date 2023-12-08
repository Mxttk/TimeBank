using Microsoft.EntityFrameworkCore.Migrations;

namespace POE_PART_3_ST0084433.Data.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SelfStudy",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    moduleCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    selfStudyHours = table.Column<int>(type: "int", nullable: false),
                    studyHoursRemaining = table.Column<int>(type: "int", nullable: false),
                    studyDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    studyDuration = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SelfStudy", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SelfStudy");
        }
    }
}
