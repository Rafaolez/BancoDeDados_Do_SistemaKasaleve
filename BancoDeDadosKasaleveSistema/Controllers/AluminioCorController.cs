
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class AluminioCorController : Controller
{
    private readonly Contexto _context;

    public AluminioCorController(Contexto context)
    {
        _context = context;
    }

    // GET: ALUMINIOCORS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.AluminioCor.ToListAsync());
    }

    // GET: ALUMINIOCORS/Details/5
    public async Task<IActionResult> Details(int? aluminiocorid)
    {
        if (aluminiocorid == null)
        {
            return NotFound();
        }

        var aluminiocor = await _context.AluminioCor
            .FirstOrDefaultAsync(m => m.AluminioCorId == aluminiocorid);
        if (aluminiocor == null)
        {
            return NotFound();
        }

        return View(aluminiocor);
    }

    // GET: ALUMINIOCORS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: ALUMINIOCORS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("AluminioCorId,CorAluminioCor,Codigo,SKUAluminioCor,HexCor")] AluminioCor aluminiocor)
    {
        if (ModelState.IsValid)
        {
            _context.Add(aluminiocor);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(aluminiocor);
    }

    // GET: ALUMINIOCORS/Edit/5
    public async Task<IActionResult> Edit(int? aluminiocorid)
    {
        if (aluminiocorid == null)
        {
            return NotFound();
        }

        var aluminiocor = await _context.AluminioCor.FindAsync(aluminiocorid);
        if (aluminiocor == null)
        {
            return NotFound();
        }
        return View(aluminiocor);
    }

    // POST: ALUMINIOCORS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? aluminiocorid, [Bind("AluminioCorId,CorAluminioCor,Codigo,SKUAluminioCor,HexCor")] AluminioCor aluminiocor)
    {
        if (aluminiocorid != aluminiocor.AluminioCorId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(aluminiocor);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AluminioCorExists(aluminiocor.AluminioCorId))
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
        return View(aluminiocor);
    }

    // GET: ALUMINIOCORS/Delete/5
    public async Task<IActionResult> Delete(int? aluminiocorid)
    {
        if (aluminiocorid == null)
        {
            return NotFound();
        }

        var aluminiocor = await _context.AluminioCor
            .FirstOrDefaultAsync(m => m.AluminioCorId == aluminiocorid);
        if (aluminiocor == null)
        {
            return NotFound();
        }

        return View(aluminiocor);
    }

    // POST: ALUMINIOCORS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? aluminiocorid)
    {
        var aluminiocor = await _context.AluminioCor.FindAsync(aluminiocorid);
        if (aluminiocor != null)
        {
            _context.AluminioCor.Remove(aluminiocor);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool AluminioCorExists(int? aluminiocorid)
    {
        return _context.AluminioCor.Any(e => e.AluminioCorId == aluminiocorid);
    }
}
