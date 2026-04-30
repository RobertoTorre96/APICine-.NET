using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCine.Migrations
{
    /// <inheritdoc />
    public partial class projectEntitiesComplete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_funcion_SalaEntity_SalaId",
                table: "funcion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalaEntity",
                table: "SalaEntity");

            migrationBuilder.RenameTable(
                name: "SalaEntity",
                newName: "Sala");

            migrationBuilder.AddColumn<string>(
                name: "Cod",
                table: "Sala",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Sala",
                table: "Sala",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "AsientoEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    fila = table.Column<string>(type: "TEXT", nullable: false),
                    numero = table.Column<int>(type: "INTEGER", nullable: false),
                    SalaId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsientoEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsientoEntity_Sala_SalaId",
                        column: x => x.SalaId,
                        principalTable: "Sala",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsuarioEntity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsuarioEntity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reserva",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Cod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Fecha = table.Column<DateTime>(type: "TEXT", nullable: false),
                    FuncionId = table.Column<long>(type: "INTEGER", nullable: false),
                    UsuarioId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserva_UsuarioEntity_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "UsuarioEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserva_funcion_FuncionId",
                        column: x => x.FuncionId,
                        principalTable: "funcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reserva_asiento",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ReservaId = table.Column<long>(type: "INTEGER", nullable: false),
                    AsientoId = table.Column<long>(type: "INTEGER", nullable: false),
                    FuncionId = table.Column<long>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reserva_asiento", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reserva_asiento_AsientoEntity_AsientoId",
                        column: x => x.AsientoId,
                        principalTable: "AsientoEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserva_asiento_Reserva_ReservaId",
                        column: x => x.ReservaId,
                        principalTable: "Reserva",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reserva_asiento_funcion_FuncionId",
                        column: x => x.FuncionId,
                        principalTable: "funcion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sala_Cod",
                table: "Sala",
                column: "Cod",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AsientoEntity_SalaId",
                table: "AsientoEntity",
                column: "SalaId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_FuncionId",
                table: "Reserva",
                column: "FuncionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_UsuarioId",
                table: "Reserva",
                column: "UsuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_asiento_AsientoId",
                table: "Reserva_asiento",
                column: "AsientoId");

            migrationBuilder.CreateIndex(
                name: "IX_Reserva_asiento_ReservaId",
                table: "Reserva_asiento",
                column: "ReservaId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservaAsiento_FuncionId_AsientoId",
                table: "Reserva_asiento",
                columns: new[] { "FuncionId", "AsientoId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_funcion_Sala_SalaId",
                table: "funcion",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_funcion_Sala_SalaId",
                table: "funcion");

            migrationBuilder.DropTable(
                name: "Reserva_asiento");

            migrationBuilder.DropTable(
                name: "AsientoEntity");

            migrationBuilder.DropTable(
                name: "Reserva");

            migrationBuilder.DropTable(
                name: "UsuarioEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Sala",
                table: "Sala");

            migrationBuilder.DropIndex(
                name: "IX_Sala_Cod",
                table: "Sala");

            migrationBuilder.DropColumn(
                name: "Cod",
                table: "Sala");

            migrationBuilder.RenameTable(
                name: "Sala",
                newName: "SalaEntity");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalaEntity",
                table: "SalaEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_funcion_SalaEntity_SalaId",
                table: "funcion",
                column: "SalaId",
                principalTable: "SalaEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
