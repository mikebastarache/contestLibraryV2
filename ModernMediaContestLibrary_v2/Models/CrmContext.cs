using System.Data.Entity;
using MMContest.Models.Mapping;

namespace MMContest.Models
{
	public class CrmContext : DbContext
	{
		static CrmContext()
		{
			Database.SetInitializer<CrmContext>(null);
		}

		public CrmContext()
			: base("Name=CrmContext")
		{
		}

        public CrmContext(string connectionString)
            : base(connectionString)
        {
        }

		public DbSet<Ballot> Ballots { get; set; }
		public DbSet<Contest> Contests { get; set; }
		public DbSet<Friend> Friends { get; set; }
		public DbSet<InstantWin> InstantWins { get; set; }
        public DbSet<Pin> Pins { get; set; }
		public DbSet<Province> Provinces { get; set; }
		public DbSet<Salutation> Salutations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MinorUser> MinorUsers { get; set; }
        public DbSet<UserContestRegistration> UserContestRegistrations { get; set; }
        public DbSet<MinorUserContestRegistration> MinorUserContestRegistrations { get; set; }
		public DbSet<Vote> Votes { get; set; }
        public DbSet<MinorVote> MinorVotes { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			modelBuilder.Configurations.Add(new BallotMap());
			modelBuilder.Configurations.Add(new ContestMap());
			modelBuilder.Configurations.Add(new FriendMap());
			modelBuilder.Configurations.Add(new InstantWinMap());
            modelBuilder.Configurations.Add(new PinMap());
			modelBuilder.Configurations.Add(new ProvinceMap());
			modelBuilder.Configurations.Add(new SalutationMap());
			modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserContestRegistrationMap());
            modelBuilder.Configurations.Add(new MinorUserContestRegistrationMap());
            modelBuilder.Configurations.Add(new VoteMap());
            modelBuilder.Configurations.Add(new MinorUserMap());
            modelBuilder.Configurations.Add(new MinorVoteMap());
		}
	}
}
