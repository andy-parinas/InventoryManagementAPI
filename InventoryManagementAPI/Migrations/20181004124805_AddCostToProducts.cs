using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace InventoryManagementAPI.Migrations
{
    public partial class AddCostToProducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "Products",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Products");
        }
    }
}
