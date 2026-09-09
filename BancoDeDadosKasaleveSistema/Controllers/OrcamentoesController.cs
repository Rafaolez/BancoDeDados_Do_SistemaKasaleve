
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class OrcamentoesController : Controller
{
    private readonly Contexto _context;

    public OrcamentoesController(Contexto context)
    {
        _context = context;
    }

    // GET: ORCAMENTOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Orcamento.ToListAsync());
    }

    // GET: ORCAMENTOS/Details/5
    public async Task<IActionResult> Details(int? orcamentoid)
    {
        if (orcamentoid == null)
        {
            return NotFound();
        }

        var orcamento = await _context.Orcamento
            .FirstOrDefaultAsync(m => m.OrcamentoId == orcamentoid);
        if (orcamento == null)
        {
            return NotFound();
        }

        return View(orcamento);
    }

    // GET: ORCAMENTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ORCAMENTOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("OrcamentoId,DataCriacao,Validade,ClienteId,Cliente,UsuarioId,Usuario,SubTotal,Desconto,Frete,Total,Obs,Status,DataAlteracao,Itens,Checklists")] Orcamento orcamento)
    {
        if (ModelState.IsValid)
        {
            _context.Add(orcamento);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(orcamento);
    }

    // GET: ORCAMENTOS/Edit/5
    public async Task<IActionResult> Edit(int? orcamentoid)
    {
        if (orcamentoid == null)
        {
            return NotFound();
        }

        var orcamento = await _context.Orcamento.FindAsync(orcamentoid);
        if (orcamento == null)
        {
            return NotFound();
        }
        return View(orcamento);
    }

    // POST: ORCAMENTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? orcamentoid, [Bind("OrcamentoId,DataCriacao,Validade,ClienteId,Cliente,UsuarioId,Usuario,SubTotal,Desconto,Frete,Total,Obs,Status,DataAlteracao,Itens,Checklists")] Orcamento orcamento)
    {
        if (orcamentoid != orcamento.OrcamentoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(orcamento);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrcamentoExists(orcamento.OrcamentoId))
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
        return View(orcamento);
    }

    // GET: ORCAMENTOS/Delete/5
    public async Task<IActionResult> Delete(int? orcamentoid)
    {
        if (orcamentoid == null)
        {
            return NotFound();
        }

        var orcamento = await _context.Orcamento
            .FirstOrDefaultAsync(m => m.OrcamentoId == orcamentoid);
        if (orcamento == null)
        {
            return NotFound();
        }

        return View(orcamento);
    }

    // POST: ORCAMENTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? orcamentoid)
    {
        var orcamento = await _context.Orcamento.FindAsync(orcamentoid);
        if (orcamento != null)
        {
            _context.Orcamento.Remove(orcamento);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool OrcamentoExists(int? orcamentoid)
    {
        return _context.Orcamento.Any(e => e.OrcamentoId == orcamentoid);
    }
}
