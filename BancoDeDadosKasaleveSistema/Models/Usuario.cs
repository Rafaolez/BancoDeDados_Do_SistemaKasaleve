using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BancoDeDadosKasaleveSistema.Models
{
    [Table("Usuario")]
    public class Usuario
    {
        [Column("UsuarioId")]
        [Display(Name = "UsuarioId")]
        public int UsuarioId { get; set; }

        [Column("nome")]
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        [Column("email")]
        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "Formato de email inválido.")]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        [Column("telefone")]
        [StringLength(20)]
        public string? Telefone { get; set; }

        [Column("senhaHash")]
        [Microsoft.AspNetCore.Mvc.ModelBinding.BindNever]
        [Microsoft.AspNetCore.Mvc.ModelBinding.Validation.ValidateNever]
        [StringLength(255)]
        public string SenhaHash { get; set; } = string.Empty;

        /// <summary>Só recebe a senha em texto puro vinda do formulário — NUNCA é gravada (NotMapped).</summary>
        [NotMapped]
        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [Column("dataCadastro")] public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Column("cargoId")]
        [Required(ErrorMessage = "O cargo é obrigatório.")]
        public int CargoId { get; set; }

        [ForeignKey(nameof(CargoId))] public Cargo? Cargo { get; set; }

        [Column("status")]
        [StringLength(30)]
        public string Status { get; set; } = "Ativo";

        public ICollection<Cliente>? ClientesResponsavel { get; set; }
        public ICollection<Orcamento>? Orcamentos { get; set; }
        public ICollection<MovimentacaoEstoque>? Movimentacoes { get; set; }
        public ICollection<Checklist>? Checklists { get; set; }
    }
}
