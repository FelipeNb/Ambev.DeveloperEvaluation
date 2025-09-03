using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCartMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Carts",
                type: "date",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "date");
            
            migrationBuilder.CreateSequence<int>(
                name: "SaleNumber_seq",
                startValue: 1L,
                incrementBy: 1);

            migrationBuilder.AddColumn<int>(
                name: "SaleNumber",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValueSql: "nextval('\"SaleNumber_seq\"')");

            migrationBuilder.AddColumn<string>(
                name: "Branch",
                table: "Carts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Cancelled",
                table: "Carts",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalAmount",
                table: "Carts",
                type: "numeric(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SaleNumber",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Branch",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Cancelled",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "TotalAmount",
                table: "Carts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "Carts",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);
            
            migrationBuilder.DropColumn(
                name: "SaleNumber",
                table: "Carts");

            // Remove a sequence
            migrationBuilder.Sql("DROP SEQUENCE IF EXISTS sale_number_seq;");

        }
    }
}
