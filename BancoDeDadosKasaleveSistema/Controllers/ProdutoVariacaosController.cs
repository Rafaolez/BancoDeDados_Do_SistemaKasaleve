
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class ProdutoVariacaosController : Controller
{
    private readonly Contexto _context;

    public ProdutoVariacaosController(Contexto context)
    {
        _context = context;
    }

    // GET: PRODUTOVARIACAOS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.ProdutoVariacao.ToListAsync());
    }

    // GET: PRODUTOVARIACAOS/Details/5
    public async Task<IActionResult> Details(int? produtovariacaoid)
    {
        if (produtovariacaoid == null)
        {
            return NotFound();
        }

        var produtovariacao = await _context.ProdutoVariacao
            .FirstOrDefaultAsync(m => m.ProdutoVariacaoId == produtovariacaoid);
        if (produtovariacao == null)
        {
            return NotFound();
        }

        return View(produtovariacao);
    }

    // GET: PRODUTOVARIACAOS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PRODUTOVARIACAOS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ProdutoVariacaoId,ProdutoId,Produto,AluminioCorId,AluminioCor,CordaCorId,CordaCor,FibraCorId,FibraCor,TecidoId,Tecido,Sku,Ativo,Estoque,OrcamentoItens")] ProdutoVariacao produtovariacao)
    {
        if (ModelState.IsValid)
        {
            _context.Add(produtovariacao);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(produtovariacao);
    }

    // GET: PRODUTOVARIACAOS/Edit/5
    public async Task<IActionResult> Edit(int? produtovariacaoid)
    {
        if (produtovariacaoid == null)
        {
            return NotFound();
        }

        var produtovariacao = await _context.ProdutoVariacao.FindAsync(produtovariacaoid);
        if (produtovariacao == null)
        {
            return NotFound();
        }
        return View(produtovariacao);
    }

    // POST: PRODUTOVARIACAOS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? produtovariacaoid, [Bind("ProdutoVariacaoId,ProdutoId,Produto,AluminioCorId,AluminioCor,CordaCorId,CordaCor,FibraCorId,FibraCor,TecidoId,Tecido,Sku,Ativo,Estoque,OrcamentoItens")] ProdutoVariacao produtovariacao)
    {
        if (produtovariacaoid != produtovariacao.ProdutoVariacaoId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(produtovariacao);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoVariacaoExists(produtovariacao.ProdutoVariacaoId))
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
        return View(produtovariacao);
    }

    // GET: PRODUTOVARIACAOS/Delete/5
    public async Task<IActionResult> Delete(int? produtovariacaoid)
    {
        if (produtovariacaoid == null)
        {
            return NotFound();
        }

        var produtovariacao = await _context.ProdutoVariacao
            .FirstOrDefaultAsync(m => m.ProdutoVariacaoId == produtovariacaoid);
        if (produtovariacao == null)
        {
            return NotFound();
        }

        return View(produtovariacao);
    }

    // POST: PRODUTOVARIACAOS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? produtovariacaoid)
    {
        var produtovariacao = await _context.ProdutoVariacao.FindAsync(produtovariacaoid);
        if (produtovariacao != null)
        {
            _context.ProdutoVariacao.Remove(produtovariacao);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ProdutoVariacaoExists(int? produtovariacaoid)
    {
        return _context.ProdutoVariacao.Any(e => e.ProdutoVariacaoId == produtovariacaoid);
    }
}
