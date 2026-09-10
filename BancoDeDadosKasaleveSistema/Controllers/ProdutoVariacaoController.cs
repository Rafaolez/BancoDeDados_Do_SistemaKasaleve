using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class ProdutoVariacaoController : Controller
{
    private readonly Contexto _context;
    public ProdutoVariacaoController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.ProdutoVariacao.AsNoTracking().Include(x => x.Produto).Include(x => x.AluminioCor).Include(x => x.CordaCor).Include(x => x.FibraCor).Include(x => x.Tecido).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.ProdutoVariacao.AsNoTracking().Include(x => x.Produto).Include(x => x.AluminioCor).Include(x => x.CordaCor).Include(x => x.FibraCor).Include(x => x.Tecido).FirstOrDefaultAsync(x => x.ProdutoVariacaoId == id);
        return model == null ? NotFound() : View(model);
    }

    public async Task<IActionResult> Create()
    {
        var model = new ProdutoVariacao();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProdutoId,AluminioCorId,CordaCorId,FibraCorId,TecidoId,Sku,Ativo")] ProdutoVariacao model)
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
        var model = await _context.ProdutoVariacao.FindAsync(id);
        if (model == null) return NotFound();
        await LoadOptionsAsync(model);
        return View(model);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("ProdutoVariacaoId,ProdutoId,AluminioCorId,CordaCorId,FibraCorId,TecidoId,Sku,Ativo")] ProdutoVariacao model)
    {
        if (id != model.ProdutoVariacaoId) return NotFound();
        var saved = await _context.ProdutoVariacao.FindAsync(id);
        if (saved == null) return NotFound();
        await ValidateReferencesAsync(model);
        var utilizada = await _context.Estoque.AnyAsync(e => e.ProdutoVariacaoId == id)
            || await _context.OrcamentoItem.AnyAsync(i => i.ProdutoVariacaoId == id);
        if (utilizada && (saved.ProdutoId != model.ProdutoId || saved.AluminioCorId != model.AluminioCorId
            || saved.CordaCorId != model.CordaCorId || saved.FibraCorId != model.FibraCorId
            || saved.TecidoId != model.TecidoId || saved.Sku != model.Sku))
            ModelState.AddModelError("", "Esta variação já foi utilizada. Cadastre outra combinação para mudar produto, cores ou SKU.");
        if (ModelState.IsValid)
        {
            saved.ProdutoId = model.ProdutoId;
            saved.AluminioCorId = model.AluminioCorId;
            saved.CordaCorId = model.CordaCorId;
            saved.FibraCorId = model.FibraCorId;
            saved.TecidoId = model.TecidoId;
            saved.Sku = model.Sku;
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
        var model = await _context.ProdutoVariacao.AsNoTracking().Include(x => x.Produto).Include(x => x.AluminioCor).Include(x => x.CordaCor).Include(x => x.FibraCor).Include(x => x.Tecido).FirstOrDefaultAsync(x => x.ProdutoVariacaoId == id);
        return model == null ? NotFound() : View(model);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var model = await _context.ProdutoVariacao.FindAsync(id);
        if (model == null) return NotFound();
        _context.ProdutoVariacao.Remove(model);
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

    private async Task LoadOptionsAsync(ProdutoVariacao model)
    {
        ViewData["ProdutoId"] = new SelectList(await _context.Produto.AsNoTracking().ToListAsync(), "ProdutoId", "Nome", model.ProdutoId);
        ViewData["AluminioCorId"] = new SelectList(await _context.AluminioCor.AsNoTracking().ToListAsync(), "AluminioCorId", "CorAluminioCor", model.AluminioCorId);
        ViewData["CordaCorId"] = new SelectList(await _context.CordaCor.AsNoTracking().ToListAsync(), "CordaCorId", "Nome", model.CordaCorId);
        ViewData["FibraCorId"] = new SelectList(await _context.FibraCor.AsNoTracking().ToListAsync(), "FibraCorId", "Nome", model.FibraCorId);
        ViewData["TecidoId"] = new SelectList(await _context.Tecido.AsNoTracking().ToListAsync(), "TecidoId", "Nome", model.TecidoId);
    }

    private async Task ValidateReferencesAsync(ProdutoVariacao model)
    {
        model.Sku = (model.Sku ?? string.Empty).Trim();
        if (await _context.ProdutoVariacao.AnyAsync(v => v.ProdutoVariacaoId != model.ProdutoVariacaoId && v.Sku == model.Sku))
            ModelState.AddModelError("Sku", "Este SKU já está cadastrado.");
        if (await _context.ProdutoVariacao.AnyAsync(v => v.ProdutoVariacaoId != model.ProdutoVariacaoId
            && v.ProdutoId == model.ProdutoId && v.AluminioCorId == model.AluminioCorId
            && v.CordaCorId == model.CordaCorId && v.FibraCorId == model.FibraCorId && v.TecidoId == model.TecidoId))
            ModelState.AddModelError("", "Esta combinação já está cadastrada. Use a variação existente.");
        if (!await _context.Produto.AnyAsync(x => x.ProdutoId == model.ProdutoId))
            ModelState.AddModelError("ProdutoId", "Selecione um registro válido.");
        if (model.AluminioCorId.HasValue && !await _context.AluminioCor.AnyAsync(x => x.AluminioCorId == model.AluminioCorId))
            ModelState.AddModelError("AluminioCorId", "Selecione um registro válido.");
        if (model.CordaCorId.HasValue && !await _context.CordaCor.AnyAsync(x => x.CordaCorId == model.CordaCorId))
            ModelState.AddModelError("CordaCorId", "Selecione um registro válido.");
        if (model.FibraCorId.HasValue && !await _context.FibraCor.AnyAsync(x => x.FibraCorId == model.FibraCorId))
            ModelState.AddModelError("FibraCorId", "Selecione um registro válido.");
        if (model.TecidoId.HasValue && !await _context.Tecido.AnyAsync(x => x.TecidoId == model.TecidoId))
            ModelState.AddModelError("TecidoId", "Selecione um registro válido.");
    }
}
