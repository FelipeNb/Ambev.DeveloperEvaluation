using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class CartProductMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CartId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false,  defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartProducts", x => x.Id);

                    table.ForeignKey(
                        name: "FK_CartProducts_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_CartProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);

                    table.CheckConstraint(
                        name: "CK_CartProducts_Quantity",
                        sql: "\"Quantity\" > 0"
                    );
                });

            // Unique constraint (índice único para evitar duplicidade de produtos no mesmo carrinho)
            migrationBuilder.CreateIndex(
                name: "uk_cart_product",
                table: "CartProducts",
                columns: new[] { "CartId", "ProductId" },
                unique: true);

            // Indexes
            migrationBuilder.CreateIndex(
                name: "idx_cart_products_cart_id",
                table: "CartProducts",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "idx_cart_products_product_id",
                table: "CartProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "idx_cart_products_quantity",
                table: "CartProducts",
                column: "Quantity");

            migrationBuilder.CreateIndex(
                name: "idx_cart_products_created_at",
                table: "CartProducts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "idx_cart_products_updated_at",
                table: "CartProducts",
                column: "UpdatedAt");
            
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_update_cart_product_updatedat
BEFORE UPDATE ON ""CartProducts""
FOR EACH ROW
EXECUTE FUNCTION update_updatedat_column();
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartProducts");
        }
    }
}
