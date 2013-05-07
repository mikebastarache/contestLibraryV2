using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class BallotMap : EntityTypeConfiguration<Ballot>
    {
        public BallotMap()
        {
            // Primary Key
            HasKey(t => t.BallotId);

            Property(t => t.Source)
                .HasMaxLength(50);

            Property(t => t.IpAddress)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Ballots");
            Property(t => t.BallotId).HasColumnName("BallotID");
            Property(t => t.UserId).HasColumnName("UserID");
            Property(t => t.ContestId).HasColumnName("ContestID");
            Property(t => t.ReferrerUserId).HasColumnName("BallotReferrerUserID");
            Property(t => t.DateCreated).HasColumnName("BallotDateCreated");
            Property(t => t.IsBonusBallot).HasColumnName("isBonusBallot");
            Property(t => t.IsShareBonusBallot).HasColumnName("isShareBonusBallot");
            Property(t => t.Source).HasColumnName("BallotSource");
            Property(t => t.IpAddress).HasColumnName("IPaddress");

            // Relationships
            HasOptional(t => t.Contest)
                .WithMany(t => t.Ballots)
                .HasForeignKey(d => d.ContestId);
            HasOptional(t => t.User)
                .WithMany(t => t.Ballots)
                .HasForeignKey(d => d.UserId);

        }
    }
}
