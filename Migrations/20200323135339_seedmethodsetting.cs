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
                    { 1, "ApplicationName", "True" },
                    { 2, "SMTPServer", "True" },
                    { 3, "SMTPSecure", "True" },
                    { 4, "SMTPUserName", "True" },
                    { 5, "SMTPPassword", "True" },
                    { 6, "SMTPAuthendication", "True" },
                    { 7, "SMTPFromEmail", "True" },
                    { 8, "ApplicationLogo", "uploads\\logo.png" },
                    { 9, "ApplicationHeader", "True" },
                    { 10, "DisqusEnabled", "True" },
                    { 11, "DisqusShortName", "True" }
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
