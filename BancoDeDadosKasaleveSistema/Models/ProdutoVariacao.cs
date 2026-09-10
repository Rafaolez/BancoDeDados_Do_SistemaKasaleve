using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("ProdutoVariacao")]
    public class ProdutoVariacao
    {
        [Column("ProdutoVariacaoId")]
        [Display(Name = "ProdutoVariacaoId")]
        public int ProdutoVariacaoId { get; set; }

        [Column("produtoId")]
        [Required(ErrorMessage = "O produto é obrigatório.")]
        public int ProdutoId { get; set; }
        [ForeignKey(nameof(ProdutoId))] public Produto? Produto { get; set; }

        [Column("aluminioCorId")]
        [Display(Name = "Cor de alumínio")]
        public int? AluminioCorId { get; set; }
        [ForeignKey(nameof(AluminioCorId))] public AluminioCor? AluminioCor { get; set; }

        [Column("cordaCorId")]
        [Display(Name = "Cor de corda")]
        public int? CordaCorId { get; set; }
        [ForeignKey(nameof(CordaCorId))] public CordaCor? CordaCor { get; set; }

        [Column("fibraCorId")]
        [Display(Name = "Cor de fibra")]
        public int? FibraCorId { get; set; }
        [ForeignKey(nameof(FibraCorId))] public FibraCor? FibraCor { get; set; }

        [Column("tecidoId")]
        [Display(Name = "Tecido")]
        public int? TecidoId { get; set; }
        [ForeignKey(nameof(TecidoId))] public Tecido? Tecido { get; set; }

        [Column("sku")]
        [Required(ErrorMessage = "O SKU é obrigatório.")]
        [StringLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Column("ativo")] public bool Ativo { get; set; } = true;

        public ICollection<Estoque> Estoques { get; set; } = new List<Estoque>();

        [NotMapped]
        public string DescricaoCompleta => string.Join(" | ", new[]
        {
            Produto?.Nome, Sku,
            AluminioCor == null ? null : "Alumínio: " + AluminioCor.CorAluminioCor,
            CordaCor == null ? null : "Corda: " + CordaCor.Nome,
            FibraCor == null ? null : "Fibra: " + FibraCor.Nome,
            Tecido == null ? null : "Tecido: " + Tecido.Nome
        }.Where(s => !string.IsNullOrWhiteSpace(s)));
        public ICollection<OrcamentoItem>? OrcamentoItens { get; set; }
    }
}
