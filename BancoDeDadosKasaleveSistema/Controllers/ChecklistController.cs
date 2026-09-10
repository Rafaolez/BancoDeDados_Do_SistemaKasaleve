using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class ChecklistController : Controller
{
    private readonly Contexto _context;
    public ChecklistController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Checklist.AsNoTracking().Include(x => x.Orcamento).Include(x => x.Usuario).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Checklist.AsNoTracking().Include(x => x.Orcamento).Include(x => x.Usuario).FirstOrDefaultAsync(x => x.ChecklistId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Checklist();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrcamentoId,ChecklistEnderecoEntrega,Bairro,Cidade,Cep,ConferirProduto,Cores,MesaComFuro,Alteracao,Imprimir,Nota,Prazo,Frete,PrevisaoEntrega,UsuarioId,DataConclusao,Status")] Checklist model)
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
        var model = await _context.Checklist.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ChecklistId,OrcamentoId,ChecklistEnderecoEntrega,Bairro,Cidade,Cep,ConferirProduto,Cores,MesaComFuro,Alteracao,Imprimir,Nota,Prazo,Frete,PrevisaoEntrega,UsuarioId,DataConclusao,Status")] Checklist model)
    {
        if (id != model.ChecklistId) return NotFound();
        var saved = await _context.Checklist.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.OrcamentoId = model.OrcamentoId;
            saved.ChecklistEnderecoEntrega = model.ChecklistEnderecoEntrega;
            saved.Bairro = model.Bairro;
            saved.Cidade = model.Cidade;
            saved.Cep = model.Cep;
            saved.ConferirProduto = model.ConferirProduto;
            saved.Cores = model.Cores;
            saved.MesaComFuro = model.MesaComFuro;
            saved.Alteracao = model.Alteracao;
            saved.Imprimir = model.Imprimir;
            saved.Nota = model.Nota;
            saved.Prazo = model.Prazo;
            saved.Frete = model.Frete;
            saved.PrevisaoEntrega = model.PrevisaoEntrega;
            saved.UsuarioId = model.UsuarioId;
            saved.DataConclusao = model.DataConclusao;
            saved.Status = model.Status;
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
        var model = await _context.Checklist.AsNoTracking().Include(x => x.Orcamento).Include(x => x.Usuario).FirstOrDefaultAsync(x => x.ChecklistId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Checklist.FindAsync(id);
        if (model == null) return NotFound();
        _context.Checklist.Remove(model);
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

    private async Task LoadOptionsAsync(Checklist model)
    {
        ViewData["OrcamentoId"] = new SelectList(await _context.Orcamento.AsNoTracking().ToListAsync(), "OrcamentoId", "OrcamentoId", model.OrcamentoId);
        ViewData["UsuarioId"] = new SelectList(await _context.Usuario.AsNoTracking().ToListAsync(), "UsuarioId", "Nome", model.UsuarioId);
    }

    private async Task ValidateReferencesAsync(Checklist model)
    {
        if (!model.OrcamentoId.HasValue || !await _context.Orcamento.AnyAsync(x => x.OrcamentoId == model.OrcamentoId))
            ModelState.AddModelError("OrcamentoId", "Selecione um registro válido.");
        if (!await _context.Usuario.AnyAsync(x => x.UsuarioId == model.UsuarioId))
            ModelState.AddModelError("UsuarioId", "Selecione um registro válido.");
    }
}
