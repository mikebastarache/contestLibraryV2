using System;

namespace MMContest.Models
{
    public class Vote
    {
        public int VoteId { get; set; }
        public int ContestId { get; set; }
        public int? BallotId { get; set; }
        public DateTime? DateCreated { get; set; }
        public string VoteValue { get; set; }
        public string IpAddress { get; set; }
        public int? UserId { get; set; }

        public virtual Ballot Ballot { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual User User { get; set; }
    }
}
