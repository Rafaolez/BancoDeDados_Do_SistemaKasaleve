using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("AluminioCor")]
    public class AluminioCor
    {
        [Column("AluminioCorId")]
        public int AluminioCorId { get; set; }

        [Column("CorAluminioCor")]
        [Display(Name = "Qual a cor do Aluminio?")]
        public string CorAluminioCor { get; set; } = null!;

        [Column("CodigoAluminioCor")]
        [Display(Name = "Código da cor")]
        public string Codigo { get; set; } = null!;

        [Column ("SKUAluminioCor")]
        [StringLength(4)]
        [Display(Name = "SKU")]
        public string SKUAluminioCor { get; set; } = null!;

        [Column("hexCor")]
        [StringLength(7)]
        [Display(Name = "Cor (hex)")]
        public string? HexCor { get; set; }
    }
}
