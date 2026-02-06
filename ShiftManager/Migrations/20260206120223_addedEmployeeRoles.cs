using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftManager.Migrations
{
    /// <inheritdoc />
    public partial class addedEmployeeRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employee_role_employees_employee_id",
                table: "employee_role");

            migrationBuilder.DropForeignKey(
                name: "fk_employee_role_roles_role_id",
                table: "employee_role");

            migrationBuilder.DropPrimaryKey(
                name: "pk_employee_role",
                table: "employee_role");

            migrationBuilder.RenameTable(
                name: "employee_role",
                newName: "employee_roles");

            migrationBuilder.RenameIndex(
                name: "ix_employee_role_role_id",
                table: "employee_roles",
                newName: "ix_employee_roles_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_employee_roles",
                table: "employee_roles",
                columns: new[] { "employee_id", "role_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_employee_roles_employees_employee_id",
                table: "employee_roles",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_employee_roles_roles_role_id",
                table: "employee_roles",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_employee_roles_employees_employee_id",
                table: "employee_roles");

            migrationBuilder.DropForeignKey(
                name: "fk_employee_roles_roles_role_id",
                table: "employee_roles");

            migrationBuilder.DropPrimaryKey(
                name: "pk_employee_roles",
                table: "employee_roles");

            migrationBuilder.RenameTable(
                name: "employee_roles",
                newName: "employee_role");

            migrationBuilder.RenameIndex(
                name: "ix_employee_roles_role_id",
                table: "employee_role",
                newName: "ix_employee_role_role_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_employee_role",
                table: "employee_role",
                columns: new[] { "employee_id", "role_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_employee_role_employees_employee_id",
                table: "employee_role",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_employee_role_roles_role_id",
                table: "employee_role",
                column: "role_id",
                principalTable: "roles",
                principalColumn: "role_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
