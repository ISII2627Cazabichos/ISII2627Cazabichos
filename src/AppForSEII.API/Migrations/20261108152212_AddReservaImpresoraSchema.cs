using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII.API.Migrations
{
    /// <inheritdoc />
    public partial class AddReservaImpresoraSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Materiales_Piezas3D_Pieza3DId",
                table: "Materiales");

            migrationBuilder.DropIndex(
                name: "IX_Materiales_Pieza3DId",
                table: "Materiales");

            migrationBuilder.DropColumn(
                name: "Pieza3DId",
                table: "Materiales");

            migrationBuilder.CreateTable(
                name: "Impresoras3D",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Modelo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    PrecioKilovatioHora = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PrecioReserva = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Impresoras3D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PiezasMateriales",
                columns: table => new
                {
                    PiezaId = table.Column<int>(type: "int", nullable: false),
                    MaterialId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PiezasMateriales", x => new { x.PiezaId, x.MaterialId });
                    table.ForeignKey(
                        name: "FK_PiezasMateriales_Materiales_MaterialId",
                        column: x => x.MaterialId,
                        principalTable: "Materiales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PiezasMateriales_Piezas3D_PiezaId",
                        column: x => x.PiezaId,
                        principalTable: "Piezas3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReservasImpresora",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FechaReserva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApellidosCliente = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DireccionFacturacion = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PrecioTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    MetodoPago = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    ClienteId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservasImpresora", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservasImpresora_AspNetUsers_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LineasReserva",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TiempoReserva = table.Column<int>(type: "int", maxLength: 50, nullable: false),
                    PrecioSubtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Impresora3DId = table.Column<int>(type: "int", nullable: false),
                    ReservaImpresoraId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineasReserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LineasReserva_Impresoras3D_Impresora3DId",
                        column: x => x.Impresora3DId,
                        principalTable: "Impresoras3D",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LineasReserva_ReservasImpresora_ReservaImpresoraId",
                        column: x => x.ReservaImpresoraId,
                        principalTable: "ReservasImpresora",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LineasReserva_Impresora3DId",
                table: "LineasReserva",
                column: "Impresora3DId");

            migrationBuilder.CreateIndex(
                name: "IX_LineasReserva_ReservaImpresoraId",
                table: "LineasReserva",
                column: "ReservaImpresoraId");

            migrationBuilder.CreateIndex(
                name: "IX_PiezasMateriales_MaterialId",
                table: "PiezasMateriales",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservasImpresora_ClienteId",
                table: "ReservasImpresora",
                column: "ClienteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LineasReserva");

            migrationBuilder.DropTable(
                name: "PiezasMateriales");

            migrationBuilder.DropTable(
                name: "Impresoras3D");

            migrationBuilder.DropTable(
                name: "ReservasImpresora");

            migrationBuilder.AddColumn<int>(
                name: "Pieza3DId",
                table: "Materiales",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Materiales_Pieza3DId",
                table: "Materiales",
                column: "Pieza3DId");

            migrationBuilder.AddForeignKey(
                name: "FK_Materiales_Piezas3D_Pieza3DId",
                table: "Materiales",
                column: "Pieza3DId",
                principalTable: "Piezas3D",
                principalColumn: "Id");
        }
    }
}
