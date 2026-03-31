using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiCine.Migrations
{
    /// <inheritdoc />
    public partial class AddAtributeInSala : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AsientoEntity_Sala_SalaId",
                table: "AsientoEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_UsuarioEntity_UsuarioId",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_asiento_AsientoEntity_AsientoId",
                table: "Reserva_asiento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UsuarioEntity",
                table: "UsuarioEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AsientoEntity",
                table: "AsientoEntity");

            migrationBuilder.RenameTable(
                name: "UsuarioEntity",
                newName: "Usuario");

            migrationBuilder.RenameTable(
                name: "AsientoEntity",
                newName: "Asiento");

            migrationBuilder.RenameColumn(
                name: "numero",
                table: "Asiento",
                newName: "Numero");

            migrationBuilder.RenameColumn(
                name: "fila",
                table: "Asiento",
                newName: "Fila");

            migrationBuilder.RenameIndex(
                name: "IX_AsientoEntity_SalaId",
                table: "Asiento",
                newName: "IX_Asiento_SalaId");

            migrationBuilder.AddColumn<int>(
                name: "CantidadColumnas",
                table: "Sala",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CantidadFilas",
                table: "Sala",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Sala",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Usuario",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Usuario",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Usuario",
                type: "TEXT",
                maxLength: 255,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Usuario",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Usuario",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Asiento",
                table: "Asiento",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Email",
                table: "Usuario",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_Username",
                table: "Usuario",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Asiento_Sala_SalaId",
                table: "Asiento",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_Usuario_UsuarioId",
                table: "Reserva",
                column: "UsuarioId",
                principalTable: "Usuario",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_asiento_Asiento_AsientoId",
                table: "Reserva_asiento",
                column: "AsientoId",
                principalTable: "Asiento",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Asiento_Sala_SalaId",
                table: "Asiento");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_Usuario_UsuarioId",
                table: "Reserva");

            migrationBuilder.DropForeignKey(
                name: "FK_Reserva_asiento_Asiento_AsientoId",
                table: "Reserva_asiento");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Usuario",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_Email",
                table: "Usuario");

            migrationBuilder.DropIndex(
                name: "IX_Usuario_Username",
                table: "Usuario");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Asiento",
                table: "Asiento");

            migrationBuilder.DropColumn(
                name: "CantidadColumnas",
                table: "Sala");

            migrationBuilder.DropColumn(
                name: "CantidadFilas",
                table: "Sala");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Sala");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Password",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Usuario");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Usuario");

            migrationBuilder.RenameTable(
                name: "Usuario",
                newName: "UsuarioEntity");

            migrationBuilder.RenameTable(
                name: "Asiento",
                newName: "AsientoEntity");

            migrationBuilder.RenameColumn(
                name: "Numero",
                table: "AsientoEntity",
                newName: "numero");

            migrationBuilder.RenameColumn(
                name: "Fila",
                table: "AsientoEntity",
                newName: "fila");

            migrationBuilder.RenameIndex(
                name: "IX_Asiento_SalaId",
                table: "AsientoEntity",
                newName: "IX_AsientoEntity_SalaId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UsuarioEntity",
                table: "UsuarioEntity",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AsientoEntity",
                table: "AsientoEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AsientoEntity_Sala_SalaId",
                table: "AsientoEntity",
                column: "SalaId",
                principalTable: "Sala",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_UsuarioEntity_UsuarioId",
                table: "Reserva",
                column: "UsuarioId",
                principalTable: "UsuarioEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reserva_asiento_AsientoEntity_AsientoId",
                table: "Reserva_asiento",
                column: "AsientoId",
                principalTable: "AsientoEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
