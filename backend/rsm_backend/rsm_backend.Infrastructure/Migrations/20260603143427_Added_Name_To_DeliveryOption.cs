using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace rsm_backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Added_Name_To_DeliveryOption : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "delivery_options",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "delivery_options");
        }
    }
}
