using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("FibraCor")]
    public class FibraCor
    {
        [Column("FibraCorId")]
        [Display(Name = "FibraCorId")]
        public int FibraCorId { get; set; }

        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Column("sku")]
        [StringLength(50)]
        public string? Sku { get; set; }

        [Column("hexCor")]
        [StringLength(7)]
        [Display(Name = "Cor (hex)")]
        public string? HexCor { get; set; }

        public ICollection<ProdutoVariacao>? Variacoes { get; set; }

    }
}
