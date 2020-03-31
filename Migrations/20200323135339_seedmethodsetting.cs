using Microsoft.EntityFrameworkCore.Migrations;

namespace blazorblog.Migrations
{
    public partial class seedmethodsetting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GlobalSetting",
                columns: new[] { "SettingId", "SettingName", "SettingValue" },
                values: new object[,]
                {
                    { 1, "ApplicationName", "tlq" },
                    { 2, "SMTPServer", "test" },
                    { 3, "SMTPSecure", "test" },
                    { 4, "SMTPUserName", "test" },
                    { 5, "SMTPPassword", "test" },
                    { 6, "SMTPAuthendication", "1" },
                    { 7, "SMTPFromEmail", "True" },
                    { 8, "ApplicationLogo", @"\uploads\0ktgjc50_400x400.jpg" },
                    { 9, "ApplicationHeader", "<p>True</p>" },
                    { 10, "DisqusEnabled", "True" },
                    { 11, "DisqusShortName", "tlqblog" }
                });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "GlobalSetting",
                keyColumn: "SettingId",
                keyValue: 11);

        }
    }
}
