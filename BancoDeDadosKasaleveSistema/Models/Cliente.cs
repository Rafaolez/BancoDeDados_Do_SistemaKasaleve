using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Cliente")]
    public class Cliente
    {
        [Column("ClienteId")]
        [Display(Name = "ClienteId")]
        public int ClienteId { get; set; }

        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Column("cpfCnpj")]
        [StringLength(20)]
        [Display(Name = "CPF/CNPJ")]
        public string? CpfCnpj { get; set; }

        [Column("telefone")]
        [StringLength(20)]
        public string? Telefone { get; set; }

        [Column("endereco")]
        [StringLength(255)]
        [Display(Name = "Endereço")]
        public string? Endereco { get; set; }

        [Column("cidade")]
        [StringLength(100)]
        public string? Cidade { get; set; }

        [Column("estado")]
        [StringLength(2)]
        public string? Estado { get; set; }

        [Column("cep")]
        [StringLength(10)]
        [Display(Name = "CEP")]
        public string? Cep { get; set; }

        [Column("dataCadastro")] public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Column("status")]
        [StringLength(30)]
        public string Status { get; set; } = "Ativo";

        [Column("usuarioId")]
        [Display(Name = "Usuário responsável")]
        public int? UsuarioId { get; set; }
        [ForeignKey(nameof(UsuarioId))] public Usuario? Usuario { get; set; }

        public ICollection<Orcamento>? Orcamentos { get; set; }
    }
}
