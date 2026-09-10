using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;
using Microsoft.AspNetCore.Identity;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class UsuarioController : Controller
{
    private readonly Contexto _context;
    public UsuarioController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Usuario.AsNoTracking().Include(x => x.Cargo).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Usuario.AsNoTracking().Include(x => x.Cargo).FirstOrDefaultAsync(x => x.UsuarioId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Usuario();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,Email,Telefone,Senha,CargoId,Status")] Usuario model)
    {
        await ValidateReferencesAsync(model);
        if (string.IsNullOrWhiteSpace(model.Senha)) ModelState.AddModelError("Senha", "Informe a senha.");
        if (ModelState.IsValid)
        {
            model.SenhaHash = new PasswordHasher<Usuario>().HashPassword(model, model.Senha!);
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
        var model = await _context.Usuario.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("UsuarioId,Nome,Email,Telefone,Senha,CargoId,Status")] Usuario model)
    {
        if (id != model.UsuarioId) return NotFound();
        var saved = await _context.Usuario.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.Nome = model.Nome;
            saved.Email = model.Email;
            saved.Telefone = model.Telefone;
            saved.CargoId = model.CargoId;
            saved.Status = model.Status;
            if (!string.IsNullOrWhiteSpace(model.Senha))
                saved.SenhaHash = new PasswordHasher<Usuario>().HashPassword(saved, model.Senha);
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
        var model = await _context.Usuario.AsNoTracking().Include(x => x.Cargo).FirstOrDefaultAsync(x => x.UsuarioId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Usuario.FindAsync(id);
        if (model == null) return NotFound();
        _context.Usuario.Remove(model);
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

    private async Task LoadOptionsAsync(Usuario model)
    {
        ViewData["CargoId"] = new SelectList(await _context.Cargo.AsNoTracking().ToListAsync(), "CargoId", "CargoNome", model.CargoId);
    }

    private async Task ValidateReferencesAsync(Usuario model)
    {
        if (!await _context.Cargo.AnyAsync(x => x.CargoId == model.CargoId))
            ModelState.AddModelError("CargoId", "Selecione um registro válido.");
    }
}
