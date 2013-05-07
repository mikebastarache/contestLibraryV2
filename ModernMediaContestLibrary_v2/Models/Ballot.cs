using System;
using System.Collections.Generic;

namespace MMContest.Models
{
    public class Ballot
    {
        public Ballot()
        {
            Votes = new List<Vote>();
        }

        public int BallotId { get; set; }
        public int? UserId { get; set; }
        public int? ContestId { get; set; }
        public int? ReferrerUserId { get; set; }
        public DateTime? DateCreated { get; set; }
        public bool? IsBonusBallot { get; set; }
        public bool? IsShareBonusBallot { get; set; }
        public string Source { get; set; }
        public string IpAddress { get; set; }
        
        public virtual Contest Contest { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
