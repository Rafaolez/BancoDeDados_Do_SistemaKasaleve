
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class ProdutoesController : Controller
{
    private readonly Contexto _context;

    public ProdutoesController(Contexto context)
    {
        _context = context;
    }

    // GET: PRODUTOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Produto.ToListAsync());
    }

    // GET: PRODUTOS/Details/5
    public async Task<IActionResult> Details(int? produtoid)
    {
        if (produtoid == null)
        {
            return NotFound();
        }

        var produto = await _context.Produto
            .FirstOrDefaultAsync(m => m.ProdutoId == produtoid);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    // GET: PRODUTOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUTOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProdutoId,CategoriaId,Categoria,Nome,Descricao,ValorLogista,ValorFinal,Img,DataCadastro,Sku,EstoqueMinimo,Ativo,Variacoes")] Produto produto)
    {
        if (ModelState.IsValid)
        {
            _context.Add(produto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produto);
    }

    // GET: PRODUTOS/Edit/5
    public async Task<IActionResult> Edit(int? produtoid)
    {
        if (produtoid == null)
        {
            return NotFound();
        }

        var produto = await _context.Produto.FindAsync(produtoid);
        if (produto == null)
        {
            return NotFound();
        }
        return View(produto);
    }

    // POST: PRODUTOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? produtoid, [Bind("ProdutoId,CategoriaId,Categoria,Nome,Descricao,ValorLogista,ValorFinal,Img,DataCadastro,Sku,EstoqueMinimo,Ativo,Variacoes")] Produto produto)
    {
        if (produtoid != produto.ProdutoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(produto);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(produto.ProdutoId))
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
        return View(produto);
    }

    // GET: PRODUTOS/Delete/5
    public async Task<IActionResult> Delete(int? produtoid)
    {
        if (produtoid == null)
        {
            return NotFound();
        }

        var produto = await _context.Produto
            .FirstOrDefaultAsync(m => m.ProdutoId == produtoid);
        if (produto == null)
        {
            return NotFound();
        }

        return View(produto);
    }

    // POST: PRODUTOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? produtoid)
    {
        var produto = await _context.Produto.FindAsync(produtoid);
        if (produto != null)
        {
            _context.Produto.Remove(produto);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoExists(int? produtoid)
    {
        return _context.Produto.Any(e => e.ProdutoId == produtoid);
    }
}
