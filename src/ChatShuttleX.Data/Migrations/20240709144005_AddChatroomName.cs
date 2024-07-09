using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatShuttleX.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddChatroomName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Chatrooms",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Chatrooms_Name",
                table: "Chatrooms",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Chatrooms_Name",
                table: "Chatrooms");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Chatrooms");
        }
    }
}
