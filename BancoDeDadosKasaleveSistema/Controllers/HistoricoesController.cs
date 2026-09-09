
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class HistoricoesController : Controller
{
    private readonly Contexto _context;

    public HistoricoesController(Contexto context)
    {
        _context = context;
    }

    // GET: HISTORICOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Historico.ToListAsync());
    }

    // GET: HISTORICOS/Details/5
    public async Task<IActionResult> Details(int? historicoid)
    {
        if (historicoid == null)
        {
            return NotFound();
        }

        var historico = await _context.Historico
            .FirstOrDefaultAsync(m => m.HistoricoId == historicoid);
        if (historico == null)
        {
            return NotFound();
        }

        return View(historico);
    }

    // GET: HISTORICOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: HISTORICOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("HistoricoId,UsuarioId,Usuario,Entidade,EntidadeId,Acao,Descricao,DataHora")] Historico historico)
    {
        if (ModelState.IsValid)
        {
            _context.Add(historico);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(historico);
    }

    // GET: HISTORICOS/Edit/5
    public async Task<IActionResult> Edit(int? historicoid)
    {
        if (historicoid == null)
        {
            return NotFound();
        }

        var historico = await _context.Historico.FindAsync(historicoid);
        if (historico == null)
        {
            return NotFound();
        }
        return View(historico);
    }

    // POST: HISTORICOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? historicoid, [Bind("HistoricoId,UsuarioId,Usuario,Entidade,EntidadeId,Acao,Descricao,DataHora")] Historico historico)
    {
        if (historicoid != historico.HistoricoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(historico);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricoExists(historico.HistoricoId))
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
        return View(historico);
    }

    // GET: HISTORICOS/Delete/5
    public async Task<IActionResult> Delete(int? historicoid)
    {
        if (historicoid == null)
        {
            return NotFound();
        }

        var historico = await _context.Historico
            .FirstOrDefaultAsync(m => m.HistoricoId == historicoid);
        if (historico == null)
        {
            return NotFound();
        }

        return View(historico);
    }

    // POST: HISTORICOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? historicoid)
    {
        var historico = await _context.Historico.FindAsync(historicoid);
        if (historico != null)
        {
            _context.Historico.Remove(historico);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool HistoricoExists(int? historicoid)
    {
        return _context.Historico.Any(e => e.HistoricoId == historicoid);
    }
}
