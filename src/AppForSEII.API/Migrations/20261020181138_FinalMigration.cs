using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppForSEII.API.Migrations
{
    /// <inheritdoc />
    public partial class FinalMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncargoImpresion_AspNetUsers_ClienteId",
                table: "EncargoImpresion");

            migrationBuilder.DropForeignKey(
                name: "FK_LineaEncargo_EncargoImpresion_EncargoImpresionId",
                table: "LineaEncargo");

            migrationBuilder.DropForeignKey(
                name: "FK_LineaEncargo_Material_MaterialSeleccionadoId",
                table: "LineaEncargo");

            migrationBuilder.DropForeignKey(
                name: "FK_LineaEncargo_Pieza3D_PiezaId",
                table: "LineaEncargo");

            migrationBuilder.DropForeignKey(
                name: "FK_Material_Pieza3D_Pieza3DId",
                table: "Material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pieza3D",
                table: "Pieza3D");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Material",
                table: "Material");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LineaEncargo",
                table: "LineaEncargo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EncargoImpresion",
                table: "EncargoImpresion");

            migrationBuilder.RenameTable(
                name: "Pieza3D",
                newName: "Piezas3D");

            migrationBuilder.RenameTable(
                name: "Material",
                newName: "Materiales");

            migrationBuilder.RenameTable(
                name: "LineaEncargo",
                newName: "LineasEncargo");

            migrationBuilder.RenameTable(
                name: "EncargoImpresion",
                newName: "EncargoImpresiones");

            migrationBuilder.RenameIndex(
                name: "IX_Material_Pieza3DId",
                table: "Materiales",
                newName: "IX_Materiales_Pieza3DId");

            migrationBuilder.RenameIndex(
                name: "IX_LineaEncargo_PiezaId",
                table: "LineasEncargo",
                newName: "IX_LineasEncargo_PiezaId");

            migrationBuilder.RenameIndex(
                name: "IX_LineaEncargo_MaterialSeleccionadoId",
                table: "LineasEncargo",
                newName: "IX_LineasEncargo_MaterialSeleccionadoId");

            migrationBuilder.RenameIndex(
                name: "IX_LineaEncargo_EncargoImpresionId",
                table: "LineasEncargo",
                newName: "IX_LineasEncargo_EncargoImpresionId");

            migrationBuilder.RenameIndex(
                name: "IX_EncargoImpresion_ClienteId",
                table: "EncargoImpresiones",
                newName: "IX_EncargoImpresiones_ClienteId");

            migrationBuilder.AlterColumn<int>(
                name: "EncargoImpresionId",
                table: "LineasEncargo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Piezas3D",
                table: "Piezas3D",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Materiales",
                table: "Materiales",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LineasEncargo",
                table: "LineasEncargo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EncargoImpresiones",
                table: "EncargoImpresiones",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EncargoImpresiones_AspNetUsers_ClienteId",
                table: "EncargoImpresiones",
                column: "ClienteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineasEncargo_EncargoImpresiones_EncargoImpresionId",
                table: "LineasEncargo",
                column: "EncargoImpresionId",
                principalTable: "EncargoImpresiones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineasEncargo_Materiales_MaterialSeleccionadoId",
                table: "LineasEncargo",
                column: "MaterialSeleccionadoId",
                principalTable: "Materiales",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineasEncargo_Piezas3D_PiezaId",
                table: "LineasEncargo",
                column: "PiezaId",
                principalTable: "Piezas3D",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Materiales_Piezas3D_Pieza3DId",
                table: "Materiales",
                column: "Pieza3DId",
                principalTable: "Piezas3D",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EncargoImpresiones_AspNetUsers_ClienteId",
                table: "EncargoImpresiones");

            migrationBuilder.DropForeignKey(
                name: "FK_LineasEncargo_EncargoImpresiones_EncargoImpresionId",
                table: "LineasEncargo");

            migrationBuilder.DropForeignKey(
                name: "FK_LineasEncargo_Materiales_MaterialSeleccionadoId",
                table: "LineasEncargo");

            migrationBuilder.DropForeignKey(
                name: "FK_LineasEncargo_Piezas3D_PiezaId",
                table: "LineasEncargo");

            migrationBuilder.DropForeignKey(
                name: "FK_Materiales_Piezas3D_Pieza3DId",
                table: "Materiales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Piezas3D",
                table: "Piezas3D");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Materiales",
                table: "Materiales");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LineasEncargo",
                table: "LineasEncargo");

            migrationBuilder.DropPrimaryKey(
                name: "PK_EncargoImpresiones",
                table: "EncargoImpresiones");

            migrationBuilder.RenameTable(
                name: "Piezas3D",
                newName: "Pieza3D");

            migrationBuilder.RenameTable(
                name: "Materiales",
                newName: "Material");

            migrationBuilder.RenameTable(
                name: "LineasEncargo",
                newName: "LineaEncargo");

            migrationBuilder.RenameTable(
                name: "EncargoImpresiones",
                newName: "EncargoImpresion");

            migrationBuilder.RenameIndex(
                name: "IX_Materiales_Pieza3DId",
                table: "Material",
                newName: "IX_Material_Pieza3DId");

            migrationBuilder.RenameIndex(
                name: "IX_LineasEncargo_PiezaId",
                table: "LineaEncargo",
                newName: "IX_LineaEncargo_PiezaId");

            migrationBuilder.RenameIndex(
                name: "IX_LineasEncargo_MaterialSeleccionadoId",
                table: "LineaEncargo",
                newName: "IX_LineaEncargo_MaterialSeleccionadoId");

            migrationBuilder.RenameIndex(
                name: "IX_LineasEncargo_EncargoImpresionId",
                table: "LineaEncargo",
                newName: "IX_LineaEncargo_EncargoImpresionId");

            migrationBuilder.RenameIndex(
                name: "IX_EncargoImpresiones_ClienteId",
                table: "EncargoImpresion",
                newName: "IX_EncargoImpresion_ClienteId");

            migrationBuilder.AlterColumn<int>(
                name: "EncargoImpresionId",
                table: "LineaEncargo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pieza3D",
                table: "Pieza3D",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Material",
                table: "Material",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LineaEncargo",
                table: "LineaEncargo",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_EncargoImpresion",
                table: "EncargoImpresion",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_EncargoImpresion_AspNetUsers_ClienteId",
                table: "EncargoImpresion",
                column: "ClienteId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineaEncargo_EncargoImpresion_EncargoImpresionId",
                table: "LineaEncargo",
                column: "EncargoImpresionId",
                principalTable: "EncargoImpresion",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LineaEncargo_Material_MaterialSeleccionadoId",
                table: "LineaEncargo",
                column: "MaterialSeleccionadoId",
                principalTable: "Material",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LineaEncargo_Pieza3D_PiezaId",
                table: "LineaEncargo",
                column: "PiezaId",
                principalTable: "Pieza3D",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Material_Pieza3D_Pieza3DId",
                table: "Material",
                column: "Pieza3DId",
                principalTable: "Pieza3D",
                principalColumn: "Id");
        }
    }
}
