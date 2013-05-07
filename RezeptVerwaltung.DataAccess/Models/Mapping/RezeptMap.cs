using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RezeptVerwaltung.DataAccess.Models.Mapping
{
    public class RezeptMap : EntityTypeConfiguration<Rezept>
    {
        public RezeptMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Name)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Menge)
                .IsRequired()
                .IsFixedLength()
                .HasMaxLength(10);

            this.Property(t => t.Anleitung)
                .IsRequired();

            this.Property(t => t.Kommentar)
                .HasMaxLength(1024);

            // Table & Column Mappings
            this.ToTable("Rezept");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.KategorieID).HasColumnName("KategorieID");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.Bild).HasColumnName("Bild");
            this.Property(t => t.Menge).HasColumnName("Menge");
            this.Property(t => t.EinheitID).HasColumnName("EinheitID");
            this.Property(t => t.Anleitung).HasColumnName("Anleitung");
            this.Property(t => t.Kommentar).HasColumnName("Kommentar");

            // Relationships
            this.HasRequired(t => t.Einheit)
                .WithMany(t => t.Rezepts)
                .HasForeignKey(d => d.EinheitID);
            this.HasRequired(t => t.Kategorie)
                .WithMany(t => t.Rezepts)
                .HasForeignKey(d => d.KategorieID);

        }
    }
}
