using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class MinorVoteMap : EntityTypeConfiguration<MinorVote>
    {
        public MinorVoteMap()
        {
            // Primary Key
            HasKey(t => t.MinorVoteId);

            // Properties
            Property(t => t.MinorVoteValue)
                .HasMaxLength(50);

            Property(t => t.IpAddress)
                .HasMaxLength(15);

            // Table & Column Mappings
            ToTable("MinorVotes");
            Property(t => t.MinorVoteId).HasColumnName("minorVoteId");
            Property(t => t.ContestId).HasColumnName("contestId");
            Property(t => t.MinorId).HasColumnName("minorId");
            Property(t => t.MinorVoteValue).HasColumnName("minorVote");
            Property(t => t.IpAddress).HasColumnName("IPaddress");
            Property(t => t.DateCreated).HasColumnName("dateCreated");

            // Relationships
            //HasOptional(t => t.MinorUser)
            //    .WithMany(t => t.MinorVotes)
            //    .HasForeignKey(d => d.MinorId);
            HasRequired(t => t.Contest)
                .WithMany(t => t.MinorVotes)
                .HasForeignKey(d => d.ContestId);

        }
    }
}
