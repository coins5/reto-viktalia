using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clientes.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CLIENTES",
                columns: table => new
                {
                    ID = table.Column<long>(type: "NUMBER(19)", nullable: false)
                        .Annotation("Oracle:Identity", "START WITH 1 INCREMENT BY 1"),
                    RUC = table.Column<string>(type: "NVARCHAR2(11)", maxLength: 11, nullable: false),
                    RAZON_SOCIAL = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: false),
                    TELEFONO = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true),
                    CORREO = table.Column<string>(type: "NVARCHAR2(150)", maxLength: 150, nullable: true),
                    DIRECCION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    CREATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: false),
                    UPDATED_AT = table.Column<DateTime>(type: "TIMESTAMP", nullable: true),
                    IS_DELETED = table.Column<bool>(type: "BOOLEAN", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CLIENTES", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CLIENTES_RAZON_SOCIAL",
                table: "CLIENTES",
                column: "RAZON_SOCIAL");

            migrationBuilder.CreateIndex(
                name: "UX_CLIENTES_RUC",
                table: "CLIENTES",
                column: "RUC",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CLIENTES");
        }
    }
}
