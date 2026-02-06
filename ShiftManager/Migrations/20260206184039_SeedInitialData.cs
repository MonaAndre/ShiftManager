using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftManager.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
      protected override void Up(MigrationBuilder migrationBuilder)
{
    migrationBuilder.Sql("""
    INSERT INTO employees (employee_id, department_id, email, first_name, last_name)
    VALUES (1, 2, 'anna.andersson@company.se', 'Anna', 'Andersson')
    ON CONFLICT (employee_id) DO NOTHING;

    INSERT INTO employees (employee_id, department_id, email, first_name, last_name)
    VALUES (2, 3, 'erik.johansson@company.se', 'Erik', 'Johansson')
    ON CONFLICT (employee_id) DO NOTHING;

    INSERT INTO employees (employee_id, department_id, email, first_name, last_name)
    VALUES (3, 1, 'sara.nilsson@company.se', 'Sara', 'Nilsson')
    ON CONFLICT (employee_id) DO NOTHING;

    INSERT INTO employee_roles (employee_id, role_id)
    VALUES (1, 1)
    ON CONFLICT (employee_id, role_id) DO NOTHING;

    INSERT INTO employee_roles (employee_id, role_id)
    VALUES (1, 2)
    ON CONFLICT (employee_id, role_id) DO NOTHING;

    INSERT INTO employee_roles (employee_id, role_id)
    VALUES (2, 3)
    ON CONFLICT (employee_id, role_id) DO NOTHING;

    INSERT INTO employee_roles (employee_id, role_id)
    VALUES (3, 3)
    ON CONFLICT (employee_id, role_id) DO NOTHING;

    INSERT INTO shifts (shift_id, created_at, department_id, employee_id, start_date, end_date)
    VALUES
      (1, '2026-01-29T12:00:00Z', 2, 1, '2026-02-02T08:00:00Z', '2026-02-02T16:00:00Z')
    ON CONFLICT (shift_id) DO NOTHING;

    INSERT INTO shifts (shift_id, created_at, department_id, employee_id, start_date, end_date)
    VALUES
      (2, '2026-01-29T12:00:00Z', 2, 1, '2026-02-03T08:00:00Z', '2026-02-03T16:00:00Z')
    ON CONFLICT (shift_id) DO NOTHING;

    INSERT INTO shifts (shift_id, created_at, department_id, employee_id, start_date, end_date)
    VALUES
      (3, '2026-01-29T12:00:00Z', 3, 2, '2026-02-02T09:00:00Z', '2026-02-02T17:00:00Z')
    ON CONFLICT (shift_id) DO NOTHING;

    INSERT INTO shifts (shift_id, created_at, department_id, employee_id, start_date, end_date)
    VALUES
      (4, '2026-01-29T12:00:00Z', 3, 2, '2026-02-03T09:00:00Z', '2026-02-03T17:00:00Z')
    ON CONFLICT (shift_id) DO NOTHING;

    INSERT INTO shifts (shift_id, created_at, department_id, employee_id, start_date, end_date)
    VALUES
      (5, '2026-01-29T12:00:00Z', 1, 3, '2026-02-02T10:00:00Z', '2026-02-02T18:00:00Z')
    ON CONFLICT (shift_id) DO NOTHING;

    INSERT INTO shifts (shift_id, created_at, department_id, employee_id, start_date, end_date)
    VALUES
      (6, '2026-01-29T12:00:00Z', 1, 3, '2026-02-03T10:00:00Z', '2026-02-03T18:00:00Z')
    ON CONFLICT (shift_id) DO NOTHING;
    """);

    migrationBuilder.Sql("""
    SELECT setval(pg_get_serial_sequence('employees','employee_id'),
                  (SELECT COALESCE(MAX(employee_id), 1) FROM employees), true);
    SELECT setval(pg_get_serial_sequence('shifts','shift_id'),
                  (SELECT COALESCE(MAX(shift_id), 1) FROM shifts), true);
    """);
}


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                          table: "employee_roles",
                          keyColumns: new[] { "employee_id", "role_id" },
                          keyValues: new object[] { 1, 1 });
          
                      migrationBuilder.DeleteData(
                          table: "employee_roles",
                          keyColumns: new[] { "employee_id", "role_id" },
                          keyValues: new object[] { 1, 2 });
          
                      migrationBuilder.DeleteData(
                          table: "employee_roles",
                          keyColumns: new[] { "employee_id", "role_id" },
                          keyValues: new object[] { 2, 3 });
          
                      migrationBuilder.DeleteData(
                          table: "employee_roles",
                          keyColumns: new[] { "employee_id", "role_id" },
                          keyValues: new object[] { 3, 3 });
          
                      migrationBuilder.DeleteData(
                          table: "shifts",
                          keyColumn: "shift_id",
                          keyValue: 1);
          
                      migrationBuilder.DeleteData(
                          table: "shifts",
                          keyColumn: "shift_id",
                          keyValue: 2);
          
                      migrationBuilder.DeleteData(
                          table: "shifts",
                          keyColumn: "shift_id",
                          keyValue: 3);
          
                      migrationBuilder.DeleteData(
                          table: "shifts",
                          keyColumn: "shift_id",
                          keyValue: 4);
          
                      migrationBuilder.DeleteData(
                          table: "shifts",
                          keyColumn: "shift_id",
                          keyValue: 5);
          
                      migrationBuilder.DeleteData(
                          table: "shifts",
                          keyColumn: "shift_id",
                          keyValue: 6);
          
                      migrationBuilder.DeleteData(
                          table: "employees",
                          keyColumn: "employee_id",
                          keyValue: 1);
          
                      migrationBuilder.DeleteData(
                          table: "employees",
                          keyColumn: "employee_id",
                          keyValue: 2);
          
                      migrationBuilder.DeleteData(
                          table: "employees",
                          keyColumn: "employee_id",
                          keyValue: 3);
        }
    }
}
