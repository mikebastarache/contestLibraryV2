using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class MinorUserMap : EntityTypeConfiguration<MinorUser>
    {
        public MinorUserMap()
        {
            // Primary Key
            HasKey(t => t.MinorId);

            // Properties
            Property(t => t.MinorFirstName)
                .IsRequired()
                .HasMaxLength(50);

            Property(t => t.MinorLastName)
                .HasMaxLength(50);

            Property(t => t.MinorEmail)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.MinorDateOfBirth)
                .IsRequired();

            Property(t => t.MinorLanguage)
                .HasMaxLength(20);

            Property(t => t.MinorGuid)
                .IsRequired();

            Property(t => t.GuardianFirstName)
                .HasMaxLength(50);

            Property(t => t.GuardianLastName)
                .HasMaxLength(50);

            Property(t => t.GuardianEmail)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.GuardianAddress1)
                .HasMaxLength(255);

            Property(t => t.GuardianAddress2)
                .HasMaxLength(255);

            Property(t => t.GuardianCity)
                .HasMaxLength(50);

            Property(t => t.GuardianProvince)
                .HasMaxLength(50);

            Property(t => t.GuardianPostalCode)
                .HasMaxLength(50);

            Property(t => t.GuardianTelephone)
                .HasMaxLength(20);

            Property(t => t.GuardianYearOfBirth)
                .HasMaxLength(50);

            Property(t => t.MinorContestSignUpId)
                .IsRequired();

            // Table & Column Mappings
            ToTable("MinorUsers");
            Property(t => t.MinorId).HasColumnName("minorId");
            Property(t => t.MinorFirstName).HasColumnName("minorFirstName");
            Property(t => t.MinorLastName).HasColumnName("minorLastName");
            Property(t => t.MinorEmail).HasColumnName("minorEmail");
            Property(t => t.MinorDateOfBirth).HasColumnName("minorDateOfBirth");
            Property(t => t.MinorLanguage).HasColumnName("minorLanguage");
            Property(t => t.MinorApproved).HasColumnName("minorApproved");
            Property(t => t.MinorGuid).HasColumnName("minorGuid");

            Property(t => t.GuardianFirstName).HasColumnName("guardianFirstName");
            Property(t => t.GuardianLastName).HasColumnName("guardianLastName");
            Property(t => t.GuardianEmail).HasColumnName("guardianEmail");
            Property(t => t.GuardianAddress1).HasColumnName("guardianAddress1");
            Property(t => t.GuardianAddress2).HasColumnName("guardianAddress2");
            Property(t => t.GuardianCity).HasColumnName("guardianCity");
            Property(t => t.GuardianProvince).HasColumnName("guardianProvince");
            Property(t => t.GuardianPostalCode).HasColumnName("guardianPostalCode");
            Property(t => t.GuardianTelephone).HasColumnName("guardianTelephone");
            Property(t => t.GuardianYearOfBirth).HasColumnName("guardianYearOfBirth");
            Property(t => t.MinorContestSignUpId).HasColumnName("minorContestSignUpId");

            Property(t => t.DateCreated).HasColumnName("minorDateCreated");
            Property(t => t.DateModified).HasColumnName("minorDateModified");

            Property(t => t.MinorAcceptRules).HasColumnName("minorAcceptRules");
        }
    }
}
