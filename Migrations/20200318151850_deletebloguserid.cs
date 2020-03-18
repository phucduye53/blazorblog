using Microsoft.EntityFrameworkCore.Migrations;

namespace blazorblog.Migrations
{
    public partial class deletebloguserid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Blogs");

            // migrationBuilder.UpdateData(
            //     schema: "dbo",
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
            //     columns: new[] { "ConcurrencyStamp", "PasswordHash" },
            //     values: new object[] { "9537f65c-2dca-4b59-92b7-ee7f714bec87", "AQAAAAEAACcQAAAAEFnKqqxnPENcGH3ibMAdKAY0YQ7/ggaR4K/Xu9dreJE4RYcalt8o+WocTtCYPhuw6g==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Blogs",
                type: "text",
                nullable: true);

            // migrationBuilder.UpdateData(
            //     schema: "dbo",
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
            //     columns: new[] { "ConcurrencyStamp", "PasswordHash" },
            //     values: new object[] { "a77f527a-7bbb-45ab-afd4-67fbb401edd4", "AQAAAAEAACcQAAAAEL/ABDXpfw3KHw7legAdjW/rGi+4BASU2qzssm2rAaSS+TcdTm2dClgkgEMxTwzKvQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_UserId",
                table: "Blogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Users_UserId",
                table: "Blogs",
                column: "UserId",
                principalSchema: "dbo",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
