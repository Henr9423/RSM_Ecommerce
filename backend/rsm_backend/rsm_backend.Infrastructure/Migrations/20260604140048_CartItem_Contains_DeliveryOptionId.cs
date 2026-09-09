using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CartItem_Contains_DeliveryOptionId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_carts_delivery_options_delivery_option_id",
                table: "carts");

            migrationBuilder.DropIndex(
                name: "ix_carts_delivery_option_id",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "carts");

            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "cart_items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_cart_items_delivery_option_id",
                table: "cart_items",
                column: "delivery_option_id");

            migrationBuilder.AddForeignKey(
                name: "fk_cart_items_delivery_options_delivery_option_id",
                table: "cart_items",
                column: "delivery_option_id",
                principalTable: "delivery_options",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cart_items_delivery_options_delivery_option_id",
                table: "cart_items");

            migrationBuilder.DropIndex(
                name: "ix_cart_items_delivery_option_id",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "cart_items");

            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "carts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_carts_delivery_option_id",
                table: "carts",
                column: "delivery_option_id");

            migrationBuilder.AddForeignKey(
                name: "fk_carts_delivery_options_delivery_option_id",
                table: "carts",
                column: "delivery_option_id",
                principalTable: "delivery_options",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
