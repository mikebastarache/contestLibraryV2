using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class ContestMap : EntityTypeConfiguration<Contest>
    {
        public ContestMap()
        {
            // Primary Key
            HasKey(t => t.ContestId);

            // Properties
            Property(t => t.Name)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Contests");
            Property(t => t.ContestId).HasColumnName("ContestID");
            Property(t => t.Name).HasColumnName("ContestName");
            Property(t => t.Description).HasColumnName("ContestDescription");
            Property(t => t.DateCreated).HasColumnName("ContestDateCreated");
            Property(t => t.DateStart).HasColumnName("ContestDateStart");
            Property(t => t.DateEnd).HasColumnName("ContestDateEnd");
        }
    }
}
