using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class ClienteController : Controller
{
    private readonly Contexto _context;
    public ClienteController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Cliente.AsNoTracking().Include(x => x.Usuario).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Cliente.AsNoTracking().Include(x => x.Usuario).FirstOrDefaultAsync(x => x.ClienteId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Cliente();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Nome,CpfCnpj,Telefone,Endereco,Cidade,Estado,Cep,Status,UsuarioId")] Cliente model)
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
        var model = await _context.Cliente.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ClienteId,Nome,CpfCnpj,Telefone,Endereco,Cidade,Estado,Cep,Status,UsuarioId")] Cliente model)
    {
        if (id != model.ClienteId) return NotFound();
        var saved = await _context.Cliente.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.Nome = model.Nome;
            saved.CpfCnpj = model.CpfCnpj;
            saved.Telefone = model.Telefone;
            saved.Endereco = model.Endereco;
            saved.Cidade = model.Cidade;
            saved.Estado = model.Estado;
            saved.Cep = model.Cep;
            saved.Status = model.Status;
            saved.UsuarioId = model.UsuarioId;
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
        var model = await _context.Cliente.AsNoTracking().Include(x => x.Usuario).FirstOrDefaultAsync(x => x.ClienteId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Cliente.FindAsync(id);
        if (model == null) return NotFound();
        _context.Cliente.Remove(model);
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

    private async Task LoadOptionsAsync(Cliente model)
    {
        ViewData["UsuarioId"] = new SelectList(await _context.Usuario.AsNoTracking().ToListAsync(), "UsuarioId", "Nome", model.UsuarioId);
    }

    private async Task ValidateReferencesAsync(Cliente model)
    {
        if (model.UsuarioId.HasValue && !await _context.Usuario.AnyAsync(x => x.UsuarioId == model.UsuarioId))
            ModelState.AddModelError("UsuarioId", "Selecione um registro válido.");
    }
}
