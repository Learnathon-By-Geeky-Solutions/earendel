using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TalentMesh.Migrations.PGSql.Experties
{
    /// <inheritdoc />
    public partial class AddExperties99Schema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "experties");

            migrationBuilder.CreateTable(
                name: "Seniorities",
                schema: "experties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seniorities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                schema: "experties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeniorityLevelJunctions",
                schema: "experties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: false),
                    SeniorityLevelId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeniorityLevelJunctions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeniorityLevelJunctions_Seniorities_SeniorityLevelId",
                        column: x => x.SeniorityLevelId,
                        principalSchema: "experties",
                        principalTable: "Seniorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeniorityLevelJunctions_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "experties",
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SubSkills",
                schema: "experties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SkillId = table.Column<Guid>(type: "uuid", nullable: true),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubSkills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalSchema: "experties",
                        principalTable: "Skills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Rubrics",
                schema: "experties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    RubricDescription = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    SubSkillId = table.Column<Guid>(type: "uuid", nullable: true),
                    SeniorityLevelId = table.Column<Guid>(type: "uuid", nullable: true),
                    Weight = table.Column<decimal>(type: "numeric", nullable: false),
                    SeniorityId = table.Column<Guid>(type: "uuid", nullable: false),
                    Created = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    LastModified = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    LastModifiedBy = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedBy = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rubrics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rubrics_Seniorities_SeniorityId",
                        column: x => x.SeniorityId,
                        principalSchema: "experties",
                        principalTable: "Seniorities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rubrics_SubSkills_SubSkillId",
                        column: x => x.SubSkillId,
                        principalSchema: "experties",
                        principalTable: "SubSkills",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_SeniorityId",
                schema: "experties",
                table: "Rubrics",
                column: "SeniorityId");

            migrationBuilder.CreateIndex(
                name: "IX_Rubrics_SubSkillId",
                schema: "experties",
                table: "Rubrics",
                column: "SubSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SeniorityLevelJunctions_SeniorityLevelId",
                schema: "experties",
                table: "SeniorityLevelJunctions",
                column: "SeniorityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_SeniorityLevelJunctions_SkillId",
                schema: "experties",
                table: "SeniorityLevelJunctions",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SubSkills_SkillId",
                schema: "experties",
                table: "SubSkills",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rubrics",
                schema: "experties");

            migrationBuilder.DropTable(
                name: "SeniorityLevelJunctions",
                schema: "experties");

            migrationBuilder.DropTable(
                name: "SubSkills",
                schema: "experties");

            migrationBuilder.DropTable(
                name: "Seniorities",
                schema: "experties");

            migrationBuilder.DropTable(
                name: "Skills",
                schema: "experties");
        }
    }
}
