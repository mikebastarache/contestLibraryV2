using System;

namespace MMContest.Models
{
    public class Pin
    {
        public int PinId { get; set; }
        public int ContestId { get; set; }
        public string PinNumber { get; set; }
        public string PrizeEn { get; set; }
        public string PrizeFr { get; set; }
        public int? UserId { get; set; }
        public string SkillTestingAnswer { get; set; }
        public bool? IsWinner { get; set; }
        public string IpAddress { get; set; }
        public string Source { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
    }
}
