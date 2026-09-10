using BancoDeDadosKasaleveSistema.Models;
using Microsoft.EntityFrameworkCore;
using BancoDeDadosKasaleveSistema.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<EstoqueService>();
builder.Services.AddDbContext<Contexto>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Contexto")
        ?? throw new InvalidOperationException("A conexão 'Contexto' não foi configurada.")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

foreach (var route in new Dictionary<string, string>
{
    ["Usuarios"] = "Usuario", ["Produtoes"] = "Produto", ["Produtos"] = "Produto",
    ["ProdutoVariacaos"] = "ProdutoVariacao", ["Orcamentoes"] = "Orcamento",
    ["OrcamentoItems"] = "OrcamentoItem", ["Tecidoes"] = "Tecido",
    ["TipoMovimentacaos"] = "TipoMovimentacao", ["Historicoes"] = "Historico",
    ["MovimentacaoEstoques"] = "MovimentacaoEstoque"
})
{
    app.MapControllerRoute("compat_" + route.Key, route.Key + "/{action=Index}/{id?}", new { controller = route.Value });
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
