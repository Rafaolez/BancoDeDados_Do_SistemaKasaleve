using Microsoft.AspNetCore.Mvc;
using BancoDeDadosKasaleveSistema.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class EstoqueController : Controller
{
    private readonly Contexto _context;
    public EstoqueController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Estoque.AsNoTracking().ComCores().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Estoque.AsNoTracking().ComCores().FirstOrDefaultAsync(x => x.EstoqueId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Estoque();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProdutoVariacaoId,Localizacao,EstoqueMinimo")] Estoque model)
    {
        model.Localizacao = model.Localizacao?.Trim();
        await ValidateReferencesAsync(model);
        if (await _context.Estoque.AnyAsync(x => x.ProdutoVariacaoId == model.ProdutoVariacaoId && x.Localizacao == model.Localizacao))
            ModelState.AddModelError("ProdutoVariacaoId", "Esta variação já possui estoque neste local.");
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

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Estoque.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("EstoqueId,EstoqueMinimo")] Estoque model)
    {
        if (id != model.EstoqueId) return NotFound();
        var saved = await _context.Estoque.FindAsync(id);
        if (saved == null) return NotFound();
        model.ProdutoVariacaoId = saved.ProdutoVariacaoId;
        model.Localizacao = saved.Localizacao;
        ModelState.Remove(nameof(Estoque.Localizacao));
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.EstoqueMinimo = model.EstoqueMinimo;
            saved.DataModificacao = DateTime.Now;
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

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Estoque.AsNoTracking().ComCores().FirstOrDefaultAsync(x => x.EstoqueId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Estoque.FindAsync(id);
        if (model == null) return NotFound();
        if (model.Quantidade != 0 || await _context.MovimentacaoEstoque.AnyAsync(m => m.EstoqueId == id))
        {
            ModelState.AddModelError("", "Um estoque com saldo ou movimentações não pode ser excluído.");
            return View("Delete", model);
        }
        _context.Estoque.Remove(model);
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

    private async Task LoadOptionsAsync(Estoque model)
    {
        ViewData["ProdutoVariacaoId"] = new SelectList(await _context.ProdutoVariacao.AsNoTracking().ComCores().Where(v => v.Ativo).ToListAsync(), "ProdutoVariacaoId", "DescricaoCompleta", model.ProdutoVariacaoId);
    }

    private async Task ValidateReferencesAsync(Estoque model)
    {
        if (model.EstoqueMinimo < 0) ModelState.AddModelError("EstoqueMinimo", "O estoque mínimo não pode ser negativo.");
        if (!await _context.ProdutoVariacao.AnyAsync(x => x.ProdutoVariacaoId == model.ProdutoVariacaoId))
            ModelState.AddModelError("ProdutoVariacaoId", "Selecione um registro válido.");
    }
}
