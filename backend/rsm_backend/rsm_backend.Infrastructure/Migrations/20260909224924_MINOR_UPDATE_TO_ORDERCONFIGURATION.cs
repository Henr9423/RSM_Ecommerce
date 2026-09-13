using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MINOR_UPDATE_TO_ORDERCONFIGURATION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_order_total_valid",
                table: "orders");

            migrationBuilder.AddCheckConstraint(
                name: "ck_order_total_valid",
                table: "orders",
                sql: "total = subtotal + tax + shipping_fee-coupon_discount");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "ck_order_total_valid",
                table: "orders");

            migrationBuilder.AddCheckConstraint(
                name: "ck_order_total_valid",
                table: "orders",
                sql: "total = subtotal + tax + shipping_fee-discount-discount");
        }
    }
}
