using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class OrcamentoItemController : Controller
{
    private readonly Contexto _context;
    public OrcamentoItemController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.OrcamentoItem.AsNoTracking().Include(x => x.Orcamento).Include(x => x.ProdutoVariacao).ToListAsync());

    public async Task<IActionResult> Details(string? id)
    {
        if (id == null) return NotFound();
        var model = await _context.OrcamentoItem.AsNoTracking().Include(x => x.Orcamento).Include(x => x.ProdutoVariacao).FirstOrDefaultAsync(x => x.OrcamentoItemId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new OrcamentoItem();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrcamentoId,ProdutoVariacaoId,Quantidade,ValorUnitario,ValorExtra,Desconto,ValorTotal,Obs,NomeProdutoSnapshot")] OrcamentoItem model)
    {
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            _context.Add(model);
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível salvar. Confira os dados e os registros relacionados.");
            }
        }
        await LoadOptionsAsync(model);
        return View(model);
    }

    public async Task<IActionResult> Edit(string? id)
    {
        if (id == null) return NotFound();
        var model = await _context.OrcamentoItem.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(string id, [Bind("OrcamentoItemId,OrcamentoId,ProdutoVariacaoId,Quantidade,ValorUnitario,ValorExtra,Desconto,ValorTotal,Obs,NomeProdutoSnapshot")] OrcamentoItem model)
    {
        if (id != model.OrcamentoItemId) return NotFound();
        var saved = await _context.OrcamentoItem.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.OrcamentoId = model.OrcamentoId;
            saved.ProdutoVariacaoId = model.ProdutoVariacaoId;
            saved.Quantidade = model.Quantidade;
            saved.ValorUnitario = model.ValorUnitario;
            saved.ValorExtra = model.ValorExtra;
            saved.Desconto = model.Desconto;
            saved.ValorTotal = model.ValorTotal;
            saved.Obs = model.Obs;
            saved.NomeProdutoSnapshot = model.NomeProdutoSnapshot;
            try
            {
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "O registro foi alterado ou excluído. Recarregue e tente novamente.");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível salvar. Confira os dados e os registros relacionados.");
            }
        }
        await LoadOptionsAsync(model);
        return View(model);
    }

    public async Task<IActionResult> Delete(string? id)
    {
        if (id == null) return NotFound();
        var model = await _context.OrcamentoItem.AsNoTracking().Include(x => x.Orcamento).Include(x => x.ProdutoVariacao).FirstOrDefaultAsync(x => x.OrcamentoItemId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(string id)
    {
        var model = await _context.OrcamentoItem.FindAsync(id);
        if (model == null) return NotFound();
        _context.OrcamentoItem.Remove(model);
        try
        {
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            ModelState.AddModelError("", "Este registro possui vínculos e não pode ser excluído.");
            return View("Delete", model);
        }
    }

    private async Task LoadOptionsAsync(OrcamentoItem model)
    {
        ViewData["OrcamentoId"] = new SelectList(await _context.Orcamento.AsNoTracking().ToListAsync(), "OrcamentoId", "OrcamentoId", model.OrcamentoId);
        ViewData["ProdutoVariacaoId"] = new SelectList(await _context.ProdutoVariacao.AsNoTracking().ToListAsync(), "ProdutoVariacaoId", "Sku", model.ProdutoVariacaoId);
    }

    private async Task ValidateReferencesAsync(OrcamentoItem model)
    {
        if (!model.OrcamentoId.HasValue || !await _context.Orcamento.AnyAsync(x => x.OrcamentoId == model.OrcamentoId))
            ModelState.AddModelError("OrcamentoId", "Selecione um registro válido.");
        if (!await _context.ProdutoVariacao.AnyAsync(x => x.ProdutoVariacaoId == model.ProdutoVariacaoId))
            ModelState.AddModelError("ProdutoVariacaoId", "Selecione um registro válido.");
    }
}
