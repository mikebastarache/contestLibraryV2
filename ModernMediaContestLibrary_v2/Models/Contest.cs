using System;
using System.Collections.Generic;

namespace MMContest.Models
{
    public class Contest
    {
        public Contest()
        {
            Ballots = new List<Ballot>();
            Friends = new List<Friend>();
            UserContestRegistrations = new List<UserContestRegistration>();
            MinorUserContestRegistrations = new List<MinorUserContestRegistration>();
            MinorVotes = new List<MinorVote>();
            Votes = new List<Vote>();
        }

        public int ContestId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public virtual ICollection<Ballot> Ballots { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<UserContestRegistration> UserContestRegistrations { get; set; }
        public virtual ICollection<MinorUserContestRegistration> MinorUserContestRegistrations { get; set; }
        public virtual ICollection<MinorVote> MinorVotes { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
