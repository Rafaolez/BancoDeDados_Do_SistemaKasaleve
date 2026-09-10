using System.ComponentModel.DataAnnotations;
using System.Data;
using BancoDeDadosKasaleveSistema.Models;
using BancoDeDadosKasaleveSistema.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BancoDeDadosKasaleveSistema.Services;

public class EstoqueService(Contexto context)
{
    public async Task<MovimentacaoEstoque> RegistrarAsync(MovimentacaoEstoqueForm form)
    {
        Validator.ValidateObject(form, new ValidationContext(form), validateAllProperties: true);

        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var estoque = await context.Estoque.FromSqlInterpolated($"SELECT * FROM Estoque WITH (UPDLOCK, HOLDLOCK) WHERE EstoqueId = {form.EstoqueId}")
            .SingleOrDefaultAsync()
            ?? throw new ValidationException("Estoque não encontrado.");
        await context.Estoque.ComCores().Where(e => e.EstoqueId == estoque.EstoqueId).LoadAsync();
        var tipo = await context.TipoMovimentacao.SingleOrDefaultAsync(t => t.TipoMovimentacaoId == form.TipoMovimentacaoId)
            ?? throw new ValidationException("Tipo de movimentação não encontrado.");
        if (tipo.Nome.StartsWith("Transferência —", StringComparison.Ordinal))
            throw new ValidationException("Use a opção Transferir para movimentar produtos entre locais.");
        if (!await context.Usuario.AnyAsync(u => u.UsuarioId == form.UsuarioId && u.Status == "Ativo"))
            throw new ValidationException("Selecione um responsável ativo.");
        if (form.OrcamentoId.HasValue && !await context.Orcamento.AnyAsync(o => o.OrcamentoId == form.OrcamentoId))
            throw new ValidationException("Orçamento não encontrado.");

        var saldo = (long)estoque.Quantidade + (tipo.Entrada ? form.Quantidade : -(long)form.Quantidade);
        if (saldo < 0) throw new ValidationException("Saldo insuficiente para esta saída.");
        if (saldo > int.MaxValue) throw new ValidationException("A quantidade excede o limite do estoque.");

        var movimentacao = new MovimentacaoEstoque
        {
            EstoqueId = estoque.EstoqueId,
            TipoMovimentacaoId = tipo.TipoMovimentacaoId,
            Quantidade = form.Quantidade,
            SaldoAnterior = estoque.Quantidade,
            SaldoPosterior = (int)saldo,
            Motivo = form.Motivo.Trim(),
            OrcamentoId = form.OrcamentoId,
            UsuarioId = form.UsuarioId,
            Obs = form.Obs,
            DataMovimentacao = DateTime.Now
        };
        movimentacao.DescricaoVariacao = estoque.ProdutoVariacao!.DescricaoCompleta;
        movimentacao.LocalizacaoRegistro = estoque.Localizacao;
        estoque.Quantidade = movimentacao.SaldoPosterior;
        estoque.DataModificacao = movimentacao.DataMovimentacao;
        context.MovimentacaoEstoque.Add(movimentacao);
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return movimentacao;
    }

    public async Task<Guid> TransferirAsync(TransferenciaEstoqueForm form)
    {
        Validator.ValidateObject(form, new ValidationContext(form), validateAllProperties: true);
        if (form.EstoqueOrigemId == form.EstoqueDestinoId)
            throw new ValidationException("Origem e destino devem ser diferentes.");
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        foreach (var estoqueId in new[] { form.EstoqueOrigemId, form.EstoqueDestinoId }.OrderBy(id => id))
            await context.Estoque.FromSqlInterpolated($"SELECT * FROM Estoque WITH (UPDLOCK, HOLDLOCK) WHERE EstoqueId = {estoqueId}").LoadAsync();
        var stocks = await context.Estoque.ComCores()
            .Where(e => e.EstoqueId == form.EstoqueOrigemId || e.EstoqueId == form.EstoqueDestinoId)
            .OrderBy(e => e.EstoqueId).ToListAsync();
        var origem = stocks.SingleOrDefault(e => e.EstoqueId == form.EstoqueOrigemId)
            ?? throw new ValidationException("Origem não encontrada.");
        var destino = stocks.SingleOrDefault(e => e.EstoqueId == form.EstoqueDestinoId)
            ?? throw new ValidationException("Destino não encontrado.");
        if (origem.ProdutoVariacaoId != destino.ProdutoVariacaoId)
            throw new ValidationException("Origem e destino devem possuir exatamente a mesma variação de produto e cores.");
        if (string.Equals(origem.Localizacao?.Trim(), destino.Localizacao?.Trim(), StringComparison.OrdinalIgnoreCase))
            throw new ValidationException("Selecione outro local de destino.");
        if (origem.Quantidade < form.Quantidade) throw new ValidationException("Saldo insuficiente na origem.");
        if ((long)destino.Quantidade + form.Quantidade > int.MaxValue) throw new ValidationException("Saldo de destino excede o limite.");
        if (!await context.Usuario.AnyAsync(u => u.UsuarioId == form.UsuarioId && u.Status == "Ativo"))
            throw new ValidationException("Selecione um responsável ativo.");

        async Task<TipoMovimentacao> Tipo(bool entrada)
        {
            var nome = entrada ? "Transferência — entrada" : "Transferência — saída";
            var tipo = await context.TipoMovimentacao.FirstOrDefaultAsync(t => t.Nome == nome && t.Entrada == entrada);
            if (tipo != null) return tipo;
            tipo = new TipoMovimentacao { Nome = nome, Entrada = entrada };
            context.TipoMovimentacao.Add(tipo);
            return tipo;
        }
        var entrada = await Tipo(true);
        var saida = await Tipo(false);
        var id = Guid.NewGuid();
        var agora = DateTime.Now;
        MovimentacaoEstoque Registro(Estoque stock, TipoMovimentacao tipo, int saldo) => new()
        {
            Estoque = stock, TipoMovimentacao = tipo, Quantidade = form.Quantidade,
            SaldoAnterior = stock.Quantidade, SaldoPosterior = saldo,
            Motivo = form.Motivo.Trim(), UsuarioId = form.UsuarioId,
            Obs = form.Obs, TransferenciaId = id, DataMovimentacao = agora,
            DescricaoVariacao = stock.ProdutoVariacao!.DescricaoCompleta,
            LocalizacaoRegistro = stock.Localizacao
        };
        var registroSaida = Registro(origem, saida, origem.Quantidade - form.Quantidade);
        var registroEntrada = Registro(destino, entrada, destino.Quantidade + form.Quantidade);
        context.MovimentacaoEstoque.AddRange(registroSaida, registroEntrada);
        origem.Quantidade = registroSaida.SaldoPosterior;
        destino.Quantidade = registroEntrada.SaldoPosterior;
        origem.DataModificacao = destino.DataModificacao = agora;
        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        return id;
    }
}
