﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NewMasterMarket.Migrations
{
    public partial class AddingMinAbouttoItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinAmount",
                table: "Items",
                type: "int",
                nullable: false,
                defaultValue: 1);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinAmount",
                table: "Items");
        }
    }
}
