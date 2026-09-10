using System.ComponentModel.DataAnnotations;

namespace BancoDeDadosKasaleveSistema.ViewModels;

public class TransferenciaEstoqueForm
{
    [Range(1, int.MaxValue), Display(Name = "Estoque de origem")]
    public int EstoqueOrigemId { get; set; }
    [Range(1, int.MaxValue), Display(Name = "Estoque de destino (mesma variação)")]
    public int EstoqueDestinoId { get; set; }
    [Range(1, int.MaxValue)]
    public int Quantidade { get; set; } = 1;
    [Range(1, int.MaxValue), Display(Name = "Responsável")]
    public int UsuarioId { get; set; }
    [Required, StringLength(100)]
    public string Motivo { get; set; } = "Transferência entre locais";
    [StringLength(500), Display(Name = "Observações")]
    public string? Obs { get; set; }
}
