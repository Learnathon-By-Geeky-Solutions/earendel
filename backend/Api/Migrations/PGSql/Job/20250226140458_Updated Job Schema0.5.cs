using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentMesh.Migrations.PGSql.Job
{
    /// <inheritdoc />
    public partial class UpdatedJobSchema05 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobApplications_Jobs_JobId1",
                schema: "job",
                table: "JobApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobApplications_JobId1",
                schema: "job",
                table: "JobApplications");

            migrationBuilder.DropColumn(
                name: "JobId1",
                schema: "job",
                table: "JobApplications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "JobId1",
                schema: "job",
                table: "JobApplications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_JobApplications_JobId1",
                schema: "job",
                table: "JobApplications",
                column: "JobId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JobApplications_Jobs_JobId1",
                schema: "job",
                table: "JobApplications",
                column: "JobId1",
                principalSchema: "job",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
