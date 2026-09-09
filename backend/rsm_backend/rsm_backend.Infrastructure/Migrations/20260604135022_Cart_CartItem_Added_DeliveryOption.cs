using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cart_CartItem_Added_DeliveryOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
