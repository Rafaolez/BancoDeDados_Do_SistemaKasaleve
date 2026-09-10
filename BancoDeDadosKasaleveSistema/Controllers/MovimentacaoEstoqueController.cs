using System.ComponentModel.DataAnnotations;
using BancoDeDadosKasaleveSistema.Models;
using BancoDeDadosKasaleveSistema.Services;
using BancoDeDadosKasaleveSistema.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class MovimentacaoEstoqueController(Contexto context, EstoqueService estoqueService) : Controller
{
    private IQueryable<MovimentacaoEstoque> Historico() => context.MovimentacaoEstoque.AsNoTracking()
        .Include(m => m.Estoque).ThenInclude(e => e!.ProdutoVariacao)
        .Include(m => m.TipoMovimentacao).Include(m => m.Usuario).Include(m => m.Orcamento);

    public async Task<IActionResult> Index(int? estoqueId, DateTime? inicio, DateTime? fim, Guid? transferenciaId)
    {
        var query = Historico();
        if (transferenciaId.HasValue) query = query.Where(m => m.TransferenciaId == transferenciaId);
        ViewData["TransferenciaId"] = transferenciaId;
        if (estoqueId.HasValue) query = query.Where(m => m.EstoqueId == estoqueId);
        if (inicio.HasValue) query = query.Where(m => m.DataMovimentacao >= inicio.Value.Date);
        if (fim.HasValue && fim.Value.Date < DateTime.MaxValue.Date)
        {
            var limite = fim.Value.Date.AddDays(1);
            query = query.Where(m => m.DataMovimentacao < limite);
        }
        await LoadOptionsAsync(new MovimentacaoEstoqueForm { EstoqueId = estoqueId ?? 0 });
        ViewData["Inicio"] = inicio?.ToString("yyyy-MM-dd");
        ViewData["Fim"] = fim?.ToString("yyyy-MM-dd");
        return View(await query.OrderByDescending(m => m.DataMovimentacao).ThenByDescending(m => m.MovimentacaoEstoqueId).ToListAsync());
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await Historico().SingleOrDefaultAsync(m => m.MovimentacaoEstoqueId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create(int? estoqueId)
    {
        var model = new MovimentacaoEstoqueForm { EstoqueId = estoqueId ?? 0 };
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(MovimentacaoEstoqueForm model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var movimentacao = await estoqueService.RegistrarAsync(model);
                return RedirectToAction(nameof(Details), new { id = movimentacao.MovimentacaoEstoqueId });
            }
            catch (ValidationException error)
            {
                ModelState.AddModelError("", error.Message);
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível registrar. Recarregue e confira o histórico antes de tentar novamente.");
            }
            catch (SqlException)
            {
                ModelState.AddModelError("", "Não foi possível concluir. Recarregue e confira o histórico antes de tentar novamente.");
            }
        }
        await LoadOptionsAsync(model);
        return View(model);
    }

    public async Task<IActionResult> Transferir(int? estoqueId)
    {
        var model = new TransferenciaEstoqueForm { EstoqueOrigemId = estoqueId ?? 0 };
        await LoadTransferOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Transferir(TransferenciaEstoqueForm model)
    {
        if (ModelState.IsValid)
        {
            try
            {
                var id = await estoqueService.TransferirAsync(model);
                return RedirectToAction(nameof(Index), new { transferenciaId = id });
            }
            catch (ValidationException error) { ModelState.AddModelError("", error.Message); }
            catch (DbUpdateException) { ModelState.AddModelError("", "Não foi possível transferir. Recarregue e confira o histórico antes de tentar novamente."); }
            catch (SqlException) { ModelState.AddModelError("", "Não foi possível concluir. Recarregue e confira o histórico antes de tentar novamente."); }
        }
        await LoadTransferOptionsAsync(model);
        return View(model);
    }

    private async Task LoadTransferOptionsAsync(TransferenciaEstoqueForm model)
    {
        var stocks = await context.Estoque.AsNoTracking().ComCores().ToListAsync();
        ViewData["EstoqueOrigemId"] = new SelectList(stocks, "EstoqueId", "DescricaoCompleta", model.EstoqueOrigemId);
        var origem = stocks.FirstOrDefault(e => e.EstoqueId == model.EstoqueOrigemId);
        ViewData["EstoqueDestinoId"] = new SelectList(stocks.Where(e => e.EstoqueId != model.EstoqueOrigemId
            && (origem == null || e.ProdutoVariacaoId == origem.ProdutoVariacaoId)), "EstoqueId", "DescricaoCompleta", model.EstoqueDestinoId);
        ViewData["UsuarioId"] = new SelectList(await context.Usuario.AsNoTracking().Where(u => u.Status == "Ativo").ToListAsync(), "UsuarioId", "Nome", model.UsuarioId);
    }

    private async Task LoadOptionsAsync(MovimentacaoEstoqueForm model)
    {
        var estoques = await context.Estoque.AsNoTracking().ComCores().ToListAsync();
        ViewData["EstoqueId"] = new SelectList(estoques, "EstoqueId", "DescricaoCompleta", model.EstoqueId);
        var tipos = await context.TipoMovimentacao.AsNoTracking().Where(t => !t.Nome.StartsWith("Transferência —")).Select(t => new
        {
            t.TipoMovimentacaoId, Nome = t.Nome + (t.Entrada ? " (entrada)" : " (saída)")
        }).ToListAsync();
        ViewData["TipoMovimentacaoId"] = new SelectList(tipos, "TipoMovimentacaoId", "Nome", model.TipoMovimentacaoId);
        ViewData["UsuarioId"] = new SelectList(await context.Usuario.AsNoTracking().Where(u => u.Status == "Ativo").ToListAsync(), "UsuarioId", "Nome", model.UsuarioId);
        ViewData["OrcamentoId"] = new SelectList(await context.Orcamento.AsNoTracking().ToListAsync(), "OrcamentoId", "OrcamentoId", model.OrcamentoId);
    }
}
