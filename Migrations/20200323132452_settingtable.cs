using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace blazorblog.Migrations
{
    public partial class settingtable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GlobalSetting",
                columns: table => new
                {
                    SettingId = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SettingName = table.Column<string>(nullable: true),
                    SettingValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GlobalSetting", x => x.SettingId);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "b5a53606-e711-44f2-a2e8-c428eb1114a5", "AQAAAAEAACcQAAAAEN4bg35aTB+Ii+VHhR7UHgaftswg80D5qwEufsN2ODugc3yD1sCh+Q+uKisvEMhvRA==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GlobalSetting");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "Users",
                keyColumn: "Id",
                keyValue: "a18be9c0-aa65-4af8-bd17-00bd9344e575",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9537f65c-2dca-4b59-92b7-ee7f714bec87", "AQAAAAEAACcQAAAAEFnKqqxnPENcGH3ibMAdKAY0YQ7/ggaR4K/Xu9dreJE4RYcalt8o+WocTtCYPhuw6g==" });
        }
    }
}
