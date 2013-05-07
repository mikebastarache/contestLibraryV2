using System;

namespace MMContest.Models
{
    public class InstantWin
    {
        public int Id { get; set; }
        public int? ContestId { get; set; }
        public DateTime DateProposed { get; set; }
        public string PrizeEn { get; set; }
        public string PrizeFr { get; set; }
        public int? UserId { get; set; }
        public DateTime? DateAwarded { get; set; }
        public bool? IsWinner { get; set; }
        public string SkillTestingAnswer { get; set; }
        public string IpAddress { get; set; }
        public string Source { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}