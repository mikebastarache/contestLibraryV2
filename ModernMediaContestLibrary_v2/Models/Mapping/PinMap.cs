using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class PinMap : EntityTypeConfiguration<Pin>
    {
        public PinMap()
        {
            // Primary Key
            HasKey(t => t.PinId);

            // Properties
            // Table & Column Mappings
            ToTable("Pins");
            Property(t => t.PinId).HasColumnName("id");
            Property(t => t.ContestId).HasColumnName("contestId");
            Property(t => t.PinNumber).HasColumnName("pin");
            Property(t => t.PrizeEn).HasColumnName("prize_en");
            Property(t => t.PrizeFr).HasColumnName("prize_fr");
            Property(t => t.UserId).HasColumnName("userId");
            Property(t => t.SkillTestingAnswer).HasColumnName("skillTestingAnswer");
            Property(t => t.IsWinner).HasColumnName("isWinner");
            Property(t => t.IpAddress).HasColumnName("ipAddress");
            Property(t => t.Source).HasColumnName("source");
            Property(t => t.DateCreated).HasColumnName("dateCreated");
            Property(t => t.DateModified).HasColumnName("dateModified");
        }
    }
}
