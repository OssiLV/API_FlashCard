using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Migrations
{
    /// <inheritdoc />
    public partial class removed_RememberMe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99303566-de6e-4b51-81de-e94541077739"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "82574569-9419-4a1a-90ba-cc0d8c0d0cd4", "AQAAAAIAAYagAAAAEAajnUbd2FPx+Yv3eIlWZKultvT2yaUUuL/h2Yfm++HsG6hGcZKhwqVLXPBqnFC6nA==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("99303566-de6e-4b51-81de-e94541077739"),
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2243ee17-b38a-4b07-a95c-7034efccac8b", "AQAAAAIAAYagAAAAEC3lrTpOvSlwAGu7NfJZKKZMO76s0iwmuBgvHyZeeXZwh8AGPEArctH5a2j11gr/Dg==" });
        }
    }
}
