using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddBorrowedAtAndReturnedAtTimestamps : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReturnDate",
                table: "Borrows",
                newName: "ReturnedAt");

            migrationBuilder.RenameColumn(
                name: "BorrowDate",
                table: "Borrows",
                newName: "BorrowedAt");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DueDate",
                table: "Borrows",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Borrows");

            migrationBuilder.RenameColumn(
                name: "ReturnedAt",
                table: "Borrows",
                newName: "ReturnDate");

            migrationBuilder.RenameColumn(
                name: "BorrowedAt",
                table: "Borrows",
                newName: "BorrowDate");
        }
    }
}
