using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ShoppingCart.Data.Migrations
{
    public partial class addingkeystoDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "Iv",
                table: "StudentAssignments",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "Key",
                table: "StudentAssignments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrivateKey",
                table: "StudentAssignments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Iv",
                table: "StudentAssignments");

            migrationBuilder.DropColumn(
                name: "Key",
                table: "StudentAssignments");

            migrationBuilder.DropColumn(
                name: "PrivateKey",
                table: "StudentAssignments");
        }
    }
}
