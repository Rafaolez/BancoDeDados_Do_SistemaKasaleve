using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class FibraCorController : Controller
{
    private readonly Contexto _context;
    public FibraCorController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.FibraCor.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.FibraCor.AsNoTracking().FirstOrDefaultAsync(x => x.FibraCorId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new FibraCor();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Sku,HexCor")] FibraCor model)
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
        var model = await _context.FibraCor.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("FibraCorId,Nome,Sku,HexCor")] FibraCor model)
    {
        if (id != model.FibraCorId) return NotFound();
        var saved = await _context.FibraCor.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.Nome = model.Nome;
            saved.Sku = model.Sku;
            saved.HexCor = model.HexCor;
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
        var model = await _context.FibraCor.AsNoTracking().FirstOrDefaultAsync(x => x.FibraCorId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.FibraCor.FindAsync(id);
        if (model == null) return NotFound();
        _context.FibraCor.Remove(model);
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

    private async Task LoadOptionsAsync(FibraCor model)
    {
        await Task.CompletedTask;
    }

    private async Task ValidateReferencesAsync(FibraCor model)
    {
        await Task.CompletedTask;
    }
}
