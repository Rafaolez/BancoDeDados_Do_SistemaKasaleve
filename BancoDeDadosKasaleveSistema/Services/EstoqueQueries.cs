using BancoDeDadosKasaleveSistema.Models;
using Microsoft.EntityFrameworkCore;

namespace BancoDeDadosKasaleveSistema.Services;

public static class EstoqueQueries
{
    public static IQueryable<ProdutoVariacao> ComCores(this IQueryable<ProdutoVariacao> query) => query
        .Include(v => v.Produto).Include(v => v.AluminioCor).Include(v => v.CordaCor)
        .Include(v => v.FibraCor).Include(v => v.Tecido);

    public static IQueryable<Estoque> ComCores(this IQueryable<Estoque> query) => query
        .Include(e => e.ProdutoVariacao).ThenInclude(v => v!.Produto)
        .Include(e => e.ProdutoVariacao).ThenInclude(v => v!.AluminioCor)
        .Include(e => e.ProdutoVariacao).ThenInclude(v => v!.CordaCor)
        .Include(e => e.ProdutoVariacao).ThenInclude(v => v!.FibraCor)
        .Include(e => e.ProdutoVariacao).ThenInclude(v => v!.Tecido);
}
