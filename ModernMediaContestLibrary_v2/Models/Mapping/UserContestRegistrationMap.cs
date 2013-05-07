using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class UserContestRegistrationMap : EntityTypeConfiguration<UserContestRegistration>
    {
        public UserContestRegistrationMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            
            // Table & Column Mappings
            ToTable("UserContestRegistrations");
            Property(t => t.Id).HasColumnName("id");
            Property(t => t.UserId).HasColumnName("userId");
            Property(t => t.ContestId).HasColumnName("contestId");
            Property(t => t.DateCreated).HasColumnName("dateCreated");


            // Relationships
            HasRequired(t => t.Contest)
                .WithMany(t => t.UserContestRegistrations)
                .HasForeignKey(d => d.ContestId);
            HasRequired(t => t.User)
                .WithMany(t => t.UserContestRegistrations)
                .HasForeignKey(d => d.UserId);
        }
    }
}
