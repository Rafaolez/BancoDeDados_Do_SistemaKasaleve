using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class TipoMovimentacaoController : Controller
{
    private readonly Contexto _context;
    public TipoMovimentacaoController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.TipoMovimentacao.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.TipoMovimentacao.AsNoTracking().FirstOrDefaultAsync(x => x.TipoMovimentacaoId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new TipoMovimentacao();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Descricao,Entrada")] TipoMovimentacao model)
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

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.TipoMovimentacao.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("TipoMovimentacaoId,Nome,Descricao,Entrada")] TipoMovimentacao model)
    {
        if (id != model.TipoMovimentacaoId) return NotFound();
        var saved = await _context.TipoMovimentacao.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if ((model.Entrada != saved.Entrada || model.Nome != saved.Nome) && await _context.MovimentacaoEstoque.AnyAsync(m => m.TipoMovimentacaoId == id))
            ModelState.AddModelError("Entrada", "Este tipo já foi utilizado. Cadastre outro tipo para mudar entre entrada e saída.");
        if (ModelState.IsValid)
        {
            saved.Nome = model.Nome;
            saved.Descricao = model.Descricao;
            saved.Entrada = model.Entrada;
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
        var model = await _context.TipoMovimentacao.AsNoTracking().FirstOrDefaultAsync(x => x.TipoMovimentacaoId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.TipoMovimentacao.FindAsync(id);
        if (model == null) return NotFound();
        _context.TipoMovimentacao.Remove(model);
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

    private async Task LoadOptionsAsync(TipoMovimentacao model)
    {
        await Task.CompletedTask;
    }

    private async Task ValidateReferencesAsync(TipoMovimentacao model)
    {
        await Task.CompletedTask;
    }
}
