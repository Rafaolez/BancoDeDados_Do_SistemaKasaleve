using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Models;

namespace BancoDeDadosKasaleveSistema.Controllers;

public class HistoricoController : Controller
{
    private readonly Contexto _context;
    public HistoricoController(Contexto context) => _context = context;

    public async Task<IActionResult> Index() => View(await _context.Historico.AsNoTracking().Include(x => x.Usuario).ToListAsync());

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        var model = await _context.Historico.AsNoTracking().Include(x => x.Usuario).FirstOrDefaultAsync(x => x.HistoricoId == id);
        return model == null ? NotFound() : View(model);
    }
}
