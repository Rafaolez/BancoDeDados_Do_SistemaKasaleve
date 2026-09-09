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
