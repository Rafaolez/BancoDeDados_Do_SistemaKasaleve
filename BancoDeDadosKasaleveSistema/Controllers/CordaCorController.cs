
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class CordaCorController : Controller
{
    private readonly Contexto _context;

    public CordaCorController(Contexto context)
    {
        _context = context;
    }

    // GET: CORDACORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.CordaCor.ToListAsync());
    }

    // GET: CORDACORS/Details/5
    public async Task<IActionResult> Details(int? cordacorid)
    {
        if (cordacorid == null)
        {
            return NotFound();
        }

        var cordacor = await _context.CordaCor
            .FirstOrDefaultAsync(m => m.CordaCorId == cordacorid);
        if (cordacor == null)
        {
            return NotFound();
        }

        return View(cordacor);
    }

    // GET: CORDACORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CORDACORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CordaCorId,Nome,Sku,HexCor,Variacoes")] CordaCor cordacor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cordacor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cordacor);
    }

    // GET: CORDACORS/Edit/5
    public async Task<IActionResult> Edit(int? cordacorid)
    {
        if (cordacorid == null)
        {
            return NotFound();
        }

        var cordacor = await _context.CordaCor.FindAsync(cordacorid);
        if (cordacor == null)
        {
            return NotFound();
        }
        return View(cordacor);
    }

    // POST: CORDACORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? cordacorid, [Bind("CordaCorId,Nome,Sku,HexCor,Variacoes")] CordaCor cordacor)
    {
        if (cordacorid != cordacor.CordaCorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cordacor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CordaCorExists(cordacor.CordaCorId))
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
        return View(cordacor);
    }

    // GET: CORDACORS/Delete/5
    public async Task<IActionResult> Delete(int? cordacorid)
    {
        if (cordacorid == null)
        {
            return NotFound();
        }

        var cordacor = await _context.CordaCor
            .FirstOrDefaultAsync(m => m.CordaCorId == cordacorid);
        if (cordacor == null)
        {
            return NotFound();
        }

        return View(cordacor);
    }

    // POST: CORDACORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? cordacorid)
    {
        var cordacor = await _context.CordaCor.FindAsync(cordacorid);
        if (cordacor != null)
        {
            _context.CordaCor.Remove(cordacor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CordaCorExists(int? cordacorid)
    {
        return _context.CordaCor.Any(e => e.CordaCorId == cordacorid);
    }
}
