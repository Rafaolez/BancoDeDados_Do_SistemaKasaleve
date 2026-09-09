using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Tecido")]
    public class Tecido
    {
        [Column("TecidoId")]
        [Display(Name = "TecidoId")]
        public int TecidoId { get; set; }

        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100)]
        public string Nome { get; set; } = string.Empty;

        [Column("sku")]
        [StringLength(50)]
        public string? Sku { get; set; }

        public ICollection<ProdutoVariacao>? Variacoes { get; set; }
    }
}
