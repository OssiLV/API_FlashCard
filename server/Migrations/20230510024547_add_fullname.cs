using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class add_fullname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99303566-de6e-4b51-81de-e94541077739"),
                columns: new[] { "ConcurrencyStamp", "FullName", "PasswordHash" },
                values: new object[] { "dda8e9dc-c60a-43a6-8aad-c810bddfa96d", null, "AQAAAAIAAYagAAAAEEoz96WI2HreD3dwKeXyisPT6qgA3Vq/twySHtIfkTh1vow4jCfMUPbWUsk432t1Pw==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Users");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99303566-de6e-4b51-81de-e94541077739"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9c236b07-5525-42d5-b5c3-197b87057a8f", "AQAAAAIAAYagAAAAEBR0tMQjWKbn3INgTkApRZmkaiwghM64qQeH0TL43oCH3VDQfWe53FbsOuJsO4gMvA==" });
        }
    }
}
