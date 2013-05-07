using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class VoteMap : EntityTypeConfiguration<Vote>
    {
        public VoteMap()
        {
            // Primary Key
            HasKey(t => t.VoteId);

            // Properties
            Property(t => t.VoteValue)
                .HasMaxLength(50);

            Property(t => t.IpAddress)
                .HasMaxLength(15);

            // Table & Column Mappings
            ToTable("Votes");
            Property(t => t.VoteId).HasColumnName("VoteID");
            Property(t => t.ContestId).HasColumnName("contestID");
            Property(t => t.BallotId).HasColumnName("ballotID");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
            Property(t => t.VoteValue).HasColumnName("Vote");
            Property(t => t.IpAddress).HasColumnName("IPaddress");
            Property(t => t.UserId).HasColumnName("userID");

            // Relationships
            HasRequired(t => t.Ballot)
                .WithMany(t => t.Votes)
                .HasForeignKey(d => d.BallotId);
            HasRequired(t => t.Contest)
                .WithMany(t => t.Votes)
                .HasForeignKey(d => d.ContestId);
            HasOptional(t => t.User)
                .WithMany(t => t.Votes)
                .HasForeignKey(d => d.UserId);

        }
    }
}
