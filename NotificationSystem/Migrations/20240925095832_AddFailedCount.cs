using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NotificationSystem.Migrations
{
    /// <inheritdoc />
    public partial class AddFailedCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "RetryCount",
                table: "FailedNotifications",
                type: "smallint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RetryCount",
                table: "FailedNotifications");
        }
    }
}
