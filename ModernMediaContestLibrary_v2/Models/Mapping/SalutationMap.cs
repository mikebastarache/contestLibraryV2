using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class SalutationMap : EntityTypeConfiguration<Salutation>
    {
        public SalutationMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.SalutationEn)
                .IsRequired()
                .HasMaxLength(20);

            Property(t => t.SalutationFr)
                .IsRequired()
                .HasMaxLength(20);

            // Table & Column Mappings
            ToTable("Salutations");
            Property(t => t.Id).HasColumnName("id");
            Property(t => t.SalutationEn).HasColumnName("salutation_en");
            Property(t => t.SalutationFr).HasColumnName("salutation_fr");
        }
    }
}
