using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using BancoDeDadosKasaleveSistema.Models;
using BancoDeDadosKasaleveSistema.Services;
using BancoDeDadosKasaleveSistema.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

var database = "Kasaleve_Checks_" + Guid.NewGuid().ToString("N");
var connection = $"Server=(localdb)\\mssqllocaldb;Database={database};Trusted_Connection=True";
var options = new DbContextOptionsBuilder<Contexto>().UseSqlServer(connection).Options;
await using var context = new Contexto(options);
Process? server = null;
var output = new List<string>();
void Check(bool condition, string message)
{
    if (!condition) throw new Exception(message);
    Console.WriteLine("OK: " + message);
}
try
{
    await context.GetService<IMigrator>().MigrateAsync("20260910122854_Criacao-Inicial");
    var cargo = new Cargo { CargoNome = "Operador", CargoDescricao = "Testes" };
    var usuario = new Usuario { Nome = "Responsável", Email = "teste@example.com", Cargo = cargo, SenhaHash = "hash-de-teste" };
    var produto = new Produto { Nome = "Produto de teste", ValorFinal = 10m };
    var variacao = new ProdutoVariacao { Produto = produto, Sku = "TESTE" };
    var estoque = new Estoque { ProdutoVariacao = variacao, Localizacao = "Depósito" };
    var entrada = new TipoMovimentacao { Nome = "Recebimento", Entrada = true };
    var saida = new TipoMovimentacao { Nome = "Saída", Entrada = false };
    context.AddRange(usuario, estoque, entrada, saida);
    await context.SaveChangesAsync();
    await context.Database.MigrateAsync();
    Check(await context.Estoque.AsNoTracking().AnyAsync(e => e.EstoqueId == estoque.EstoqueId && e.Localizacao == "Depósito"), "Migration preserva estoque existente");
    var estoqueId = estoque.EstoqueId;
    var usuarioId = usuario.UsuarioId;
    var entradaId = entrada.TipoMovimentacaoId;
    var saidaId = saida.TipoMovimentacaoId;
    MovimentacaoEstoqueForm Form(int type, int quantity) => new()
    {
        EstoqueId = estoqueId, UsuarioId = usuarioId, TipoMovimentacaoId = type,
        Quantidade = quantity, Motivo = "Teste"
    };
    async Task<MovimentacaoEstoque> Register(MovimentacaoEstoqueForm form)
    {
        await using var db = new Contexto(options);
        return await new EstoqueService(db).RegistrarAsync(form);
    }
    var first = await Register(Form(entradaId, 10));
    var second = await Register(Form(saidaId, 3));
    Check(first.SaldoAnterior == 0 && first.SaldoPosterior == 10 && second.SaldoAnterior == 10 && second.SaldoPosterior == 7, "Entrada e saída calculam saldos e registram histórico");
    foreach (var invalid in new[] { Form(saidaId, 8), Form(entradaId, 0), Form(entradaId, -1), Form(entradaId, int.MaxValue), Form(-1, 1) })
    {
        try { await Register(invalid); throw new Exception("Movimentação inválida aceita"); }
        catch (ValidationException) { }
    }
    var badOrder = Form(entradaId, 1); badOrder.OrcamentoId = int.MaxValue;
    try { await Register(badOrder); throw new Exception("Orçamento inexistente aceito"); }
    catch (ValidationException) { }
    Check(await context.Estoque.AsNoTracking().Where(e => e.EstoqueId == estoqueId).Select(e => e.Quantidade).SingleAsync() == 7 && await context.MovimentacaoEstoque.CountAsync() == 2, "Rejeições preservam saldo e histórico");

    var listener = new TcpListener(IPAddress.Loopback, 0);
    listener.Start(); var port = ((IPEndPoint)listener.LocalEndpoint).Port; listener.Stop();
    var repo = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../../"));
    var app = Path.Combine(repo, "BancoDeDadosKasaleveSistema");
    var start = new ProcessStartInfo("dotnet") { WorkingDirectory = app, UseShellExecute = false, CreateNoWindow = true, RedirectStandardOutput = true, RedirectStandardError = true };
    start.ArgumentList.Add(Path.Combine(app, "bin/Debug/net10.0/BancoDeDadosKasaleveSistema.dll"));
    start.Environment["ASPNETCORE_URLS"] = $"http://127.0.0.1:{port}";
    start.Environment["ASPNETCORE_ENVIRONMENT"] = "Development";
    start.Environment["ConnectionStrings__Contexto"] = connection;
    server = Process.Start(start)!;
    server.OutputDataReceived += (_, e) => { if(e.Data != null) lock(output) output.Add(e.Data); };
    server.ErrorDataReceived += (_, e) => { if(e.Data != null) lock(output) output.Add(e.Data); };
    server.BeginOutputReadLine(); server.BeginErrorReadLine();
    using var client = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false, CookieContainer = new CookieContainer() }) { BaseAddress = new Uri($"http://127.0.0.1:{port}") };
    for (int i=0;i<100;i++)
    {
        try { using var ready=await client.GetAsync("/"); break; }
        catch(HttpRequestException) { if(server.HasExited || i==99) throw; await Task.Delay(100); }
    }
    foreach(var controller in new[] { "AluminioCor", "Cargo", "Categoria", "Checklist", "Cliente", "CordaCor", "Estoque", "FibraCor", "Historico", "MovimentacaoEstoque", "Orcamento", "OrcamentoItem", "Produto", "ProdutoVariacao", "Tecido", "TipoMovimentacao", "Usuario" })
    {
        foreach(var action in controller=="Historico" ? new[] { "Index" } : new[] { "Index", "Create" })
        {
            using var response = await client.GetAsync($"/{controller}/{action}");
            Check(response.StatusCode==HttpStatusCode.OK, $"Tela {controller}/{action} renderiza");
        }
    }
    foreach(var route in new[] { $"/Estoque/Edit/{estoqueId}", $"/Estoque/Details/{estoqueId}", $"/Estoque/Delete/{estoqueId}", $"/Usuario/Edit/{usuarioId}", $"/MovimentacaoEstoque/Details/{first.MovimentacaoEstoqueId}" })
    {
        using var response=await client.GetAsync(route);
        Check(response.StatusCode==HttpStatusCode.OK, "Rota com id: " + route);
    }
    var usersHtml = await client.GetStringAsync("/Usuario");
    Check(!usersHtml.Contains("hash-de-teste") && !usersHtml.Contains("SenhaHash"), "Listagem não expõe hash da senha");
    async Task<HttpResponseMessage> Post(string route, Dictionary<string,string> fields, string? getRoute=null)
    {
        var html = await client.GetStringAsync(getRoute ?? route);
        var token = Regex.Match(html, "name=\"__RequestVerificationToken\"[^>]*value=\"([^\"]+)\"").Groups[1].Value;
        if(string.IsNullOrEmpty(token)) throw new Exception("Token antiforgery ausente");
        fields["__RequestVerificationToken"] = WebUtility.HtmlDecode(token);
        return await client.PostAsync(route, new FormUrlEncodedContent(fields));
    }
    using(var response=await Post($"/Estoque/Edit/{estoqueId}", new() { ["EstoqueId"]=estoqueId.ToString(), ["Localizacao"]="Loja", ["EstoqueMinimo"]="2", ["Quantidade"]="999", ["ProdutoVariacaoId"]="999" }))
        Check(response.StatusCode==HttpStatusCode.Redirect, "Edição de estoque aceita campos permitidos");
    Check(await context.Estoque.AsNoTracking().Where(e=>e.EstoqueId==estoqueId).Select(e=>e.Quantidade).SingleAsync()==7, "POST adulterado não altera saldo diretamente");
    Check(await context.Estoque.AsNoTracking().Where(e=>e.EstoqueId==estoqueId).Select(e=>e.Localizacao).SingleAsync()=="Depósito", "Local não muda por edição de cadastro");
    using(var response=await Post("/MovimentacaoEstoque/Create", new() { ["EstoqueId"]=estoqueId.ToString(), ["TipoMovimentacaoId"]=entradaId.ToString(), ["UsuarioId"]=usuarioId.ToString(), ["Quantidade"]="2", ["Motivo"]="Entrada HTTP", ["SaldoAnterior"]="999", ["SaldoPosterior"]="999" }))
        Check(response.StatusCode==HttpStatusCode.Redirect, "Movimentação via formulário salva");
    var latest=await context.MovimentacaoEstoque.AsNoTracking().OrderByDescending(m=>m.MovimentacaoEstoqueId).FirstAsync();
    Check(latest.SaldoAnterior==7 && latest.SaldoPosterior==9, "Servidor ignora saldos enviados pelo formulário");
    foreach(var route in new[] { "/MovimentacaoEstoque/Edit/1", "/MovimentacaoEstoque/Delete/1", "/Historico/Create", "/Historico/Edit/1", "/Historico/Delete/1" })
    {
        using var response=await client.GetAsync(route);
        Check(response.StatusCode==HttpStatusCode.NotFound, "Histórico sem endpoint de alteração: " + route);
    }
    using(var response=await Post("/Usuario/Create", new() { ["Nome"]="Novo", ["Email"]="novo@example.com", ["CargoId"]=cargo.CargoId.ToString(), ["Status"]="Ativo", ["Senha"]="SenhaTeste123!", ["SenhaHash"]="forjado" }))
        Check(response.StatusCode==HttpStatusCode.Redirect, "Cadastro gera senha no backend");
    var newUser=await context.Usuario.AsNoTracking().SingleAsync(u=>u.Email=="novo@example.com");
    Check(new PasswordHasher<Usuario>().VerifyHashedPassword(newUser,newUser.SenhaHash,"SenhaTeste123!")!=PasswordVerificationResult.Failed && newUser.SenhaHash!="forjado", "Hash válido e campo adulterado ignorado");
    using(var response=await Post($"/Usuario/Edit/{newUser.UsuarioId}", new() { ["UsuarioId"]=newUser.UsuarioId.ToString(), ["Nome"]="Atualizado", ["Email"]=newUser.Email, ["CargoId"]=cargo.CargoId.ToString(), ["Status"]="Ativo", ["Senha"]="", ["SenhaHash"]="forjado" }))
        Check(response.StatusCode==HttpStatusCode.Redirect, "Editar com senha vazia funciona");
    Check(await context.Usuario.AsNoTracking().Where(u=>u.UsuarioId==newUser.UsuarioId).Select(u=>u.SenhaHash).SingleAsync()==newUser.SenhaHash, "Senha anterior preservada");

    async Task<bool> ConcurrentExit()
    {
        try { await Register(Form(saidaId,6)); return true; }
        catch(ValidationException) { return false; }
        catch(SqlException error) when(error.Number==1205) { return false; }
        catch(DbUpdateException error) when(error.InnerException is SqlException { Number:1205 }) { return false; }
    }
    var results=await Task.WhenAll(ConcurrentExit(),ConcurrentExit());
    Check(results.Count(x=>x)==1 && await context.Estoque.AsNoTracking().Where(e=>e.EstoqueId==estoqueId).Select(e=>e.Quantidade).SingleAsync()==3, "Saídas simultâneas não consomem o mesmo saldo");

    var destino = new Estoque { ProdutoVariacaoId = variacao.ProdutoVariacaoId, Localizacao = "Loja" };
    context.Estoque.Add(destino);
    await context.SaveChangesAsync();
    await using(var db = new Contexto(options))
    {
        var transferencia = await new EstoqueService(db).TransferirAsync(new()
        {
            EstoqueOrigemId = estoqueId, EstoqueDestinoId = destino.EstoqueId,
            Quantidade = 2, UsuarioId = usuarioId, Motivo = "Reposição da loja"
        });
        var registros = await db.MovimentacaoEstoque.AsNoTracking().Where(m => m.TransferenciaId == transferencia).ToListAsync();
        Check(registros.Count == 2 && registros.Any(m => m.SaldoAnterior == 3 && m.SaldoPosterior == 1)
            && registros.Any(m => m.SaldoAnterior == 0 && m.SaldoPosterior == 2), "Transferência grava saída e entrada vinculadas");
        Check(registros.All(m => !string.IsNullOrEmpty(m.DescricaoVariacao)) && registros.Select(m=>m.LocalizacaoRegistro).Distinct().Count()==2, "Histórico preserva produto, cores e locais");
    }
    Check(await context.Estoque.AsNoTracking().Where(e=>e.ProdutoVariacaoId==variacao.ProdutoVariacaoId).SumAsync(e=>e.Quantidade)==3, "Transferência preserva saldo total");
    var outra = new ProdutoVariacao { ProdutoId = produto.ProdutoId, Sku = "OUTRA-COR", CordaCor = new CordaCor { Nome = "Azul" } };
    var estoqueOutra = new Estoque { ProdutoVariacao = outra, Localizacao = "Loja" };
    context.Add(estoqueOutra); await context.SaveChangesAsync();
    foreach(var to in new[] {estoqueId, estoqueOutra.EstoqueId})
    {
        await using var db=new Contexto(options);
        try { await new EstoqueService(db).TransferirAsync(new() { EstoqueOrigemId=estoqueId, EstoqueDestinoId=to, Quantidade=1, UsuarioId=usuarioId }); throw new Exception("Transferência inválida aceita"); }
        catch(ValidationException) { }
    }
    Check(await context.Estoque.AsNoTracking().Where(e=>e.EstoqueId==estoqueId).Select(e=>e.Quantidade).SingleAsync()==1, "Transferência rejeita mesma origem e cores diferentes");
    using(var response=await Post("/MovimentacaoEstoque/Transferir", new()
    {
        ["EstoqueOrigemId"]=destino.EstoqueId.ToString(), ["EstoqueDestinoId"]=estoqueId.ToString(),
        ["Quantidade"]="1", ["UsuarioId"]=usuarioId.ToString(), ["Motivo"]="Retorno ao depósito"
    })) Check(response.StatusCode==HttpStatusCode.Redirect, "Transferência via formulário salva");
    Check(await context.Estoque.AsNoTracking().Where(e=>e.EstoqueId==estoqueId).Select(e=>e.Quantidade).SingleAsync()==2,
        "Formulário de transferência atualiza o destino");
    foreach(var route in new[] { "/Usuarios", "/Produtoes", "/ProdutoVariacaos", "/Orcamentoes", "/OrcamentoItems", "/Tecidoes", "/TipoMovimentacaos", "/Historicoes", "/MovimentacaoEstoques", $"/MovimentacaoEstoque/Transferir?estoqueId={estoqueId}" })
    {
        using var response=await client.GetAsync(route);
        Check(response.StatusCode==HttpStatusCode.OK, "Rota compatível: " + route);
    }
    Console.WriteLine("Todos os testes passaram.");
}
catch
{
    lock(output) Console.Error.WriteLine(string.Join(Environment.NewLine, output.TakeLast(45)));
    throw;
}
finally
{
    if(server is { HasExited:false }) { server.Kill(entireProcessTree:true); await server.WaitForExitAsync(); }
    server?.Dispose();
    await context.Database.EnsureDeletedAsync();
}
