using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMContest.Models
{
    public class User
    {
        public User()
        {
            Ballots = new List<Ballot>();
            Friends = new List<Friend>();
            UserContestRegistrations = new List<UserContestRegistration>();
            Votes = new List<Vote>();
        }

        
        public int UserId { get; set; }
        public int? Salutation { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string PostalCode { get; set; }
        public string Telephone { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string Language { get; set; }
        public bool OptIn { get; set; }
        public string YearOfBirth { get; set; }
        public int? OriginalFriendId { get; set; }
        public int? ContestSignupId { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public string Fbuid { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Ballot> Ballots { get; set; }
        public virtual ICollection<Friend> Friends { get; set; }
        public virtual ICollection<UserContestRegistration> UserContestRegistrations { get; set; }
        public virtual ICollection<Vote> Votes { get; set; }
    }
}
