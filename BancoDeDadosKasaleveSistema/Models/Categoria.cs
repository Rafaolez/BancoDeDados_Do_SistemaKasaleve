using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Categoria")]
    public class Categoria
    {

        [Column("CategoriaId")]
        public int CategoriaId { get; set; }

        [Column("CategoriaNome")]
        [Display(Name = "Qual a Categoria?")]
        public string CategoriaNome { get; set; } = null!;

        [Column("CategoriaDescricao")]
        [Display(Name = "Descrição da Categoria")]
        public string CategoriaDescricao { get; set; } = null!;

        [Column("ativo")] public bool Ativo { get; set; } = true;

    }
}
