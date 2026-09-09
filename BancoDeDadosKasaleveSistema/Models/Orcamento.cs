using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Orcamento")]
    public class Orcamento
    {
        [Column("OrcamentoId")]
        [Display(Name = "OrcamentoId")]
        public int OrcamentoId { get; set; }

        [Column("dataCriacao")] public DateTime DataCriacao { get; set; } = DateTime.Now;

        [Column("validade")]
        [Display(Name = "Válido até")]
        public DateTime? Validade { get; set; }

        [Column("clienteId")]
        [Required(ErrorMessage = "O cliente é obrigatório.")]
        public int ClienteId { get; set; }
        [ForeignKey(nameof(ClienteId))] public Cliente? Cliente { get; set; }

        [Column("usuarioId")]
        [Required(ErrorMessage = "O usuário é obrigatório.")]
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public Usuario? Usuario { get; set; }

        [Column("subTotal")]
        [Display(Name = "Subtotal")]
        public decimal SubTotal { get; set; }

        [Column("desconto")] public decimal Desconto { get; set; }

        [Column("frete")] public decimal Frete { get; set; }

        [Column("total")] public decimal Total { get; set; }

        [Column("obs")]
        [Display(Name = "Observações")]
        public string? Obs { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "Em aberto";

        [Column("dataAlteracao")]
        [Display(Name = "Última alteração")]
        public DateTime? DataAlteracao { get; set; }

        public ICollection<OrcamentoItem>? Itens { get; set; }
        public ICollection<Checklist>? Checklists { get; set; }

    }
}
