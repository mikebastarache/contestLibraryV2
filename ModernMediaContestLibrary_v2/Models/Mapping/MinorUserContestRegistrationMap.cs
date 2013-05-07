using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class MinorUserContestRegistrationMap : EntityTypeConfiguration<MinorUserContestRegistration>
    {
        public MinorUserContestRegistrationMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            
            // Table & Column Mappings
            ToTable("MinorUserContestRegistrations");
            Property(t => t.Id).HasColumnName("id");
            Property(t => t.MinorId).HasColumnName("minorId");
            Property(t => t.ContestId).HasColumnName("contestId");
            Property(t => t.DateCreated).HasColumnName("dateCreated");

            // Relationships
            HasRequired(t => t.Contest)
                .WithMany(t => t.MinorUserContestRegistrations)
                .HasForeignKey(d => d.ContestId);
            HasRequired(t => t.MinorUser)
                .WithMany(t => t.MinorUserContestRegistrations)
                .HasForeignKey(d => d.MinorId);
        }
    }
}
