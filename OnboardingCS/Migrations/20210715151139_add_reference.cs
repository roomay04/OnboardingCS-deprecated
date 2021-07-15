using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace OnboardingCS.Migrations
{
    public partial class add_reference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "LabelId",
                table: "TodoItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_LabelId",
                table: "TodoItems",
                column: "LabelId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_Labels_LabelId",
                table: "TodoItems",
                column: "LabelId",
                principalTable: "Labels",
                principalColumn: "LabelId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_Labels_LabelId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_LabelId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "LabelId",
                table: "TodoItems");
        }
    }
}
