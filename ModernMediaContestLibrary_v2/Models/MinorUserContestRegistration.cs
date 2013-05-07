using System;

namespace MMContest.Models
{
    public class MinorUserContestRegistration
    {
        public int Id { get; set; }
        public int? MinorId { get; set; }
        public int ContestId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual MinorUser MinorUser { get; set; }
    }
}
