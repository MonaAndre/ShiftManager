using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftManager.Migrations
{
    /// <inheritdoc />
    public partial class AddShifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "shift",
                columns: table => new
                {
                    shift_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    start_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    end_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shift", x => x.shift_id);
                    table.ForeignKey(
                        name: "fk_shift_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_shift_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "shift",
                columns: new[] { "shift_id", "created_at", "department_id", "employee_id", "end_date", "start_date" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 1, 29, 15, 45, 13, 375, DateTimeKind.Utc).AddTicks(1280), 2, 1, new DateTime(2026, 2, 2, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, new DateTime(2026, 1, 29, 15, 45, 13, 375, DateTimeKind.Utc).AddTicks(1930), 2, 1, new DateTime(2026, 2, 3, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 8, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, new DateTime(2026, 1, 29, 15, 45, 13, 375, DateTimeKind.Utc).AddTicks(1930), 3, 2, new DateTime(2026, 2, 2, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 4, new DateTime(2026, 1, 29, 15, 45, 13, 375, DateTimeKind.Utc).AddTicks(1930), 3, 2, new DateTime(2026, 2, 3, 17, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 9, 0, 0, 0, DateTimeKind.Utc) },
                    { 5, new DateTime(2026, 1, 29, 15, 45, 13, 375, DateTimeKind.Utc).AddTicks(1940), 1, 3, new DateTime(2026, 2, 2, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 2, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { 6, new DateTime(2026, 1, 29, 15, 45, 13, 375, DateTimeKind.Utc).AddTicks(1940), 1, 3, new DateTime(2026, 2, 3, 18, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 2, 3, 10, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.CreateIndex(
                name: "ix_shift_department_id",
                table: "shift",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_shift_employee_id",
                table: "shift",
                column: "employee_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shift");
        }
    }
}
