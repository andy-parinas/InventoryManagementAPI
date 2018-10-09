using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InventoryManagementAPI.Migrations
{
    public partial class AddIsArchivedToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsArchived",
                table: "Products",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsArchived",
                table: "Products");
        }
    }
}
