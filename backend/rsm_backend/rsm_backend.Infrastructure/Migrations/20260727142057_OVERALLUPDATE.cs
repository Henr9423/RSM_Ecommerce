using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class OVERALLUPDATE : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_cart_items_delivery_options_delivery_option_id",
                table: "cart_items");

            migrationBuilder.DropForeignKey(
                name: "fk_order_items_delivery_options_delivery_option_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "ix_order_items_delivery_option_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "ix_cart_items_delivery_option_id",
                table: "cart_items");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "cart_items");

            migrationBuilder.RenameColumn(
                name: "guest_access_token",
                table: "orders",
                newName: "guest_access_token_hash");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "orders",
                newName: "coupon_discount");

            migrationBuilder.RenameColumn(
                name: "discount",
                table: "order_items",
                newName: "unit_discount");

            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "estimated_delivery_from",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "estimated_delivery_to",
                table: "orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "carts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_orders_delivery_option_id",
                table: "orders",
                column: "delivery_option_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_orders_delivery_options_delivery_option_id",
                table: "orders",
                column: "delivery_option_id",
                principalTable: "delivery_options",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_carts_delivery_options_delivery_option_id",
                table: "carts");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_delivery_options_delivery_option_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_orders_delivery_option_id",
                table: "orders");

            migrationBuilder.DropIndex(
                name: "ix_carts_delivery_option_id",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "estimated_delivery_from",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "estimated_delivery_to",
                table: "orders");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "carts");

            migrationBuilder.RenameColumn(
                name: "guest_access_token_hash",
                table: "orders",
                newName: "guest_access_token");

            migrationBuilder.RenameColumn(
                name: "coupon_discount",
                table: "orders",
                newName: "discount");

            migrationBuilder.RenameColumn(
                name: "unit_discount",
                table: "order_items",
                newName: "discount");

            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "order_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "cart_items",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_delivery_option_id",
                table: "order_items",
                column: "delivery_option_id");

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

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_delivery_options_delivery_option_id",
                table: "order_items",
                column: "delivery_option_id",
                principalTable: "delivery_options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
