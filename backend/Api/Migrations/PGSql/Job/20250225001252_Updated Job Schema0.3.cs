using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentMesh.Migrations.PGSql.Job
{
    /// <inheritdoc />
    public partial class UpdatedJobSchema03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExperienceLevel",
                schema: "job",
                table: "Jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "JobType",
                schema: "job",
                table: "Jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                schema: "job",
                table: "Jobs",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Requirments",
                schema: "job",
                table: "Jobs",
                type: "character varying(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExperienceLevel",
                schema: "job",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "JobType",
                schema: "job",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Location",
                schema: "job",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "Requirments",
                schema: "job",
                table: "Jobs");
        }
    }
}
