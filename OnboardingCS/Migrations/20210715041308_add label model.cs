using Microsoft.EntityFrameworkCore.Migrations;

namespace OnboardingCS.Migrations
{
    public partial class addlabelmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TodoIsDone",
                table: "Labels");

            migrationBuilder.DropColumn(
                name: "TodoName",
                table: "Labels");

            migrationBuilder.RenameColumn(
                name: "TodoId",
                table: "Labels",
                newName: "labelId");

            migrationBuilder.AddColumn<string>(
                name: "labelName",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "TodoItem",
                columns: table => new
                {
                    TodoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TodoName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TodoIsDone = table.Column<bool>(type: "bit", nullable: false),
                    labelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TodoItem", x => x.TodoId);
                    table.ForeignKey(
                        name: "FK_TodoItem_Labels_labelId",
                        column: x => x.labelId,
                        principalTable: "Labels",
                        principalColumn: "labelId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TodoItem_labelId",
                table: "TodoItem",
                column: "labelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TodoItem");

            migrationBuilder.DropColumn(
                name: "labelName",
                table: "Labels");

            migrationBuilder.RenameColumn(
                name: "labelId",
                table: "Labels",
                newName: "TodoId");

            migrationBuilder.AddColumn<bool>(
                name: "TodoIsDone",
                table: "Labels",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "TodoName",
                table: "Labels",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
