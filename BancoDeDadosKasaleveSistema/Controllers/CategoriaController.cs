
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class CategoriaController : Controller
{
    private readonly Contexto _context;

    public CategoriaController(Contexto context)
    {
        _context = context;
    }

    // GET: CATEGORIAS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Categoria.ToListAsync());
    }

    // GET: CATEGORIAS/Details/5
    public async Task<IActionResult> Details(int? categoriaid)
    {
        if (categoriaid == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.CategoriaId == categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // GET: CATEGORIAS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CATEGORIAS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CategoriaId,CategoriaNome,CategoriaDescricao,Ativo")] Categoria categoria)
    {
        if (ModelState.IsValid)
        {
            _context.Add(categoria);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(categoria);
    }

    // GET: CATEGORIAS/Edit/5
    public async Task<IActionResult> Edit(int? categoriaid)
    {
        if (categoriaid == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria.FindAsync(categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }
        return View(categoria);
    }

    // POST: CATEGORIAS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? categoriaid, [Bind("CategoriaId,CategoriaNome,CategoriaDescricao,Ativo")] Categoria categoria)
    {
        if (categoriaid != categoria.CategoriaId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(categoria);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoriaExists(categoria.CategoriaId))
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
        return View(categoria);
    }

    // GET: CATEGORIAS/Delete/5
    public async Task<IActionResult> Delete(int? categoriaid)
    {
        if (categoriaid == null)
        {
            return NotFound();
        }

        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(m => m.CategoriaId == categoriaid);
        if (categoria == null)
        {
            return NotFound();
        }

        return View(categoria);
    }

    // POST: CATEGORIAS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? categoriaid)
    {
        var categoria = await _context.Categoria.FindAsync(categoriaid);
        if (categoria != null)
        {
            _context.Categoria.Remove(categoria);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CategoriaExists(int? categoriaid)
    {
        return _context.Categoria.Any(e => e.CategoriaId == categoriaid);
    }
}
