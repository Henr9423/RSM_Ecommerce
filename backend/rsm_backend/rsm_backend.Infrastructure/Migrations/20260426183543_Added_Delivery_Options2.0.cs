using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Delivery_Options20 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "delivery_option_id",
                table: "order_items",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "delivery_price",
                table: "order_items",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "ix_order_items_delivery_option_id",
                table: "order_items",
                column: "delivery_option_id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_items_delivery_options_delivery_option_id",
                table: "order_items",
                column: "delivery_option_id",
                principalTable: "delivery_options",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_items_delivery_options_delivery_option_id",
                table: "order_items");

            migrationBuilder.DropIndex(
                name: "ix_order_items_delivery_option_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "delivery_option_id",
                table: "order_items");

            migrationBuilder.DropColumn(
                name: "delivery_price",
                table: "order_items");
        }
    }
}
