using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Historico")]
    public class Historico
    {
        [Column("HistoricoId")]
        [Display(Name = "HistoricoId")]
        public int HistoricoId { get; set; }

        [Column("usuarioId")]
        public int? UsuarioId { get; set; }

        [ForeignKey(nameof(UsuarioId))]
        public Usuario? Usuario { get; set; }

        /// <summary>Nome da entidade afetada: "Cliente", "Produto", "Orcamento", "Checklist" etc.</summary>
        [Column("entidade")]
        [Required]
        [StringLength(50)]
        public string Entidade { get; set; } = string.Empty;

        [Column("entidadeId")]
        [Required]
        public int EntidadeId { get; set; }

        /// <summary>"Criado", "Alterado", "Excluido", "Finalizado" etc.</summary>
        [Column("acao")]
        [Required]
        [StringLength(50)]
        public string Acao { get; set; } = string.Empty;

        [Column("descricao")]
        [StringLength(500)]
        public string? Descricao { get; set; }

        [Column("dataHora")]
        public DateTime DataHora { get; set; } = DateTime.Now;
    }
}
