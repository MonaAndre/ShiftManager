using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftManager.Migrations
{
    /// <inheritdoc />
    public partial class addedShifts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_shift_departments_department_id",
                table: "shift");

            migrationBuilder.DropForeignKey(
                name: "fk_shift_employees_employee_id",
                table: "shift");

            migrationBuilder.DropPrimaryKey(
                name: "pk_shift",
                table: "shift");

            migrationBuilder.RenameTable(
                name: "shift",
                newName: "shifts");

            migrationBuilder.RenameIndex(
                name: "ix_shift_employee_id",
                table: "shifts",
                newName: "ix_shifts_employee_id");

            migrationBuilder.RenameIndex(
                name: "ix_shift_department_id",
                table: "shifts",
                newName: "ix_shifts_department_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_shifts",
                table: "shifts",
                column: "shift_id");

            migrationBuilder.AddForeignKey(
                name: "fk_shifts_departments_department_id",
                table: "shifts",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "department_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shifts_employees_employee_id",
                table: "shifts",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_shifts_departments_department_id",
                table: "shifts");

            migrationBuilder.DropForeignKey(
                name: "fk_shifts_employees_employee_id",
                table: "shifts");

            migrationBuilder.DropPrimaryKey(
                name: "pk_shifts",
                table: "shifts");

            migrationBuilder.RenameTable(
                name: "shifts",
                newName: "shift");

            migrationBuilder.RenameIndex(
                name: "ix_shifts_employee_id",
                table: "shift",
                newName: "ix_shift_employee_id");

            migrationBuilder.RenameIndex(
                name: "ix_shifts_department_id",
                table: "shift",
                newName: "ix_shift_department_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_shift",
                table: "shift",
                column: "shift_id");

            migrationBuilder.AddForeignKey(
                name: "fk_shift_departments_department_id",
                table: "shift",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "department_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_shift_employees_employee_id",
                table: "shift",
                column: "employee_id",
                principalTable: "employees",
                principalColumn: "employee_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
