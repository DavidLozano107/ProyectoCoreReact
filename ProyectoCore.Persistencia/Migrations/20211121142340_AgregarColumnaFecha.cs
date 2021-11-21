using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProyectoCore.Persistencia.Migrations
{
    public partial class AgregarColumnaFecha : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Precio_CursoId",
                table: "Precio");

            migrationBuilder.AlterColumn<decimal>(
                name: "Promocion",
                table: "Precio",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,5)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioActual",
                table: "Precio",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,4)");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Instructor",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Curso",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaCreacion",
                table: "Comentario",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Precio_CursoId",
                table: "Precio",
                column: "CursoId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Precio_CursoId",
                table: "Precio");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Instructor");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Curso");

            migrationBuilder.DropColumn(
                name: "FechaCreacion",
                table: "Comentario");

            migrationBuilder.AlterColumn<decimal>(
                name: "Promocion",
                table: "Precio",
                type: "decimal(18,5)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PrecioActual",
                table: "Precio",
                type: "decimal(18,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.CreateIndex(
                name: "IX_Precio_CursoId",
                table: "Precio",
                column: "CursoId");
        }
    }
}
