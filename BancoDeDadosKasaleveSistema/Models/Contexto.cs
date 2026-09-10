using Microsoft.EntityFrameworkCore;

namespace BancoDeDadosKasaleveSistema.Models
{
    public class Contexto : DbContext
    {
        public Contexto(DbContextOptions<Contexto> options)
            : base(options)
        {
        }

        public DbSet<AluminioCor> AluminioCor { get; set; } = default!;
        public DbSet<Cargo> Cargo { get; set; } = default!;
        public DbSet<Categoria> Categoria { get; set; } = default!;
        public DbSet<Checklist> Checklist { get; set; } = default!;
        public DbSet<Cliente> Cliente { get; set; } = default!;
        public DbSet<CordaCor> CordaCor { get; set; } = default!;
        public DbSet<Estoque> Estoque { get; set; } = default!;
        public DbSet<FibraCor> FibraCor { get; set; } = default!;
        public DbSet<Historico> Historico { get; set; } = default!;
        public DbSet<MovimentacaoEstoque> MovimentacaoEstoque { get; set; } = default!;
        public DbSet<Orcamento> Orcamento { get; set; } = default!;
        public DbSet<OrcamentoItem> OrcamentoItem { get; set; } = default!;
        public DbSet<Produto> Produto { get; set; } = default!;
        public DbSet<ProdutoVariacao> ProdutoVariacao { get; set; } = default!;
        public DbSet<Tecido> Tecido { get; set; } = default!;
        public DbSet<TipoMovimentacao> TipoMovimentacao { get; set; } = default!;
        public DbSet<Usuario> Usuario { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Estoque>()
                .HasOne(e => e.ProdutoVariacao).WithMany(v => v.Estoques)
                .HasForeignKey(e => e.ProdutoVariacaoId).OnDelete(DeleteBehavior.NoAction);
            modelBuilder.Entity<Estoque>()
                .HasIndex(e => new { e.ProdutoVariacaoId, e.Localizacao }).IsUnique();
            modelBuilder.Entity<MovimentacaoEstoque>().HasIndex(m => m.TransferenciaId);

            modelBuilder.Entity<Checklist>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Checklists)
                .HasForeignKey(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            foreach (var entity in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entity.GetProperties().Where(p => p.ClrType == typeof(decimal)))
                {
                    property.SetPrecision(18);
                    property.SetScale(2);
                }
            }

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasIndex(m => new { m.EstoqueId, m.DataMovimentacao });

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Estoque)
                .WithMany(e => e.Movimentacoes)
                .HasForeignKey(m => m.EstoqueId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Usuario)
                .WithMany(u => u.Movimentacoes)
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.TipoMovimentacao)
                .WithMany(t => t.Movimentacoes)
                .HasForeignKey(m => m.TipoMovimentacaoId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<MovimentacaoEstoque>()
                .HasOne(m => m.Orcamento)
                .WithMany()
                .HasForeignKey(m => m.OrcamentoId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }
}
