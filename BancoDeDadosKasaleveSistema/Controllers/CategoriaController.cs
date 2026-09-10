using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class CategoriaController : Controller
{
    private readonly Contexto _context;
    public CategoriaController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Categoria.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Categoria.AsNoTracking().FirstOrDefaultAsync(x => x.CategoriaId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Categoria();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CategoriaNome,CategoriaDescricao,Ativo")] Categoria model)
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
        var model = await _context.Categoria.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CategoriaId,CategoriaNome,CategoriaDescricao,Ativo")] Categoria model)
    {
        if (id != model.CategoriaId) return NotFound();
        var saved = await _context.Categoria.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.CategoriaNome = model.CategoriaNome;
            saved.CategoriaDescricao = model.CategoriaDescricao;
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
        var model = await _context.Categoria.AsNoTracking().FirstOrDefaultAsync(x => x.CategoriaId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Categoria.FindAsync(id);
        if (model == null) return NotFound();
        _context.Categoria.Remove(model);
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

    private async Task LoadOptionsAsync(Categoria model)
    {
        await Task.CompletedTask;
    }

    private async Task ValidateReferencesAsync(Categoria model)
    {
        await Task.CompletedTask;
    }
}
