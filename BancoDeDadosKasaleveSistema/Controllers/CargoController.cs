
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class CargoController : Controller
{
    private readonly Contexto _context;

    public CargoController(Contexto context)
    {
        _context = context;
    }

    // GET: CARGOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Cargo.ToListAsync());
    }

    // GET: CARGOS/Details/5
    public async Task<IActionResult> Details(int? cargoid)
    {
        if (cargoid == null)
        {
            return NotFound();
        }

        var cargo = await _context.Cargo
            .FirstOrDefaultAsync(m => m.CargoId == cargoid);
        if (cargo == null)
        {
            return NotFound();
        }

        return View(cargo);
    }

    // GET: CARGOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CARGOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CargoId,CargoNome,CargoDescricao,Ativo")] Cargo cargo)
    {
        if (ModelState.IsValid)
        {
            _context.Add(cargo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(cargo);
    }

    // GET: CARGOS/Edit/5
    public async Task<IActionResult> Edit(int? cargoid)
    {
        if (cargoid == null)
        {
            return NotFound();
        }

        var cargo = await _context.Cargo.FindAsync(cargoid);
        if (cargo == null)
        {
            return NotFound();
        }
        return View(cargo);
    }

    // POST: CARGOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? cargoid, [Bind("CargoId,CargoNome,CargoDescricao,Ativo")] Cargo cargo)
    {
        if (cargoid != cargo.CargoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(cargo);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CargoExists(cargo.CargoId))
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
        return View(cargo);
    }

    // GET: CARGOS/Delete/5
    public async Task<IActionResult> Delete(int? cargoid)
    {
        if (cargoid == null)
        {
            return NotFound();
        }

        var cargo = await _context.Cargo
            .FirstOrDefaultAsync(m => m.CargoId == cargoid);
        if (cargo == null)
        {
            return NotFound();
        }

        return View(cargo);
    }

    // POST: CARGOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? cargoid)
    {
        var cargo = await _context.Cargo.FindAsync(cargoid);
        if (cargo != null)
        {
            _context.Cargo.Remove(cargo);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CargoExists(int? cargoid)
    {
        return _context.Cargo.Any(e => e.CargoId == cargoid);
    }
}
