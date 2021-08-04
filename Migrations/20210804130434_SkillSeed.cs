using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_rpg.Migrations
{
    public partial class SkillSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "SkillId", "Damage", "Name" },
                values: new object[] { 1, 50, "Fireball" });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "SkillId", "Damage", "Name" },
                values: new object[] { 2, 40, "Lightning Strike" });

            migrationBuilder.InsertData(
                table: "Skill",
                columns: new[] { "SkillId", "Damage", "Name" },
                values: new object[] { 3, 30, "Boulder Throw" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skill",
                keyColumn: "SkillId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skill",
                keyColumn: "SkillId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skill",
                keyColumn: "SkillId",
                keyValue: 3);
        }
    }
}
