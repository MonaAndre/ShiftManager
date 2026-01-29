using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShiftManager.Migrations
{
    /// <inheritdoc />
    public partial class AddEmployeeRoleAndSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "employee_role",
                columns: table => new
                {
                    employee_id = table.Column<int>(type: "integer", nullable: false),
                    role_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_role", x => new { x.employee_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_employee_role_employees_employee_id",
                        column: x => x.employee_id,
                        principalTable: "employees",
                        principalColumn: "employee_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_employee_role_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "employee_id", "department_id", "email", "first_name", "last_name" },
                values: new object[,]
                {
                    { 1, 2, "anna.andersson@company.se", "Anna", "Andersson" },
                    { 2, 3, "erik.johansson@company.se", "Erik", "Johansson" }
                });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "employee_id", "email", "first_name", "last_name" },
                values: new object[] { 3, "sara.nilsson@company.se", "Sara", "Nilsson" });

            migrationBuilder.InsertData(
                table: "roles",
                columns: new[] { "role_id", "role_description", "role_name" },
                values: new object[,]
                {
                    { 1, "System administrator", "Admin" },
                    { 2, "Team or department manager", "Manager" },
                    { 3, "Regular staff member", "Staff" }
                });

            migrationBuilder.InsertData(
                table: "employee_role",
                columns: new[] { "employee_id", "role_id" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 1, 2 },
                    { 2, 3 },
                    { 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "ix_employee_role_role_id",
                table: "employee_role",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "employee_role");

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

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "roles",
                keyColumn: "role_id",
                keyValue: 3);
        }
    }
}
