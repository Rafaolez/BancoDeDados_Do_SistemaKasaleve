
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

public class ChecklistController : Controller
{
    private readonly Contexto _context;

    public ChecklistController(Contexto context)
    {
        _context = context;
    }

    // GET: CHECKLISTS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Checklist.ToListAsync());
    }

    // GET: CHECKLISTS/Details/5
    public async Task<IActionResult> Details(int? checklistid)
    {
        if (checklistid == null)
        {
            return NotFound();
        }

        var checklist = await _context.Checklist
            .FirstOrDefaultAsync(m => m.ChecklistId == checklistid);
        if (checklist == null)
        {
            return NotFound();
        }

        return View(checklist);
    }

    // GET: CHECKLISTS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: CHECKLISTS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("ChecklistId,Orcamento,ChecklistEnderecoEntrega,Bairro,Cidade,Cep,ConferirProduto,Cores,MesaComFuro,Alteracao,Imprimir,Nota,Prazo,Frete,PrevisaoEntrega,UsuarioId,Usuario,DataConclusao,Status")] Checklist checklist)
    {
        if (ModelState.IsValid)
        {
            _context.Add(checklist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(checklist);
    }

    // GET: CHECKLISTS/Edit/5
    public async Task<IActionResult> Edit(int? checklistid)
    {
        if (checklistid == null)
        {
            return NotFound();
        }

        var checklist = await _context.Checklist.FindAsync(checklistid);
        if (checklist == null)
        {
            return NotFound();
        }
        return View(checklist);
    }

    // POST: CHECKLISTS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? checklistid, [Bind("ChecklistId,Orcamento,ChecklistEnderecoEntrega,Bairro,Cidade,Cep,ConferirProduto,Cores,MesaComFuro,Alteracao,Imprimir,Nota,Prazo,Frete,PrevisaoEntrega,UsuarioId,Usuario,DataConclusao,Status")] Checklist checklist)
    {
        if (checklistid != checklist.ChecklistId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(checklist);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChecklistExists(checklist.ChecklistId))
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
        return View(checklist);
    }

    // GET: CHECKLISTS/Delete/5
    public async Task<IActionResult> Delete(int? checklistid)
    {
        if (checklistid == null)
        {
            return NotFound();
        }

        var checklist = await _context.Checklist
            .FirstOrDefaultAsync(m => m.ChecklistId == checklistid);
        if (checklist == null)
        {
            return NotFound();
        }

        return View(checklist);
    }

    // POST: CHECKLISTS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? checklistid)
    {
        var checklist = await _context.Checklist.FindAsync(checklistid);
        if (checklist != null)
        {
            _context.Checklist.Remove(checklist);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ChecklistExists(int? checklistid)
    {
        return _context.Checklist.Any(e => e.ChecklistId == checklistid);
    }
}
