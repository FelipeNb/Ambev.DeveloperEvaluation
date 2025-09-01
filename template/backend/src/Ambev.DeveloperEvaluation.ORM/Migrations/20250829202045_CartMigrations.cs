using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ambev.DeveloperEvaluation.ORM.Migrations
{
    /// <inheritdoc />
    public partial class CartMigrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false,  defaultValueSql: "NOW()"),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade
                    );
                });

            // Indexes
            migrationBuilder.CreateIndex(
                name: "idx_carts_user_id",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "idx_carts_date",
                table: "Carts",
                column: "Date");

            migrationBuilder.CreateIndex(
                name: "idx_carts_user_date",
                table: "Carts",
                columns: new[] { "UserId", "Date" });

            migrationBuilder.CreateIndex(
                name: "idx_carts_created_at",
                table: "Carts",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "idx_carts_updated_at",
                table: "Carts",
                column: "UpdatedAt");
            migrationBuilder.Sql(@"
CREATE TRIGGER trg_update_cart_updatedat
BEFORE UPDATE ON ""Carts""
FOR EACH ROW
EXECUTE FUNCTION update_updatedat_column();
");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Carts");
        }
    }
}
