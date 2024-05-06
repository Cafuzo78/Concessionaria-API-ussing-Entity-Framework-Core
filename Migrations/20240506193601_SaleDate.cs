using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Concessionária.Migrations
{
    /// <inheritdoc />
    public partial class SaleDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SaleDate",
                table: "Cars",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleDate",
                table: "Cars");
        }
    }
}
