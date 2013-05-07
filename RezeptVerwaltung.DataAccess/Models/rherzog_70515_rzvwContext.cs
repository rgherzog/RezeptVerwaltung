using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using RezeptVerwaltung.DataAccess.Models.Mapping;

namespace RezeptVerwaltung.DataAccess.Models
{
    public class rherzog_70515_rzvwContext : DbContext
    {
        static rherzog_70515_rzvwContext()
        {
            Database.SetInitializer<rherzog_70515_rzvwContext>(null);
        }

		public rherzog_70515_rzvwContext()
			: base("Name=rherzog_70515_rzvwContext")
		{
		}

        public DbSet<Einheit> Einheits { get; set; }
        public DbSet<Kategorie> Kategories { get; set; }
        public DbSet<Rezept> Rezepts { get; set; }
        public DbSet<Rezeptabteilung> Rezeptabteilungs { get; set; }
        public DbSet<RezeptZutat> RezeptZutats { get; set; }
        public DbSet<Zutat> Zutats { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new EinheitMap());
            modelBuilder.Configurations.Add(new KategorieMap());
            modelBuilder.Configurations.Add(new RezeptMap());
            modelBuilder.Configurations.Add(new RezeptabteilungMap());
            modelBuilder.Configurations.Add(new RezeptZutatMap());
            modelBuilder.Configurations.Add(new ZutatMap());
        }
    }
}
