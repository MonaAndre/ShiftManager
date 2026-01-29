using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShiftManager.Migrations
{
    /// <inheritdoc />
    public partial class FixShiftCreatedAtSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 1,
                column: "created_at",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 2,
                column: "created_at",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 3,
                column: "created_at",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 4,
                column: "created_at",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 5,
                column: "created_at",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.UpdateData(
                table: "shift",
                keyColumn: "shift_id",
                keyValue: 6,
                column: "created_at",
                value: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
