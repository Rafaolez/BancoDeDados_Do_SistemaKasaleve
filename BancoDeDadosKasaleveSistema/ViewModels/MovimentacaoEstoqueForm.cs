using System.ComponentModel.DataAnnotations;

namespace BancoDeDadosKasaleveSistema.ViewModels;

public class MovimentacaoEstoqueForm
{
    [Range(1, int.MaxValue), Display(Name = "Estoque")]
    public int EstoqueId { get; set; }

    [Range(1, int.MaxValue), Display(Name = "Tipo de movimentação")]
    public int TipoMovimentacaoId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Informe uma quantidade maior que zero.")]
    public int Quantidade { get; set; } = 1;

    [Required(ErrorMessage = "Informe o motivo."), StringLength(100)]
    public string Motivo { get; set; } = string.Empty;

    [Display(Name = "Orçamento (opcional)")]
    public int? OrcamentoId { get; set; }

    [Range(1, int.MaxValue), Display(Name = "Responsável")]
    public int UsuarioId { get; set; }

    [StringLength(500), Display(Name = "Observações")]
    public string? Obs { get; set; }
}
