using System;

namespace MMContest.Models
{
    public class UserContestRegistration
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public int ContestId { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual User User { get; set; }
    }
}
