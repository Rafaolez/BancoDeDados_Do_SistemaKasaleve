using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDeDadosKasaleveSistema.Migrations
{
    /// <inheritdoc />
    public partial class EstoquePorLocalETransferencias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_Usuario_usuarioId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoque_ProdutoVariacao_produtoVariacaoId",
                table: "Estoque");

            migrationBuilder.DropIndex(
                name: "IX_Estoque_produtoVariacaoId",
                table: "Estoque");

            migrationBuilder.AddColumn<string>(
                name: "descricaoVariacao",
                table: "MovimentacaoEstoque",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "localizacaoRegistro",
                table: "MovimentacaoEstoque",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "transferenciaId",
                table: "MovimentacaoEstoque",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("UPDATE Estoque SET localizacao = COALESCE(NULLIF(LTRIM(RTRIM(localizacao)), ''), N'Não informado');");

            migrationBuilder.AlterColumn<string>(
                name: "localizacao",
                table: "Estoque",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_transferenciaId",
                table: "MovimentacaoEstoque",
                column: "transferenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_produtoVariacaoId_localizacao",
                table: "Estoque",
                columns: new[] { "produtoVariacaoId", "localizacao" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_Usuario_usuarioId",
                table: "Checklist",
                column: "usuarioId",
                principalTable: "Usuario",
                principalColumn: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Estoque_ProdutoVariacao_produtoVariacaoId",
                table: "Estoque",
                column: "produtoVariacaoId",
                principalTable: "ProdutoVariacao",
                principalColumn: "ProdutoVariacaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Checklist_Usuario_usuarioId",
                table: "Checklist");

            migrationBuilder.DropForeignKey(
                name: "FK_Estoque_ProdutoVariacao_produtoVariacaoId",
                table: "Estoque");

            migrationBuilder.DropIndex(
                name: "IX_MovimentacaoEstoque_transferenciaId",
                table: "MovimentacaoEstoque");

            migrationBuilder.DropIndex(
                name: "IX_Estoque_produtoVariacaoId_localizacao",
                table: "Estoque");

            migrationBuilder.DropColumn(
                name: "descricaoVariacao",
                table: "MovimentacaoEstoque");

            migrationBuilder.DropColumn(
                name: "localizacaoRegistro",
                table: "MovimentacaoEstoque");

            migrationBuilder.DropColumn(
                name: "transferenciaId",
                table: "MovimentacaoEstoque");

            migrationBuilder.AlterColumn<string>(
                name: "localizacao",
                table: "Estoque",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_produtoVariacaoId",
                table: "Estoque",
                column: "produtoVariacaoId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Checklist_Usuario_usuarioId",
                table: "Checklist",
                column: "usuarioId",
                principalTable: "Usuario",
                principalColumn: "UsuarioId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estoque_ProdutoVariacao_produtoVariacaoId",
                table: "Estoque",
                column: "produtoVariacaoId",
                principalTable: "ProdutoVariacao",
                principalColumn: "ProdutoVariacaoId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
