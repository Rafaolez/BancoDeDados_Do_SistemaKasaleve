using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class CargoController : Controller
{
    private readonly Contexto _context;
    public CargoController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Cargo.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Cargo.AsNoTracking().FirstOrDefaultAsync(x => x.CargoId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Cargo();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CargoNome,CargoDescricao,Ativo")] Cargo model)
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
        var model = await _context.Cargo.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CargoId,CargoNome,CargoDescricao,Ativo")] Cargo model)
    {
        if (id != model.CargoId) return NotFound();
        var saved = await _context.Cargo.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.CargoNome = model.CargoNome;
            saved.CargoDescricao = model.CargoDescricao;
            saved.Ativo = model.Ativo;
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
        var model = await _context.Cargo.AsNoTracking().FirstOrDefaultAsync(x => x.CargoId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Cargo.FindAsync(id);
        if (model == null) return NotFound();
        _context.Cargo.Remove(model);
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

    private async Task LoadOptionsAsync(Cargo model)
    {
        await Task.CompletedTask;
    }

    private async Task ValidateReferencesAsync(Cargo model)
    {
        await Task.CompletedTask;
    }
}
