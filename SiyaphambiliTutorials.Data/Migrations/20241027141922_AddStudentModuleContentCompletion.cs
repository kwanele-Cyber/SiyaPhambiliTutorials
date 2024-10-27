using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SiyaphambiliTutorials.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentModuleContentCompletion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "ModuleContents",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "StudentModuleContentCompletions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ModuleContentId = table.Column<int>(type: "int", nullable: false),
                    CompletionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentModuleContentCompletions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentModuleContentCompletions_ModuleContents_ModuleContentId",
                        column: x => x.ModuleContentId,
                        principalTable: "ModuleContents",
                        principalColumn: "ModuleContentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentModuleContentCompletions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentModuleContentCompletions_ModuleContentId",
                table: "StudentModuleContentCompletions",
                column: "ModuleContentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentModuleContentCompletions_StudentId",
                table: "StudentModuleContentCompletions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentModuleContentCompletions");

            migrationBuilder.DropColumn(
                name: "Completed",
                table: "ModuleContents");
        }
    }
}
