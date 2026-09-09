
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class TecidoesController : Controller
{
    private readonly Contexto _context;

    public TecidoesController(Contexto context)
    {
        _context = context;
    }

    // GET: TECIDOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Tecido.ToListAsync());
    }

    // GET: TECIDOS/Details/5
    public async Task<IActionResult> Details(int? tecidoid)
    {
        if (tecidoid == null)
        {
            return NotFound();
        }

        var tecido = await _context.Tecido
            .FirstOrDefaultAsync(m => m.TecidoId == tecidoid);
        if (tecido == null)
        {
            return NotFound();
        }

        return View(tecido);
    }

    // GET: TECIDOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TECIDOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("TecidoId,Nome,Sku,Variacoes")] Tecido tecido)
    {
        if (ModelState.IsValid)
        {
            _context.Add(tecido);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(tecido);
    }

    // GET: TECIDOS/Edit/5
    public async Task<IActionResult> Edit(int? tecidoid)
    {
        if (tecidoid == null)
        {
            return NotFound();
        }

        var tecido = await _context.Tecido.FindAsync(tecidoid);
        if (tecido == null)
        {
            return NotFound();
        }
        return View(tecido);
    }

    // POST: TECIDOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? tecidoid, [Bind("TecidoId,Nome,Sku,Variacoes")] Tecido tecido)
    {
        if (tecidoid != tecido.TecidoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(tecido);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TecidoExists(tecido.TecidoId))
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
        return View(tecido);
    }

    // GET: TECIDOS/Delete/5
    public async Task<IActionResult> Delete(int? tecidoid)
    {
        if (tecidoid == null)
        {
            return NotFound();
        }

        var tecido = await _context.Tecido
            .FirstOrDefaultAsync(m => m.TecidoId == tecidoid);
        if (tecido == null)
        {
            return NotFound();
        }

        return View(tecido);
    }

    // POST: TECIDOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? tecidoid)
    {
        var tecido = await _context.Tecido.FindAsync(tecidoid);
        if (tecido != null)
        {
            _context.Tecido.Remove(tecido);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TecidoExists(int? tecidoid)
    {
        return _context.Tecido.Any(e => e.TecidoId == tecidoid);
    }
}
