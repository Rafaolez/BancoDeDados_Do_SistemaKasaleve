
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class EstoqueController : Controller
{
    private readonly Contexto _context;

    public EstoqueController(Contexto context)
    {
        _context = context;
    }

    // GET: ESTOQUES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Estoque.ToListAsync());
    }

    // GET: ESTOQUES/Details/5
    public async Task<IActionResult> Details(int? estoqueid)
    {
        if (estoqueid == null)
        {
            return NotFound();
        }

        var estoque = await _context.Estoque
            .FirstOrDefaultAsync(m => m.EstoqueId == estoqueid);
        if (estoque == null)
        {
            return NotFound();
        }

        return View(estoque);
    }

    // GET: ESTOQUES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ESTOQUES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("EstoqueId,ProdutoVariacaoId,ProdutoVariacao,Localizacao,Quantidade,DataModificacao,EstoqueMinimo,Movimentacoes")] Estoque estoque)
    {
        if (ModelState.IsValid)
        {
            _context.Add(estoque);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(estoque);
    }

    // GET: ESTOQUES/Edit/5
    public async Task<IActionResult> Edit(int? estoqueid)
    {
        if (estoqueid == null)
        {
            return NotFound();
        }

        var estoque = await _context.Estoque.FindAsync(estoqueid);
        if (estoque == null)
        {
            return NotFound();
        }
        return View(estoque);
    }

    // POST: ESTOQUES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? estoqueid, [Bind("EstoqueId,ProdutoVariacaoId,ProdutoVariacao,Localizacao,Quantidade,DataModificacao,EstoqueMinimo,Movimentacoes")] Estoque estoque)
    {
        if (estoqueid != estoque.EstoqueId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(estoque);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EstoqueExists(estoque.EstoqueId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(estoque);
    }

    // GET: ESTOQUES/Delete/5
    public async Task<IActionResult> Delete(int? estoqueid)
    {
        if (estoqueid == null)
        {
            return NotFound();
        }

        var estoque = await _context.Estoque
            .FirstOrDefaultAsync(m => m.EstoqueId == estoqueid);
        if (estoque == null)
        {
            return NotFound();
        }

        return View(estoque);
    }

    // POST: ESTOQUES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? estoqueid)
    {
        var estoque = await _context.Estoque.FindAsync(estoqueid);
        if (estoque != null)
        {
            _context.Estoque.Remove(estoque);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool EstoqueExists(int? estoqueid)
    {
        return _context.Estoque.Any(e => e.EstoqueId == estoqueid);
    }
}
