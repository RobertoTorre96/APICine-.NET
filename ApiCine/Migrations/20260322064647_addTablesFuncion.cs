using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCine.Migrations
{
    /// <inheritdoc />
    public partial class addTablesFuncion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Pelicula_Genero",
                table: "Pelicula_Genero");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Pelicula_Genero",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pelicula_Genero",
                table: "Pelicula_Genero",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalaEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalaEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "funcion",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Precio = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    FechaHora = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PeliculaId = table.Column<long>(type: "INTEGER", nullable: false),
                    SalaId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_funcion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_funcion_Pelicula_PeliculaId",
                        column: x => x.PeliculaId,
                        principalTable: "Pelicula",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_funcion_SalaEntity_SalaId",
                        column: x => x.SalaId,
                        principalTable: "SalaEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PeliculaGenero_PeliculaId_GeneroId",
                table: "Pelicula_Genero",
                columns: new[] { "PeliculaId", "GeneroId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funcion_FechaHora_SalaId",
                table: "funcion",
                columns: new[] { "FechaHora", "SalaId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_funcion_PeliculaId",
                table: "funcion",
                column: "PeliculaId");

            migrationBuilder.CreateIndex(
                name: "IX_funcion_SalaId",
                table: "funcion",
                column: "SalaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "funcion");

            migrationBuilder.DropTable(
                name: "SalaEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Pelicula_Genero",
                table: "Pelicula_Genero");

            migrationBuilder.DropIndex(
                name: "IX_PeliculaGenero_PeliculaId_GeneroId",
                table: "Pelicula_Genero");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Pelicula_Genero");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Pelicula_Genero",
                table: "Pelicula_Genero",
                columns: new[] { "PeliculaId", "GeneroId" });
        }
    }
}
