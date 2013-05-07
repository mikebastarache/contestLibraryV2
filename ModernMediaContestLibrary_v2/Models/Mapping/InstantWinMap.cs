using System.Data.Entity.ModelConfiguration;

namespace MMContest.Models.Mapping
{
    public class InstantWinMap : EntityTypeConfiguration<InstantWin>
    {
        public InstantWinMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("InstantWins");
            Property(t => t.Id).HasColumnName("id");
            Property(t => t.ContestId).HasColumnName("contestId");
            Property(t => t.DateProposed).HasColumnName("dateProposed");
            Property(t => t.PrizeEn).HasColumnName("prize_en");
            Property(t => t.PrizeFr).HasColumnName("prize_fr");
            Property(t => t.UserId).HasColumnName("userId");
            Property(t => t.DateAwarded).HasColumnName("dateAwarded");
            Property(t => t.IsWinner).HasColumnName("isWinner");
            Property(t => t.SkillTestingAnswer).HasColumnName("skillTestingAnswer");
            Property(t => t.IpAddress).HasColumnName("ipAddress");
            Property(t => t.Source).HasColumnName("source");
            Property(t => t.DateCreated).HasColumnName("dateCreated");
            Property(t => t.DateModified).HasColumnName("dateModified");
        }
    }
}