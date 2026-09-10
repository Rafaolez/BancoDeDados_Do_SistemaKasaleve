using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class AluminioCorController : Controller
{
    private readonly Contexto _context;
    public AluminioCorController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.AluminioCor.AsNoTracking().ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.AluminioCor.AsNoTracking().FirstOrDefaultAsync(x => x.AluminioCorId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new AluminioCor();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CorAluminioCor,Codigo,SKUAluminioCor,HexCor")] AluminioCor model)
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
        var model = await _context.AluminioCor.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("AluminioCorId,CorAluminioCor,Codigo,SKUAluminioCor,HexCor")] AluminioCor model)
    {
        if (id != model.AluminioCorId) return NotFound();
        var saved = await _context.AluminioCor.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.CorAluminioCor = model.CorAluminioCor;
            saved.Codigo = model.Codigo;
            saved.SKUAluminioCor = model.SKUAluminioCor;
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
        var model = await _context.AluminioCor.AsNoTracking().FirstOrDefaultAsync(x => x.AluminioCorId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.AluminioCor.FindAsync(id);
        if (model == null) return NotFound();
        _context.AluminioCor.Remove(model);
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

    private async Task LoadOptionsAsync(AluminioCor model)
    {
        await Task.CompletedTask;
    }

    private async Task ValidateReferencesAsync(AluminioCor model)
    {
        await Task.CompletedTask;
    }
}
