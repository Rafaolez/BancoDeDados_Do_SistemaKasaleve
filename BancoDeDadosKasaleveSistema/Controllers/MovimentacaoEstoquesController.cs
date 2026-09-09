
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class MovimentacaoEstoquesController : Controller
{
    private readonly Contexto _context;

    public MovimentacaoEstoquesController(Contexto context)
    {
        _context = context;
    }

    // GET: MOVIMENTACAOESTOQUES
    public async Task<IActionResult> Index()    
    {
        return View(await _context.MovimentacaoEstoque.ToListAsync());
    }

    // GET: MOVIMENTACAOESTOQUES/Details/5
    public async Task<IActionResult> Details(int? movimentacaoestoqueid)
    {
        if (movimentacaoestoqueid == null)
        {
            return NotFound();
        }

        var movimentacaoestoque = await _context.MovimentacaoEstoque
            .FirstOrDefaultAsync(m => m.MovimentacaoEstoqueId == movimentacaoestoqueid);
        if (movimentacaoestoque == null)
        {
            return NotFound();
        }

        return View(movimentacaoestoque);
    }

    // GET: MOVIMENTACAOESTOQUES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MOVIMENTACAOESTOQUES/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("MovimentacaoEstoqueId,EstoqueId,Estoque,TipoMovimentacaoId,TipoMovimentacao,Quantidade,SaldoAnterior,SaldoPosterior,Motivo,OrcamentoId,Orcamento,DataMovimentacao,Obs,UsuarioId,Usuario")] MovimentacaoEstoque movimentacaoestoque)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movimentacaoestoque);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movimentacaoestoque);
    }

    // GET: MOVIMENTACAOESTOQUES/Edit/5
    public async Task<IActionResult> Edit(int? movimentacaoestoqueid)
    {
        if (movimentacaoestoqueid == null)
        {
            return NotFound();
        }

        var movimentacaoestoque = await _context.MovimentacaoEstoque.FindAsync(movimentacaoestoqueid);
        if (movimentacaoestoque == null)
        {
            return NotFound();
        }
        return View(movimentacaoestoque);
    }

    // POST: MOVIMENTACAOESTOQUES/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? movimentacaoestoqueid, [Bind("MovimentacaoEstoqueId,EstoqueId,Estoque,TipoMovimentacaoId,TipoMovimentacao,Quantidade,SaldoAnterior,SaldoPosterior,Motivo,OrcamentoId,Orcamento,DataMovimentacao,Obs,UsuarioId,Usuario")] MovimentacaoEstoque movimentacaoestoque)
    {
        if (movimentacaoestoqueid != movimentacaoestoque.MovimentacaoEstoqueId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movimentacaoestoque);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovimentacaoEstoqueExists(movimentacaoestoque.MovimentacaoEstoqueId))
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
        return View(movimentacaoestoque);
    }

    // GET: MOVIMENTACAOESTOQUES/Delete/5
    public async Task<IActionResult> Delete(int? movimentacaoestoqueid)
    {
        if (movimentacaoestoqueid == null)
        {
            return NotFound();
        }

        var movimentacaoestoque = await _context.MovimentacaoEstoque
            .FirstOrDefaultAsync(m => m.MovimentacaoEstoqueId == movimentacaoestoqueid);
        if (movimentacaoestoque == null)
        {
            return NotFound();
        }

        return View(movimentacaoestoque);
    }

    // POST: MOVIMENTACAOESTOQUES/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? movimentacaoestoqueid)
    {
        var movimentacaoestoque = await _context.MovimentacaoEstoque.FindAsync(movimentacaoestoqueid);
        if (movimentacaoestoque != null)
        {
            _context.MovimentacaoEstoque.Remove(movimentacaoestoque);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovimentacaoEstoqueExists(int? movimentacaoestoqueid)
    {
        return _context.MovimentacaoEstoque.Any(e => e.MovimentacaoEstoqueId == movimentacaoestoqueid);
    }
}
