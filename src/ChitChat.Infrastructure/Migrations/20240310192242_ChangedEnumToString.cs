using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChitChat.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedEnumToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "FriendshipStatus",
                table: "Friendships",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "FriendshipStatus",
                table: "Friendships",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");
        }
    }
}
