using Microsoft.EntityFrameworkCore.Migrations;

namespace blazorblog.Migrations
{
    public partial class changetablename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Blogs_BlogId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Categories_CategoryId",
                table: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Comments",
                table: "Comments");

            migrationBuilder.RenameTable(
                name: "Comments",
                newName: "BlogCategories");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_CategoryId",
                table: "BlogCategories",
                newName: "IX_BlogCategories_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_BlogId",
                table: "BlogCategories",
                newName: "IX_BlogCategories_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories",
                column: "Id");

            // migrationBuilder.UpdateData(
            //     schema: "dbo",
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
            //     columns: new[] { "ConcurrencyStamp", "PasswordHash" },
            //     values: new object[] { "a77f527a-7bbb-45ab-afd4-67fbb401edd4", "AQAAAAEAACcQAAAAEL/ABDXpfw3KHw7legAdjW/rGi+4BASU2qzssm2rAaSS+TcdTm2dClgkgEMxTwzKvQ==" });

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCategories_Blogs_BlogId",
                table: "BlogCategories",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogCategories_Categories_CategoryId",
                table: "BlogCategories",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogCategories_Blogs_BlogId",
                table: "BlogCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_BlogCategories_Categories_CategoryId",
                table: "BlogCategories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BlogCategories",
                table: "BlogCategories");

            migrationBuilder.RenameTable(
                name: "BlogCategories",
                newName: "Comments");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCategories_CategoryId",
                table: "Comments",
                newName: "IX_Comments_CategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BlogCategories_BlogId",
                table: "Comments",
                newName: "IX_Comments_BlogId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Comments",
                table: "Comments",
                column: "Id");

            // migrationBuilder.UpdateData(
            //     schema: "dbo",
            //     table: "Users",
            //     keyColumn: "Id",
            //     keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
            //     columns: new[] { "ConcurrencyStamp", "PasswordHash" },
            //     values: new object[] { "efa6c379-20bf-49fd-ad58-24d9c65b9d50", "AQAAAAEAACcQAAAAEBo0W7mzfUXpFZoS9JZ4rogOMOST6ZWu77X95qAQ2hLoe1IiXndJ5lkc/U9b8GEVbg==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Blogs_BlogId",
                table: "Comments",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Categories_CategoryId",
                table: "Comments",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
