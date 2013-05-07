using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            HasKey(t => t.UserId);

            // Properties
            Property(t => t.FirstName)
                .HasMaxLength(255);

            Property(t => t.LastName)
                .HasMaxLength(255);

            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(255);

            Property(t => t.Address1)
                .HasMaxLength(50);

            Property(t => t.Address2)
                .HasMaxLength(50);

            Property(t => t.City)
                .HasMaxLength(50);

            Property(t => t.Province)
                .HasMaxLength(50);

            Property(t => t.PostalCode)
                .HasMaxLength(50);

            Property(t => t.Telephone)
                .HasMaxLength(20);

            Property(t => t.Language)
                .HasMaxLength(255);

            Property(t => t.YearOfBirth)
                .HasMaxLength(50);

            Property(t => t.Fbuid)
                .HasMaxLength(50);

            Property(t => t.Password)
                .HasMaxLength(50);

            // Table & Column Mappings
            ToTable("Users");
            Property(t => t.UserId).HasColumnName("UserID");
            Property(t => t.Salutation).HasColumnName("UserSalutation");
            Property(t => t.FirstName).HasColumnName("UserFirstName");
            Property(t => t.LastName).HasColumnName("UserLastName");
            Property(t => t.Email).HasColumnName("UserEmail");
            Property(t => t.Address1).HasColumnName("UserAddress");
            Property(t => t.Address2).HasColumnName("UserAddress2");
            Property(t => t.City).HasColumnName("UserCity");
            Property(t => t.Province).HasColumnName("UserProvince");
            Property(t => t.PostalCode).HasColumnName("UserPostalCode");
            Property(t => t.Telephone).HasColumnName("UserTelephone");
            Property(t => t.DateOfBirth).HasColumnName("UserDOB");
            Property(t => t.Language).HasColumnName("UserLanguage");
            Property(t => t.OptIn).HasColumnName("UserOptIn");
            Property(t => t.YearOfBirth).HasColumnName("UserYearOfBirth");
            Property(t => t.OriginalFriendId).HasColumnName("UserOriginalFriendID");
            Property(t => t.ContestSignupId).HasColumnName("UserContestSignupID");
            Property(t => t.DateCreated).HasColumnName("UserDateCreated");
            Property(t => t.DateModified).HasColumnName("UserDateModified");
            Property(t => t.Fbuid).HasColumnName("FBUID");
            Property(t => t.Password).HasColumnName("Password");
        }
    }
}
