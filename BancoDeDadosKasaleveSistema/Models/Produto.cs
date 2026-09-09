using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Produto")]
    public class Produto
    {
        [Column("ProdutoId")]
        [Display(Name = "ProdutoId")]
        public int ProdutoId { get; set; }

        [Column("categoriaId")] public int? CategoriaId { get; set; }
        [ForeignKey(nameof(CategoriaId))] public Categoria? Categoria { get; set; }

        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Column("descricao")]
        public string? Descricao { get; set; }

        [Column("valorLogista")]
        [Display(Name = "Valor logista")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor logista não pode ser negativo.")]
        public decimal ValorLogista { get; set; }

        [Column("valorFinal")]
        [Display(Name = "Valor final")]
        [Required(ErrorMessage = "O valor final é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor final não pode ser negativo.")]
        public decimal ValorFinal { get; set; }

        /// <summary>Caminho/URL da imagem principal do produto (não é um blob — é só o caminho do arquivo).</summary>
        [Column("img")]
        [StringLength(500)]
        [Display(Name = "Imagem")]
        public string? Img { get; set; }

        [Column("dataCadastro")] public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Column("sku")]
        [StringLength(50)]
        public string? Sku { get; set; }

        [Column("estoqueMinimo")]
        [Display(Name = "Estoque mínimo sugerido")]
        public int EstoqueMinimo { get; set; } = 0;

        [Column("ativo")] public bool Ativo { get; set; } = true;

        public ICollection<ProdutoVariacao>? Variacoes { get; set; }
    }
}
