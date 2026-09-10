using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("MovimentacaoEstoque")]
    public class MovimentacaoEstoque
    {
        [Column("MovimentacaoEstoqueId")]
        [Display(Name = "MovimentacaoEstoqueId")]
        public int MovimentacaoEstoqueId { get; set; }

        [Column("transferenciaId")]
        public Guid? TransferenciaId { get; set; }

        [Column("descricaoVariacao"), StringLength(1000)]
        public string? DescricaoVariacao { get; set; }

        [Column("localizacaoRegistro"), StringLength(100)]
        public string? LocalizacaoRegistro { get; set; }

        [Column("estoqueId")]
        [Required]
        public int EstoqueId { get; set; }
        [ForeignKey(nameof(EstoqueId))] public Estoque? Estoque { get; set; }

        [Column("tipoMovimentacaoId")]
        [Required(ErrorMessage = "O tipo de movimentação é obrigatório.")]
        public int TipoMovimentacaoId { get; set; }
        [ForeignKey(nameof(TipoMovimentacaoId))] public TipoMovimentacao? TipoMovimentacao { get; set; }

        [Column("quantidade")]
        [Range(1, int.MaxValue, ErrorMessage = "A quantidade deve ser maior que zero.")]
        public int Quantidade { get; set; }

        [Column("saldoAnterior")]
        [Range(0, int.MaxValue, ErrorMessage = "O saldo anterior não pode ser negativo.")]
        [Display(Name = "Saldo anterior")]
        public int SaldoAnterior { get; set; }

        [Column("saldoPosterior")]
        [Range(0, int.MaxValue, ErrorMessage = "O saldo posterior não pode ser negativo.")]
        [Display(Name = "Saldo posterior")]
        public int SaldoPosterior { get; set; }

        [Column("motivo")]
        [Required(ErrorMessage = "O motivo da movimentação é obrigatório.")]
        [StringLength(100)]
        [Display(Name = "Motivo da movimentação")]
        public string Motivo { get; set; } = string.Empty;

        [Column("orcamentoId")]
        [Display(Name = "Orçamento")]
        public int? OrcamentoId { get; set; }
        [ForeignKey(nameof(OrcamentoId))] public Orcamento? Orcamento { get; set; }

        [Column("dataMovimentacao")] public DateTime DataMovimentacao { get; set; } = DateTime.Now;

        [Column("obs")]
        [StringLength(500)]
        [Display(Name = "Observações")]
        public string? Obs { get; set; }

        [Column("usuarioId")]
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public Usuario? Usuario { get; set; }
    }
}
