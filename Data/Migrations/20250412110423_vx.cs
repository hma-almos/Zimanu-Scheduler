using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication5.Data.Migrations
{
    /// <inheritdoc />
    public partial class vx : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dayes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DayName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dayes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stages = table.Column<int>(type: "int", nullable: false),
                    Room = table.Column<int>(type: "int", nullable: false),
                    hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lectures", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rooms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleHours",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "stages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Groups = table.Column<int>(type: "int", nullable: false),
                    Division = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teachers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teachers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "teacher_Lectures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Teacher_Id = table.Column<int>(type: "int", nullable: false),
                    Lecture_Id = table.Column<int>(type: "int", nullable: false),
                    theory = table.Column<bool>(type: "bit", nullable: false),
                    practical = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_teacher_Lectures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_teacher_Lectures_Lectures_Lecture_Id",
                        column: x => x.Lecture_Id,
                        principalTable: "Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_teacher_Lectures_teachers_Teacher_Id",
                        column: x => x.Teacher_Id,
                        principalTable: "teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "finalSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Lec_id = table.Column<int>(type: "int", nullable: false),
                    Hour_id = table.Column<int>(type: "int", nullable: false),
                    Day_id = table.Column<int>(type: "int", nullable: false),
                    Room_id = table.Column<int>(type: "int", nullable: false),
                    Stage_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_finalSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_finalSchedules_ScheduleHours_Hour_id",
                        column: x => x.Hour_id,
                        principalTable: "ScheduleHours",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_finalSchedules_dayes_Day_id",
                        column: x => x.Day_id,
                        principalTable: "dayes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_finalSchedules_rooms_Room_id",
                        column: x => x.Room_id,
                        principalTable: "rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_finalSchedules_stages_Stage_id",
                        column: x => x.Stage_id,
                        principalTable: "stages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_finalSchedules_teacher_Lectures_Lec_id",
                        column: x => x.Lec_id,
                        principalTable: "teacher_Lectures",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_finalSchedules_Day_id",
                table: "finalSchedules",
                column: "Day_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalSchedules_Hour_id",
                table: "finalSchedules",
                column: "Hour_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalSchedules_Lec_id",
                table: "finalSchedules",
                column: "Lec_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalSchedules_Room_id",
                table: "finalSchedules",
                column: "Room_id");

            migrationBuilder.CreateIndex(
                name: "IX_finalSchedules_Stage_id",
                table: "finalSchedules",
                column: "Stage_id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_Lectures_Lecture_Id",
                table: "teacher_Lectures",
                column: "Lecture_Id");

            migrationBuilder.CreateIndex(
                name: "IX_teacher_Lectures_Teacher_Id",
                table: "teacher_Lectures",
                column: "Teacher_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "finalSchedules");

            migrationBuilder.DropTable(
                name: "ScheduleHours");

            migrationBuilder.DropTable(
                name: "dayes");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "stages");

            migrationBuilder.DropTable(
                name: "teacher_Lectures");

            migrationBuilder.DropTable(
                name: "Lectures");

            migrationBuilder.DropTable(
                name: "teachers");
        }
    }
}
