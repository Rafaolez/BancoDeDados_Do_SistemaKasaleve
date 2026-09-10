using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Checklist")]
    public class Checklist
    {
        [Column("ChecklistId")]
        [Display(Name = "ChecklistId")]
        public int ChecklistId { get; set; }

        [Column("OrcamentoId")]
        public int? OrcamentoId { get; set; }
        [ForeignKey(nameof(OrcamentoId))] public Orcamento? Orcamento { get; set; }

        [Column("ChecklistenderecoEntrega")]
        [StringLength(255)]
        [Display(Name = "Endereço de entrega")]
        public string? ChecklistEnderecoEntrega { get; set; }

        [Column("bairro")]
        [StringLength(100)]
        public string? Bairro { get; set; }

        [Column("cidade")]
        [StringLength(100)]
        public string? Cidade { get; set; }

        [Column("cep")]
        [StringLength(10)]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [Column("conferirProduto")]
        [Display(Name = "Conferir produto")]
        public bool ConferirProduto { get; set; }

        [Column("cores")]
        [StringLength(255)]
        public string? Cores { get; set; }

        [Column("mesaComFuro")]
        [Display(Name = "Mesa com furo")]
        public bool MesaComFuro { get; set; }

        [Column("alteracao")]
        [StringLength(500)]
        [Display(Name = "Alteração")]
        public string? Alteracao { get; set; }

        [Column("imprimir")] public bool Imprimir { get; set; }

        [Column("nota")]
        [StringLength(500)]
        public string? Nota { get; set; }

        [Column("prazo")] public DateTime? Prazo { get; set; }

        [Column("frete")]
        [StringLength(100)]
        public string? Frete { get; set; }

        [Column("previsaoEntrega")]
        [Display(Name = "Previsão de entrega")]
        public DateTime? PrevisaoEntrega { get; set; }

        [Column("usuarioId")]
        [Required]
        public int UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public Usuario? Usuario { get; set; }

        [Column("dataConclusao")]
        [Display(Name = "Data de conclusão")]
        public DateTime? DataConclusao { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string Status { get; set; } = "Pendente";
    }
}
