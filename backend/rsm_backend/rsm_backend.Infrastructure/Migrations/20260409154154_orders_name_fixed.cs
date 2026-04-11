using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class orders_name_fixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_customers_customer_id",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "fk_order_order_addresses_billing_address_id",
                table: "order");

            migrationBuilder.DropForeignKey(
                name: "fk_order_order_addresses_shipping_address_id",
                table: "order");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order",
                table: "order");

            migrationBuilder.RenameTable(
                name: "order",
                newName: "orders");

            migrationBuilder.RenameIndex(
                name: "ix_order_shipping_address_id",
                table: "orders",
                newName: "ix_orders_shipping_address_id");

            migrationBuilder.RenameIndex(
                name: "ix_order_customer_id",
                table: "orders",
                newName: "ix_orders_customer_id");

            migrationBuilder.RenameIndex(
                name: "ix_order_billing_address_id",
                table: "orders",
                newName: "ix_orders_billing_address_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_orders",
                table: "orders",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_order_addresses_billing_address_id",
                table: "orders",
                column: "billing_address_id",
                principalTable: "order_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_orders_order_addresses_shipping_address_id",
                table: "orders",
                column: "shipping_address_id",
                principalTable: "order_addresses",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_order_addresses_billing_address_id",
                table: "orders");

            migrationBuilder.DropForeignKey(
                name: "fk_orders_order_addresses_shipping_address_id",
                table: "orders");

            migrationBuilder.DropPrimaryKey(
                name: "pk_orders",
                table: "orders");

            migrationBuilder.RenameTable(
                name: "orders",
                newName: "order");

            migrationBuilder.RenameIndex(
                name: "ix_orders_shipping_address_id",
                table: "order",
                newName: "ix_order_shipping_address_id");

            migrationBuilder.RenameIndex(
                name: "ix_orders_customer_id",
                table: "order",
                newName: "ix_order_customer_id");

            migrationBuilder.RenameIndex(
                name: "ix_orders_billing_address_id",
                table: "order",
                newName: "ix_order_billing_address_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order",
                table: "order",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_customers_customer_id",
                table: "order",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_order_order_addresses_billing_address_id",
                table: "order",
                column: "billing_address_id",
                principalTable: "order_addresses",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_order_order_addresses_shipping_address_id",
                table: "order",
                column: "shipping_address_id",
                principalTable: "order_addresses",
                principalColumn: "id");
        }
    }
}
