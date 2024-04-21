using Microsoft.EntityFrameworkCore.Migrations;

namespace Recipe_Sharing_Platform.Migrations
{
    public partial class UpdateRecipeTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Recipes_RecipeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RecipeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RecipeId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RecipeId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RecipeId",
                table: "AspNetUsers",
                column: "RecipeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Recipes_RecipeId",
                table: "AspNetUsers",
                column: "RecipeId",
                principalTable: "Recipes",
                principalColumn: "RecipeId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
