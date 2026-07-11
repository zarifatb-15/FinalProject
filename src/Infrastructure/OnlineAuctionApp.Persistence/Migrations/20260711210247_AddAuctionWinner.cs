using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineAuctionApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuctionWinner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WinnerId",
                table: "Auctions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Auctions_WinnerId",
                table: "Auctions",
                column: "WinnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Auctions_AspNetUsers_WinnerId",
                table: "Auctions",
                column: "WinnerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Auctions_AspNetUsers_WinnerId",
                table: "Auctions");

            migrationBuilder.DropIndex(
                name: "IX_Auctions_WinnerId",
                table: "Auctions");

            migrationBuilder.DropColumn(
                name: "WinnerId",
                table: "Auctions");
        }
    }
}
