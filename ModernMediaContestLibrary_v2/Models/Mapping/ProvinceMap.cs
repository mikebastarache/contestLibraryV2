using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class ProvinceMap : EntityTypeConfiguration<Province>
    {
        public ProvinceMap()
        {
            // Primary Key
            HasKey(t => t.Abbreviation);

            // Properties
            Property(t => t.NameEn)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.NameFr)
                .HasMaxLength(50);

            Property(t => t.Country)
                .IsRequired()
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("ProvincesStates");
            Property(t => t.Abbreviation).HasColumnName("abbreviation");
            Property(t => t.NameEn).HasColumnName("name_en");
            Property(t => t.NameFr).HasColumnName("name_fr");
            Property(t => t.Country).HasColumnName("country");
        }
    }
}
