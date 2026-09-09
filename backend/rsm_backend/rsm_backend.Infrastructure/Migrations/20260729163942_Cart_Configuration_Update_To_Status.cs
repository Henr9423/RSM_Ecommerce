using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cart_Configuration_Update_To_Status : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_carts_guest_cart_token",
                table: "carts");

            migrationBuilder.CreateIndex(
                name: "ix_carts_guest_cart_token",
                table: "carts",
                column: "guest_cart_token",
                unique: true,
                filter: "\"status\" = 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_carts_guest_cart_token",
                table: "carts");

            migrationBuilder.CreateIndex(
                name: "ix_carts_guest_cart_token",
                table: "carts",
                column: "guest_cart_token",
                unique: true);
        }
    }
}
