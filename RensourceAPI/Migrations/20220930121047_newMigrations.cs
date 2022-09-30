using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RensourceAPI.Migrations
{
    public partial class newMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutiveTeam_ExecutiveTeamCategory_ExecutiveTeamCategoryId",
                table: "ExecutiveTeam");

            migrationBuilder.AddColumn<string>(
                name: "SinglePageTitle",
                table: "Projects",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "PressRelease",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "ExecutiveTeamCategoryId",
                table: "ExecutiveTeam",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutiveTeam_ExecutiveTeamCategory_ExecutiveTeamCategoryId",
                table: "ExecutiveTeam",
                column: "ExecutiveTeamCategoryId",
                principalTable: "ExecutiveTeamCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutiveTeam_ExecutiveTeamCategory_ExecutiveTeamCategoryId",
                table: "ExecutiveTeam");

            migrationBuilder.DropColumn(
                name: "SinglePageTitle",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "PressRelease");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExecutiveTeamCategoryId",
                table: "ExecutiveTeam",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutiveTeam_ExecutiveTeamCategory_ExecutiveTeamCategoryId",
                table: "ExecutiveTeam",
                column: "ExecutiveTeamCategoryId",
                principalTable: "ExecutiveTeamCategory",
                principalColumn: "Id");
        }
    }
}
