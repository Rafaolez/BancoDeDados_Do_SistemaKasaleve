using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Cargo")]
    public class Cargo
    {
        [Column("CargoId")]
        public int CargoId { get; set; }

        [Column("CargoNome")]
        [Display(Name = "Qual o seu Cargo?")]
        public string CargoNome { get; set; } = null!;

        [Column("CargoDescricao")]
        [Display(Name = "Descrição do Cargo")]
        public string CargoDescricao { get; set; } = null!;

        [Column("ativo")] public bool Ativo { get; set; } = true;

    }
}
