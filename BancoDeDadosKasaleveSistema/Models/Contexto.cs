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

    }
}
