
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class TipoMovimentacaosController : Controller
{
    private readonly Contexto _context;

    public TipoMovimentacaosController(Contexto context)
    {
        _context = context;
    }

    // GET: TIPOMOVIMENTACAOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.TipoMovimentacao.ToListAsync());
    }

    // GET: TIPOMOVIMENTACAOS/Details/5
    public async Task<IActionResult> Details(int? tipomovimentacaoid)
    {
        if (tipomovimentacaoid == null)
        {
            return NotFound();
        }

        var tipomovimentacao = await _context.TipoMovimentacao
            .FirstOrDefaultAsync(m => m.TipoMovimentacaoId == tipomovimentacaoid);
        if (tipomovimentacao == null)
        {
            return NotFound();
        }

        return View(tipomovimentacao);
    }

    // GET: TIPOMOVIMENTACAOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TIPOMOVIMENTACAOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TipoMovimentacaoId,Nome,Descricao,Entrada,Movimentacoes")] TipoMovimentacao tipomovimentacao)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tipomovimentacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tipomovimentacao);
    }

    // GET: TIPOMOVIMENTACAOS/Edit/5
    public async Task<IActionResult> Edit(int? tipomovimentacaoid)
    {
        if (tipomovimentacaoid == null)
        {
            return NotFound();
        }

        var tipomovimentacao = await _context.TipoMovimentacao.FindAsync(tipomovimentacaoid);
        if (tipomovimentacao == null)
        {
            return NotFound();
        }
        return View(tipomovimentacao);
    }

    // POST: TIPOMOVIMENTACAOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? tipomovimentacaoid, [Bind("TipoMovimentacaoId,Nome,Descricao,Entrada,Movimentacoes")] TipoMovimentacao tipomovimentacao)
    {
        if (tipomovimentacaoid != tipomovimentacao.TipoMovimentacaoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tipomovimentacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMovimentacaoExists(tipomovimentacao.TipoMovimentacaoId))
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
        return View(tipomovimentacao);
    }

    // GET: TIPOMOVIMENTACAOS/Delete/5
    public async Task<IActionResult> Delete(int? tipomovimentacaoid)
    {
        if (tipomovimentacaoid == null)
        {
            return NotFound();
        }

        var tipomovimentacao = await _context.TipoMovimentacao
            .FirstOrDefaultAsync(m => m.TipoMovimentacaoId == tipomovimentacaoid);
        if (tipomovimentacao == null)
        {
            return NotFound();
        }

        return View(tipomovimentacao);
    }

    // POST: TIPOMOVIMENTACAOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? tipomovimentacaoid)
    {
        var tipomovimentacao = await _context.TipoMovimentacao.FindAsync(tipomovimentacaoid);
        if (tipomovimentacao != null)
        {
            _context.TipoMovimentacao.Remove(tipomovimentacao);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TipoMovimentacaoExists(int? tipomovimentacaoid)
    {
        return _context.TipoMovimentacao.Any(e => e.TipoMovimentacaoId == tipomovimentacaoid);
    }
}
