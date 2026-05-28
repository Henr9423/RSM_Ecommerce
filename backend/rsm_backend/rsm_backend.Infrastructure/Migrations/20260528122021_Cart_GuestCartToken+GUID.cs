using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Cart_GuestCartTokenGUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "session_token",
                table: "carts");

            migrationBuilder.AddColumn<Guid>(
                name: "guest_cart_token",
                table: "carts",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_carts_guest_cart_token",
                table: "carts",
                column: "guest_cart_token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_carts_guest_cart_token",
                table: "carts");

            migrationBuilder.DropColumn(
                name: "guest_cart_token",
                table: "carts");

            migrationBuilder.AddColumn<int>(
                name: "session_token",
                table: "carts",
                type: "integer",
                nullable: true);
        }
    }
}
