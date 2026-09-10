using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BancoDeDadosKasaleveSistema.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AluminioCor",
                columns: table => new
                {
                    AluminioCorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CorAluminioCor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CodigoAluminioCor = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SKUAluminioCor = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    hexCor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AluminioCor", x => x.AluminioCorId);
                });

            migrationBuilder.CreateTable(
                name: "Cargo",
                columns: table => new
                {
                    CargoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CargoNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CargoDescricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cargo", x => x.CargoId);
                });

            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    CategoriaId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoriaNome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoriaDescricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.CategoriaId);
                });

            migrationBuilder.CreateTable(
                name: "CordaCor",
                columns: table => new
                {
                    CordaCorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sku = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    hexCor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CordaCor", x => x.CordaCorId);
                });

            migrationBuilder.CreateTable(
                name: "FibraCor",
                columns: table => new
                {
                    FibraCorId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    hexCor = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FibraCor", x => x.FibraCorId);
                });

            migrationBuilder.CreateTable(
                name: "Tecido",
                columns: table => new
                {
                    TecidoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    sku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tecido", x => x.TecidoId);
                });

            migrationBuilder.CreateTable(
                name: "TipoMovimentacao",
                columns: table => new
                {
                    TipoMovimentacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    entrada = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TipoMovimentacao", x => x.TipoMovimentacaoId);
                });

            migrationBuilder.CreateTable(
                name: "Usuario",
                columns: table => new
                {
                    UsuarioId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    senhaHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    dataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cargoId = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuario", x => x.UsuarioId);
                    table.ForeignKey(
                        name: "FK_Usuario_Cargo_cargoId",
                        column: x => x.cargoId,
                        principalTable: "Cargo",
                        principalColumn: "CargoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Produto",
                columns: table => new
                {
                    ProdutoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    categoriaId = table.Column<int>(type: "int", nullable: true),
                    nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    valorLogista = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valorFinal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    img = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    dataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    sku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    estoqueMinimo = table.Column<int>(type: "int", nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Produto", x => x.ProdutoId);
                    table.ForeignKey(
                        name: "FK_Produto_Categoria_categoriaId",
                        column: x => x.categoriaId,
                        principalTable: "Categoria",
                        principalColumn: "CategoriaId");
                });

            migrationBuilder.CreateTable(
                name: "Cliente",
                columns: table => new
                {
                    ClienteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nome = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    cpfCnpj = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    endereco = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    estado = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    dataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cliente", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_Cliente_Usuario_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "Historico",
                columns: table => new
                {
                    HistoricoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    usuarioId = table.Column<int>(type: "int", nullable: true),
                    entidade = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    entidadeId = table.Column<int>(type: "int", nullable: false),
                    acao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    dataHora = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historico", x => x.HistoricoId);
                    table.ForeignKey(
                        name: "FK_Historico_Usuario_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateTable(
                name: "ProdutoVariacao",
                columns: table => new
                {
                    ProdutoVariacaoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produtoId = table.Column<int>(type: "int", nullable: false),
                    aluminioCorId = table.Column<int>(type: "int", nullable: true),
                    cordaCorId = table.Column<int>(type: "int", nullable: true),
                    fibraCorId = table.Column<int>(type: "int", nullable: true),
                    tecidoId = table.Column<int>(type: "int", nullable: true),
                    sku = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ativo = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProdutoVariacao", x => x.ProdutoVariacaoId);
                    table.ForeignKey(
                        name: "FK_ProdutoVariacao_AluminioCor_aluminioCorId",
                        column: x => x.aluminioCorId,
                        principalTable: "AluminioCor",
                        principalColumn: "AluminioCorId");
                    table.ForeignKey(
                        name: "FK_ProdutoVariacao_CordaCor_cordaCorId",
                        column: x => x.cordaCorId,
                        principalTable: "CordaCor",
                        principalColumn: "CordaCorId");
                    table.ForeignKey(
                        name: "FK_ProdutoVariacao_FibraCor_fibraCorId",
                        column: x => x.fibraCorId,
                        principalTable: "FibraCor",
                        principalColumn: "FibraCorId");
                    table.ForeignKey(
                        name: "FK_ProdutoVariacao_Produto_produtoId",
                        column: x => x.produtoId,
                        principalTable: "Produto",
                        principalColumn: "ProdutoId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProdutoVariacao_Tecido_tecidoId",
                        column: x => x.tecidoId,
                        principalTable: "Tecido",
                        principalColumn: "TecidoId");
                });

            migrationBuilder.CreateTable(
                name: "Orcamento",
                columns: table => new
                {
                    OrcamentoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dataCriacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    validade = table.Column<DateTime>(type: "datetime2", nullable: true),
                    clienteId = table.Column<int>(type: "int", nullable: false),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    subTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    frete = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    obs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    dataAlteracao = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orcamento", x => x.OrcamentoId);
                    table.ForeignKey(
                        name: "FK_Orcamento_Cliente_clienteId",
                        column: x => x.clienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orcamento_Usuario_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Estoque",
                columns: table => new
                {
                    EstoqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    produtoVariacaoId = table.Column<int>(type: "int", nullable: false),
                    localizacao = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    dataModificacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    estoqueMinimo = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Estoque", x => x.EstoqueId);
                    table.ForeignKey(
                        name: "FK_Estoque_ProdutoVariacao_produtoVariacaoId",
                        column: x => x.produtoVariacaoId,
                        principalTable: "ProdutoVariacao",
                        principalColumn: "ProdutoVariacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Checklist",
                columns: table => new
                {
                    ChecklistId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrcamentoId = table.Column<int>(type: "int", nullable: true),
                    ChecklistenderecoEntrega = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    bairro = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cidade = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    cep = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    conferirProduto = table.Column<bool>(type: "bit", nullable: false),
                    cores = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    mesaComFuro = table.Column<bool>(type: "bit", nullable: false),
                    alteracao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    imprimir = table.Column<bool>(type: "bit", nullable: false),
                    nota = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    prazo = table.Column<DateTime>(type: "datetime2", nullable: true),
                    frete = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    previsaoEntrega = table.Column<DateTime>(type: "datetime2", nullable: true),
                    usuarioId = table.Column<int>(type: "int", nullable: false),
                    dataConclusao = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Checklist", x => x.ChecklistId);
                    table.ForeignKey(
                        name: "FK_Checklist_Orcamento_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamento",
                        principalColumn: "OrcamentoId");
                    table.ForeignKey(
                        name: "FK_Checklist_Usuario_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrcamentoItem",
                columns: table => new
                {
                    OrcamentoItemId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    OrcamentoId = table.Column<int>(type: "int", nullable: true),
                    produtoVariacaoId = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    valorUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valorExtra = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    desconto = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    valorTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    obs = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    nomeProdutoSnapshot = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrcamentoItem", x => x.OrcamentoItemId);
                    table.ForeignKey(
                        name: "FK_OrcamentoItem_Orcamento_OrcamentoId",
                        column: x => x.OrcamentoId,
                        principalTable: "Orcamento",
                        principalColumn: "OrcamentoId");
                    table.ForeignKey(
                        name: "FK_OrcamentoItem_ProdutoVariacao_produtoVariacaoId",
                        column: x => x.produtoVariacaoId,
                        principalTable: "ProdutoVariacao",
                        principalColumn: "ProdutoVariacaoId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovimentacaoEstoque",
                columns: table => new
                {
                    MovimentacaoEstoqueId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    estoqueId = table.Column<int>(type: "int", nullable: false),
                    tipoMovimentacaoId = table.Column<int>(type: "int", nullable: false),
                    quantidade = table.Column<int>(type: "int", nullable: false),
                    saldoAnterior = table.Column<int>(type: "int", nullable: false),
                    saldoPosterior = table.Column<int>(type: "int", nullable: false),
                    motivo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    orcamentoId = table.Column<int>(type: "int", nullable: true),
                    dataMovimentacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    obs = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    usuarioId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovimentacaoEstoque", x => x.MovimentacaoEstoqueId);
                    table.ForeignKey(
                        name: "FK_MovimentacaoEstoque_Estoque_estoqueId",
                        column: x => x.estoqueId,
                        principalTable: "Estoque",
                        principalColumn: "EstoqueId");
                    table.ForeignKey(
                        name: "FK_MovimentacaoEstoque_Orcamento_orcamentoId",
                        column: x => x.orcamentoId,
                        principalTable: "Orcamento",
                        principalColumn: "OrcamentoId");
                    table.ForeignKey(
                        name: "FK_MovimentacaoEstoque_TipoMovimentacao_tipoMovimentacaoId",
                        column: x => x.tipoMovimentacaoId,
                        principalTable: "TipoMovimentacao",
                        principalColumn: "TipoMovimentacaoId");
                    table.ForeignKey(
                        name: "FK_MovimentacaoEstoque_Usuario_usuarioId",
                        column: x => x.usuarioId,
                        principalTable: "Usuario",
                        principalColumn: "UsuarioId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_OrcamentoId",
                table: "Checklist",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_Checklist_usuarioId",
                table: "Checklist",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Cliente_usuarioId",
                table: "Cliente",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Estoque_produtoVariacaoId",
                table: "Estoque",
                column: "produtoVariacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Historico_usuarioId",
                table: "Historico",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_estoqueId_dataMovimentacao",
                table: "MovimentacaoEstoque",
                columns: new[] { "estoqueId", "dataMovimentacao" });

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_orcamentoId",
                table: "MovimentacaoEstoque",
                column: "orcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_tipoMovimentacaoId",
                table: "MovimentacaoEstoque",
                column: "tipoMovimentacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_MovimentacaoEstoque_usuarioId",
                table: "MovimentacaoEstoque",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamento_clienteId",
                table: "Orcamento",
                column: "clienteId");

            migrationBuilder.CreateIndex(
                name: "IX_Orcamento_usuarioId",
                table: "Orcamento",
                column: "usuarioId");

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoItem_OrcamentoId",
                table: "OrcamentoItem",
                column: "OrcamentoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrcamentoItem_produtoVariacaoId",
                table: "OrcamentoItem",
                column: "produtoVariacaoId");

            migrationBuilder.CreateIndex(
                name: "IX_Produto_categoriaId",
                table: "Produto",
                column: "categoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoVariacao_aluminioCorId",
                table: "ProdutoVariacao",
                column: "aluminioCorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoVariacao_cordaCorId",
                table: "ProdutoVariacao",
                column: "cordaCorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoVariacao_fibraCorId",
                table: "ProdutoVariacao",
                column: "fibraCorId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoVariacao_produtoId",
                table: "ProdutoVariacao",
                column: "produtoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProdutoVariacao_tecidoId",
                table: "ProdutoVariacao",
                column: "tecidoId");

            migrationBuilder.CreateIndex(
                name: "IX_Usuario_cargoId",
                table: "Usuario",
                column: "cargoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Checklist");

            migrationBuilder.DropTable(
                name: "Historico");

            migrationBuilder.DropTable(
                name: "MovimentacaoEstoque");

            migrationBuilder.DropTable(
                name: "OrcamentoItem");

            migrationBuilder.DropTable(
                name: "Estoque");

            migrationBuilder.DropTable(
                name: "TipoMovimentacao");

            migrationBuilder.DropTable(
                name: "Orcamento");

            migrationBuilder.DropTable(
                name: "ProdutoVariacao");

            migrationBuilder.DropTable(
                name: "Cliente");

            migrationBuilder.DropTable(
                name: "AluminioCor");

            migrationBuilder.DropTable(
                name: "CordaCor");

            migrationBuilder.DropTable(
                name: "FibraCor");

            migrationBuilder.DropTable(
                name: "Produto");

            migrationBuilder.DropTable(
                name: "Tecido");

            migrationBuilder.DropTable(
                name: "Usuario");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Cargo");
        }
    }
}
