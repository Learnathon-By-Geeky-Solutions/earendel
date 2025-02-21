using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentMesh.Migrations.PGSql.Job
{
    /// <inheritdoc />
    public partial class UpdatedJobSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Products",
                schema: "job",
                table: "Products");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "job",
                newName: "Jobs",
                newSchema: "job");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Jobs",
                schema: "job",
                table: "Jobs",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Jobs",
                schema: "job",
                table: "Jobs");

            migrationBuilder.RenameTable(
                name: "Jobs",
                schema: "job",
                newName: "Products",
                newSchema: "job");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Products",
                schema: "job",
                table: "Products",
                column: "Id");
        }
    }
}
