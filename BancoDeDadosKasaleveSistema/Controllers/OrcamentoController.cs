using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class OrcamentoController : Controller
{
    private readonly Contexto _context;
    public OrcamentoController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Orcamento.AsNoTracking().Include(x => x.Cliente).Include(x => x.Usuario).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Orcamento.AsNoTracking().Include(x => x.Cliente).Include(x => x.Usuario).FirstOrDefaultAsync(x => x.OrcamentoId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Orcamento();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Validade,ClienteId,UsuarioId,SubTotal,Desconto,Frete,Total,Obs,Status")] Orcamento model)
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
        var model = await _context.Orcamento.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("OrcamentoId,Validade,ClienteId,UsuarioId,SubTotal,Desconto,Frete,Total,Obs,Status")] Orcamento model)
    {
        if (id != model.OrcamentoId) return NotFound();
        var saved = await _context.Orcamento.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.Validade = model.Validade;
            saved.ClienteId = model.ClienteId;
            saved.UsuarioId = model.UsuarioId;
            saved.SubTotal = model.SubTotal;
            saved.Desconto = model.Desconto;
            saved.Frete = model.Frete;
            saved.Total = model.Total;
            saved.Obs = model.Obs;
            saved.Status = model.Status;
            saved.DataAlteracao = DateTime.Now;
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
        var model = await _context.Orcamento.AsNoTracking().Include(x => x.Cliente).Include(x => x.Usuario).FirstOrDefaultAsync(x => x.OrcamentoId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Orcamento.FindAsync(id);
        if (model == null) return NotFound();
        _context.Orcamento.Remove(model);
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

    private async Task LoadOptionsAsync(Orcamento model)
    {
        ViewData["ClienteId"] = new SelectList(await _context.Cliente.AsNoTracking().ToListAsync(), "ClienteId", "Nome", model.ClienteId);
        ViewData["UsuarioId"] = new SelectList(await _context.Usuario.AsNoTracking().ToListAsync(), "UsuarioId", "Nome", model.UsuarioId);
    }

    private async Task ValidateReferencesAsync(Orcamento model)
    {
        if (!await _context.Cliente.AnyAsync(x => x.ClienteId == model.ClienteId))
            ModelState.AddModelError("ClienteId", "Selecione um registro válido.");
        if (!await _context.Usuario.AnyAsync(x => x.UsuarioId == model.UsuarioId))
            ModelState.AddModelError("UsuarioId", "Selecione um registro válido.");
    }
}
