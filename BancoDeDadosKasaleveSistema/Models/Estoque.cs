using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Estoque")]
    public class Estoque
    {

        [Column("EstoqueId")]
        [Display(Name = "EstoqueId")]
        public int EstoqueId { get; set; }

        [Column("produtoVariacaoId")]
        [Required]
        public int ProdutoVariacaoId { get; set; }
        [ForeignKey(nameof(ProdutoVariacaoId))] public ProdutoVariacao? ProdutoVariacao { get; set; }

        [Column("localizacao")]
        [Required(ErrorMessage = "Informe a localização.")]
        [StringLength(100)]
        [Display(Name = "Localização")]
        public string? Localizacao { get; set; }

        [Column("quantidade")] public int Quantidade { get; set; } = 0;

        [Column("dataModificacao")]
        [Display(Name = "Última modificação")]
        public DateTime DataModificacao { get; set; } = DateTime.Now;

        [Column("estoqueMinimo")]
        [Display(Name = "Estoque mínimo")]
        public int EstoqueMinimo { get; set; } = 0;

        [NotMapped]
        public string DescricaoCompleta => $"{ProdutoVariacao?.DescricaoCompleta} — {Localizacao} (saldo: {Quantidade})";

        public ICollection<MovimentacaoEstoque>? Movimentacoes { get; set; }

    }
}
