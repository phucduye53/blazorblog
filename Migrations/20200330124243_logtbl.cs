using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace blazorblog.Migrations
{
    public partial class logtbl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Logs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    LogDate = table.Column<DateTime>(nullable: false),
                    LogAction = table.Column<string>(nullable: true),
                    LogUserName = table.Column<string>(nullable: true),
                    LogIpaddress = table.Column<string>(nullable: true),
                    LogExcuteStatus = table.Column<string>(nullable: true),
                    LogPerfTime = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logs", x => x.Id);
                });


        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Logs");

          
        }
    }
}
