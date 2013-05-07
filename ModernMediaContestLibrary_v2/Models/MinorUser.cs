using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MMContest.Models
{
    public class MinorUser
    {
        public MinorUser()
        {
            MinorVotes = new List<MinorVote>();
            MinorUserContestRegistrations = new List<MinorUserContestRegistration>();
        }

        [ScaffoldColumn(false)]
        public int MinorId { get; set; }
        
        [Required]
        [DisplayName("First Name")]
        public string MinorFirstName { get; set; }

        [DisplayName("Last Name")]
        public string MinorLastName { get; set; }

        [Required]
        [DisplayName("Email")]
        public string MinorEmail { get; set; }

        [Required]
        [DisplayName("Date of Birth")]
        public DateTime MinorDateOfBirth { get; set; }

        [DisplayName("Language")]
        public string MinorLanguage { get; set; }

        public bool? MinorApproved { get; set; }

        public Guid MinorGuid { get; set; }
        
        [DisplayName("Guardian First Name")]
        public string GuardianFirstName { get; set; }

        [DisplayName("Guardian Last Name")]
        public string GuardianLastName { get; set; }

        [Required]
        [DisplayName("Guardian Email")]
        public string GuardianEmail { get; set; }

        [DisplayName("Guardian Address")]
        public string GuardianAddress1 { get; set; }
        public string GuardianAddress2 { get; set; }

        [DisplayName("Guardian City")]
        public string GuardianCity { get; set; }

        [DisplayName("Guardian Province")]
        public string GuardianProvince { get; set; }

        [DisplayName("Guardian Postal Code")]
        public string GuardianPostalCode { get; set; }

        [DisplayName("Guardian Telephone")]
        public string GuardianTelephone { get; set; }

        [DisplayName("GuardianYear of Birth")]
        public string GuardianYearOfBirth { get; set; }

        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public int MinorContestSignUpId { get; set; }

        public bool? MinorAcceptRules { get; set; }
        
        public virtual ICollection<MinorVote> MinorVotes { get; set; }
        public virtual ICollection<MinorUserContestRegistration> MinorUserContestRegistrations { get; set; }

    }
}
