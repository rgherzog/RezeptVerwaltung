using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace RezeptVerwaltung.DataAccess.Models.Mapping
{
    public class EinheitMap : EntityTypeConfiguration<Einheit>
    {
        public EinheitMap()
        {
            // Primary Key
            this.HasKey(t => t.ID);

            // Properties
            this.Property(t => t.Bezeichnung)
                .IsRequired()
                .HasMaxLength(50);

            this.Property(t => t.Kurzzeichen)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            this.ToTable("Einheit");
            this.Property(t => t.ID).HasColumnName("ID");
            this.Property(t => t.Bezeichnung).HasColumnName("Bezeichnung");
            this.Property(t => t.Kurzzeichen).HasColumnName("Kurzzeichen");
        }
    }
}
