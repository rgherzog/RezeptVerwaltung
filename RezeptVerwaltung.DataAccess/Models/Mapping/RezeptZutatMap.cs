using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RezeptVerwaltung.DataAccess.Models.Mapping
{
    public class RezeptZutatMap : EntityTypeConfiguration<RezeptZutat>
    {
        public RezeptZutatMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            // Table & Column Mappings
            this.ToTable("RezeptZutat");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.RezeptID).HasColumnName("RezeptID");
            this.Property(t => t.ZutatID).HasColumnName("ZutatID");
            this.Property(t => t.MengeVon).HasColumnName("MengeVon");
            this.Property(t => t.EinheitID).HasColumnName("EinheitID");
            this.Property(t => t.RezeptabteilungID).HasColumnName("RezeptabteilungID");
            this.Property(t => t.MengeBis).HasColumnName("MengeBis");

            // Relationships
            this.HasOptional(t => t.Einheit)
                .WithMany(t => t.RezeptZutats)
                .HasForeignKey(d => d.EinheitID);
            this.HasRequired(t => t.Rezept)
                .WithMany(t => t.RezeptZutats)
                .HasForeignKey(d => d.RezeptID);
            this.HasOptional(t => t.Rezeptabteilung)
                .WithMany(t => t.RezeptZutats)
                .HasForeignKey(d => d.RezeptabteilungID);
            this.HasRequired(t => t.Zutat)
                .WithMany(t => t.RezeptZutats)
                .HasForeignKey(d => d.ZutatID);

        }
    }
}
