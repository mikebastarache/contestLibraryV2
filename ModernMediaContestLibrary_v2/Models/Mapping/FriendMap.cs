using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class FriendMap : EntityTypeConfiguration<Friend>
    {
        public FriendMap()
        {
            // Primary Key
            HasKey(t => t.FriendId);

            // Properties
            Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(150);

            // Table & Column Mappings
            ToTable("Friends");
            Property(t => t.FriendId).HasColumnName("FriendID");
            Property(t => t.FirstName).HasColumnName("FriendFirstName");
            Property(t => t.LastName).HasColumnName("FriendLastName");
            Property(t => t.Email).HasColumnName("FriendEmail");
            Property(t => t.ReferrerUserId).HasColumnName("FriendReferrerUserID");
            Property(t => t.ContestId).HasColumnName("ContestID");
            Property(t => t.DateCreated).HasColumnName("FriendDateCreated");
            Property(t => t.InvitationIdentifier).HasColumnName("InvitationIdentifier");

            // Relationships
            HasOptional(t => t.Contest)
                .WithMany(t => t.Friends)
                .HasForeignKey(d => d.ContestId);
            HasOptional(t => t.User)
                .WithMany(t => t.Friends)
                .HasForeignKey(d => d.ReferrerUserId);

        }
    }
}
