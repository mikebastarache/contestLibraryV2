using System;

namespace MMContest.Models
{
    public class MinorVote
    {
        public int MinorVoteId { get; set; }
        public int ContestId { get; set; }
        public int MinorId { get; set; }
        public string MinorVoteValue { get; set; }
        public string IpAddress { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual MinorUser MinorUser { get; set; }
    }
}
