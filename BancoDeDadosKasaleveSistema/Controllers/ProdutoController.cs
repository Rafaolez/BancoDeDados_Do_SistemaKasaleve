using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class ProdutoController : Controller
{
    private readonly Contexto _context;
    public ProdutoController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Produto.AsNoTracking().Include(x => x.Categoria).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Produto.AsNoTracking().Include(x => x.Categoria).FirstOrDefaultAsync(x => x.ProdutoId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new Produto();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CategoriaId,Nome,Descricao,ValorLogista,ValorFinal,Img,Sku,EstoqueMinimo,Ativo")] Produto model)
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
        var model = await _context.Produto.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProdutoId,CategoriaId,Nome,Descricao,ValorLogista,ValorFinal,Img,Sku,EstoqueMinimo,Ativo")] Produto model)
    {
        if (id != model.ProdutoId) return NotFound();
        var saved = await _context.Produto.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        if (ModelState.IsValid)
        {
            saved.CategoriaId = model.CategoriaId;
            saved.Nome = model.Nome;
            saved.Descricao = model.Descricao;
            saved.ValorLogista = model.ValorLogista;
            saved.ValorFinal = model.ValorFinal;
            saved.Img = model.Img;
            saved.Sku = model.Sku;
            saved.EstoqueMinimo = model.EstoqueMinimo;
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
        var model = await _context.Produto.AsNoTracking().Include(x => x.Categoria).FirstOrDefaultAsync(x => x.ProdutoId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.Produto.FindAsync(id);
        if (model == null) return NotFound();
        _context.Produto.Remove(model);
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

    private async Task LoadOptionsAsync(Produto model)
    {
        ViewData["CategoriaId"] = new SelectList(await _context.Categoria.AsNoTracking().ToListAsync(), "CategoriaId", "CategoriaNome", model.CategoriaId);
    }

    private async Task ValidateReferencesAsync(Produto model)
    {
        if (model.CategoriaId.HasValue && !await _context.Categoria.AnyAsync(x => x.CategoriaId == model.CategoriaId))
            ModelState.AddModelError("CategoriaId", "Selecione um registro válido.");
    }
}
