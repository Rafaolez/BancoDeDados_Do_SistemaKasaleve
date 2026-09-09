using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("CordaCor")]
    public class CordaCor
    {
        [Column("CordaCorId")]
        [Display(Name = "CordaCorId")]
        public int CordaCorId { get; set; }

        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Column("sku")]
        [StringLength(3)]
        [Display(Name = "SKU")]
        public string? Sku { get; set; }

        [Column("hexCor")]
        [StringLength(7)]
        [Display(Name = "Cor (hex)")]
        public string? HexCor { get; set; }

        public ICollection<ProdutoVariacao>? Variacoes { get; set; }
    }
}
