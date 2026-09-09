using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("TipoMovimentacao")]
    public class TipoMovimentacao
    {
        [Column("TipoMovimentacaoId")]
        [Display(Name = "TipoMovimentacaoId")]
        public int TipoMovimentacaoId { get; set; }
        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(50)]
        public string Nome { get; set; } = string.Empty;

        [Column("descricao")]
        [StringLength(500)]
        public string? Descricao { get; set; }

        /// <summary>
        /// Não estava no seu desenho, mas é preciso pra saber se a movimentação soma
        /// ou subtrai do Estoque.Quantidade — sem isso o sistema teria que "adivinhar"
        /// pelo Nome (ex.: string == "Entrada"), o que quebra se alguém renomear o tipo.
        /// True = soma no estoque (entrada); False = subtrai (saída).
        /// </summary>
        [Column("entrada")]
        [Display(Name = "É entrada (soma no estoque)?")]
        public bool Entrada { get; set; } = true;

        public ICollection<MovimentacaoEstoque>? Movimentacoes { get; set; }

    }
}
