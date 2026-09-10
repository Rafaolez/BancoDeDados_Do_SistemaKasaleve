using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("OrcamentoItem")]
    public class OrcamentoItem
    {
        [Column("OrcamentoItemId")]
        [Display(Name = "OrcamentoItemId")]
        public string OrcamentoItemId { get; set; } = Guid.NewGuid().ToString("N");

        [Column("OrcamentoId")]
        public int? OrcamentoId { get; set; }
        [ForeignKey(nameof(OrcamentoId))] public Orcamento? Orcamento { get; set; }

        [Column("produtoVariacaoId")]
        [Required(ErrorMessage = "O produto/variação é obrigatório.")]
        [Display(Name = "Produto / variação")]
        public int ProdutoVariacaoId { get; set; }
        [ForeignKey(nameof(ProdutoVariacaoId))] public ProdutoVariacao? ProdutoVariacao { get; set; }

        [Column("quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; } = 1;

        [Column("valorUnitario")]
        [Display(Name = "Valor unitário")]
        public decimal ValorUnitario { get; set; }

        [Column("valorExtra")]
        [Display(Name = "Valor extra")]
        public decimal ValorExtra { get; set; }

        [Column("desconto")] public decimal Desconto { get; set; }

        [Column("valorTotal")]
        [Display(Name = "Valor total")]
        public decimal ValorTotal { get; set; }

        [Column("obs")]
        [StringLength(500)]
        [Display(Name = "Observações")]
        public string? Obs { get; set; }

        /// <summary>Nome do produto "congelado" no momento da venda — se o produto for
        /// renomeado/excluído depois, o orçamento antigo continua mostrando o nome certo.</summary>
        [Column("nomeProdutoSnapshot")]
        [StringLength(255)]
        [Display(Name = "Nome do produto (no momento da venda)")]
        public string? NomeProdutoSnapshot { get; set; }
    }
}
