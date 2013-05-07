using System;

namespace MMContest.Models
{
    public class Friend
    {
        public int FriendId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int? ReferrerUserId { get; set; }
        public int? ContestId { get; set; }
        public DateTime? DateCreated { get; set; }
        public Guid? InvitationIdentifier { get; set; }
        public virtual Contest Contest { get; set; }
        public virtual User User { get; set; }
    }
}
