using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class ProductMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Image = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RatingRate = table.Column<decimal>(type: "numeric(3,2)", nullable: true),
                    RatingCount = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false,  defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.CheckConstraint("CK_Products_Price", "\"Price\" >= 0");
                    table.CheckConstraint("CK_Products_RatingRate", "\"RatingRate\" >= 0 AND \"RatingRate\" <= 5");
                    table.CheckConstraint("CK_Products_RatingCount", "\"RatingCount\" >= 0");
                });

            // Indexes
            migrationBuilder.CreateIndex(
                name: "idx_products_category",
                table: "Products",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "idx_products_price",
                table: "Products",
                column: "Price");

            migrationBuilder.CreateIndex(
                name: "idx_products_rating_rate",
                table: "Products",
                column: "RatingRate");

            migrationBuilder.CreateIndex(
                name: "idx_products_rating_count",
                table: "Products",
                column: "RatingCount");

            migrationBuilder.CreateIndex(
                name: "idx_products_category_price",
                table: "Products",
                columns: new[] { "Category", "Price" });

            migrationBuilder.CreateIndex(
                name: "idx_products_created_at",
                table: "Products",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "idx_products_updated_at",
                table: "Products",
                column: "UpdatedAt");

            migrationBuilder.Sql(@"
CREATE TRIGGER trg_update_product_updatedat
BEFORE UPDATE ON ""Products""
FOR EACH ROW
EXECUTE FUNCTION update_updatedat_column();
");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
