
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class FibraCorController : Controller
{
    private readonly Contexto _context;

    public FibraCorController(Contexto context)
    {
        _context = context;
    }

    // GET: FIBRACORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.FibraCor.ToListAsync());
    }

    // GET: FIBRACORS/Details/5
    public async Task<IActionResult> Details(int? fibracorid)
    {
        if (fibracorid == null)
        {
            return NotFound();
        }

        var fibracor = await _context.FibraCor
            .FirstOrDefaultAsync(m => m.FibraCorId == fibracorid);
        if (fibracor == null)
        {
            return NotFound();
        }

        return View(fibracor);
    }

    // GET: FIBRACORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: FIBRACORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FibraCorId,Nome,Sku,HexCor,Variacoes")] FibraCor fibracor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(fibracor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(fibracor);
    }

    // GET: FIBRACORS/Edit/5
    public async Task<IActionResult> Edit(int? fibracorid)
    {
        if (fibracorid == null)
        {
            return NotFound();
        }

        var fibracor = await _context.FibraCor.FindAsync(fibracorid);
        if (fibracor == null)
        {
            return NotFound();
        }
        return View(fibracor);
    }

    // POST: FIBRACORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? fibracorid, [Bind("FibraCorId,Nome,Sku,HexCor,Variacoes")] FibraCor fibracor)
    {
        if (fibracorid != fibracor.FibraCorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(fibracor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FibraCorExists(fibracor.FibraCorId))
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
        return View(fibracor);
    }

    // GET: FIBRACORS/Delete/5
    public async Task<IActionResult> Delete(int? fibracorid)
    {
        if (fibracorid == null)
        {
            return NotFound();
        }

        var fibracor = await _context.FibraCor
            .FirstOrDefaultAsync(m => m.FibraCorId == fibracorid);
        if (fibracor == null)
        {
            return NotFound();
        }

        return View(fibracor);
    }

    // POST: FIBRACORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? fibracorid)
    {
        var fibracor = await _context.FibraCor.FindAsync(fibracorid);
        if (fibracor != null)
        {
            _context.FibraCor.Remove(fibracor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool FibraCorExists(int? fibracorid)
    {
        return _context.FibraCor.Any(e => e.FibraCorId == fibracorid);
    }
}
