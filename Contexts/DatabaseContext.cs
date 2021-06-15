using Microsoft.EntityFrameworkCore;

using Courses.Models;

namespace Courses.Contexts
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Drzava> Drzave { get; set; }
        public virtual DbSet<Grad> Gradovi { get; set; }
        public virtual DbSet<TipObavijesti> TipoviObavijesti { get; set; }
        public virtual DbSet<Obavijest> Obavijesti { get; set; }
        public virtual DbSet<Kurs> Kursevi { get; set; }
        public virtual DbSet<Oblast> Oblasti { get; set; }
        public virtual DbSet<Korisnik> Korisnici { get; set; }
        public virtual DbSet<KursKorisnik> KursKorisnici { get; set; }
        public virtual DbSet<Predavanje> Predavanja { get; set; }
        public virtual DbSet<Prisustvo> Prisustva { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Predavanje>()
                .HasMany<Prisustvo>(x => x.Prisustva)
                .WithOne(x => x.Predavanje)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
