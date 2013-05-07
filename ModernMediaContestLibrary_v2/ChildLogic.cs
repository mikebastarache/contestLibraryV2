using System;
using MMContest.Dal;
using MMContest.Models;
using System.Linq;

namespace MMContest
{
    public class ChildLogic : MicrositeLogic
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        public string EnterContest(string email, int contestId, Guid guid = new Guid())
        {
            string result;

            if (DoesEmailExist(email))
            {
                //USER LOGIC()

                // IF USER HAVE NOT ENTERED THIS CONTEST, SEND TO CONFIRM AGE OF MAJORITY PAGE
                if (!HasUserEnteredThisContest(email, contestId))
                    result = "userHasNotEnteredThisContest";

                // IF USER HAS A BALLOT TODAY, SEND TO ALREADY ENTERED PAGE
                else if (HasUserEnteredContestToday(email, contestId))
                    result = "userHasAlreadyEnteredToday";

                // IF EMAIL IS REGISTERED AS A USER, SEND TO NORMAL START THE OF APPLICATION
                else
                    result = "userIsReadyToEnterTheContest";

            }
            else if (DoesMinorApprovedEmailExist(email))
            {
                //MINOR USER LOGIC()

                // IF MINOR HAVE NOT ENTERED THIS CONTEST, SEND TO CONFIRM AGE OF MAJORITY PAGE
                if (!HasMinorEnteredThisContest(email, contestId))
                    result = "minorUserHasNotEnteredThisContest";

                // IF MINOR HAS A VOTE TODAY, SEND TO ALREADY ENTERED PAGE
                else if (HasMinorEnteredContestToday(email, contestId))
                    result = "userHasAlreadyEnteredToday";

                // IF EMAIL IS REGISTERED AS A MINOR, SEND TO MINOR'S SECTION OF APPLICATION
                else if (DoesMinorEmailExist(email))
                    result = "minorUserIsReadyToEnterTheContest";

                // IF EMAIL IS REGISTERED AS A USER, SEND TO NORMAL START THE OF APPLICATION
                else
                    result = "userIsReadyToEnterTheContest";

                // IF USER HAS CHILD REQUEST, SEND THEM DIRECTLY TO CHILD CONSCENT FORM
                if (DoesMinorRequestExist(guid))
                    result = "userHasMinorRequest";

            }
            else
            {
                //NO USER OR MINOR WAS FOUND IN DATABASE
                result = "userDoesNotExistInCRM";
            }

            return result;
        }

        public MinorUser GetMinor(Guid guid)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(u => u.MinorGuid == guid);

            if (minor != null)
                return minor;
            return null;
        }

        public int GetMinorId(string email)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(u => u.MinorEmail == email);

            if (minor != null)
                return minor.MinorId;
            return -1;
        }

        public bool DoesMinorEmailExist(string email)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(m => m.MinorEmail == email);

            return (minor != null);
        }

        public bool DoesMinorApprovedEmailExist(string email)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(m => m.MinorEmail == email && m.MinorApproved == true);

            return (minor != null);
        }

        public bool DoesMinorDisapprovedEmailExist(string email)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(m => m.MinorEmail == email && m.MinorApproved == false);

            return (minor != null);
        }

        public bool HasMinorEnteredThisContest(string email, int contestId)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(u => u.MinorEmail == email);

            var counter = minor.MinorUserContestRegistrations.Count(ucr => ucr.ContestId == contestId);

            return (counter > 0);
        }

        public bool HasMinorEnteredContestToday(string email, int contestId)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(u => u.MinorEmail == email);

            var counter = minor.MinorVotes.Count(vote => Convert.ToDateTime(vote.DateCreated).Date == DateTime.Now.Date && vote.ContestId == contestId);

            return (counter > 0);
        }

        public void CreateMinorVote(int minorId,  int contestId, string voteValue, string ipAddress)
        {
            // INSERT VOTE
            var vote = new MinorVote
            {
                MinorId = minorId,
                ContestId = contestId,
                MinorVoteValue = voteValue,
                IpAddress = ipAddress,
                DateCreated = DateTime.Now
            };

            _unitOfWork.MinorVoteRepository.Insert(vote);
            _unitOfWork.Save();
        }

        public bool DoesMinorRequestExist(Guid guid)
        {
            var minor = _unitOfWork.MinorUserRepository.GetSingle(m => m.MinorGuid == guid);

            return (minor != null);
        }

        public string RegisterMinor(MinorUser newMinorUser, string emailServerAddress, string sendingClient, string emailSubject, string emailBody, string emailHeader, string emailFooter)
        {
            _unitOfWork.MinorUserRepository.Insert(newMinorUser);
            _unitOfWork.Save();

            // Send welcome email IF user opted in to the CRM program
            var emailResult = SendEmail(emailServerAddress, newMinorUser.GuardianEmail, newMinorUser.MinorLanguage, sendingClient, emailSubject, emailBody, emailHeader, emailFooter);

            string result = newMinorUser.MinorId > 0 ? "AccountCreationSuccessful" : "AccountCreationFailed";

            return result;
        }
        
        public string ApproveMinor(MinorUser formMinorUser, int contestId)
        {
            var result = "";
            var minor = _unitOfWork.MinorUserRepository.GetSingle(m => m.MinorGuid == formMinorUser.MinorGuid);
            if (minor != null)
            {
                // Minor Information
                minor.MinorFirstName = formMinorUser.MinorFirstName;
                minor.MinorLastName = formMinorUser.MinorLastName;
                minor.MinorDateOfBirth = formMinorUser.MinorDateOfBirth;

                // Guardian Information
                minor.GuardianFirstName = formMinorUser.GuardianFirstName;
                minor.GuardianLastName = formMinorUser.GuardianLastName;
                minor.GuardianEmail = formMinorUser.GuardianEmail;
                minor.GuardianAddress1 = formMinorUser.GuardianAddress1;
                minor.GuardianAddress2 = formMinorUser.GuardianAddress2;
                minor.GuardianCity = formMinorUser.GuardianCity;
                minor.GuardianProvince = formMinorUser.GuardianProvince;
                minor.GuardianPostalCode = formMinorUser.GuardianPostalCode;
                minor.GuardianTelephone = formMinorUser.GuardianTelephone;
                minor.GuardianYearOfBirth = formMinorUser.GuardianYearOfBirth;

                // Set Guardian Approval
                minor.MinorApproved = true;
                minor.DateModified = DateTime.Now;

                // Agree to rules
                minor.MinorAcceptRules = formMinorUser.MinorAcceptRules;

                _unitOfWork.MinorUserRepository.Update(minor);
                _unitOfWork.Save();

                result = minor.MinorId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";

                //register minor to the database
                CreateMinorContestRegistration(minor.MinorId, contestId);
            }
            return result;
        }

        public string DoNotApproveMinor(MinorUser formMinorUser, int contestId)
        {
            var result = "";
            var minor = _unitOfWork.MinorUserRepository.GetSingle(m => m.MinorGuid == formMinorUser.MinorGuid);
            if (minor != null)
            {
                // Set Guardian Non Approval
                minor.MinorApproved = false;

                // Agree to rules
                minor.MinorAcceptRules = formMinorUser.MinorAcceptRules;

                minor.DateModified = DateTime.Now;

                _unitOfWork.MinorUserRepository.Update(minor);
                _unitOfWork.Save();

                result = minor.MinorId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";
            }
            return result;
        }

        private bool CreateMinorContestRegistration(int minorId, int contestId)
        {
            var minorContestRegistration = new MinorUserContestRegistration
            {
                MinorId = minorId,
                ContestId = contestId,
                DateCreated = DateTime.Now
            };

            _unitOfWork.MinorUserContestRegistrationRepository.Insert(minorContestRegistration);
            _unitOfWork.Save();

            return minorContestRegistration.Id > 0;
        }

        public string IsAgeOfMajority(int yearOfBirth, int monthOfBirth, int dayOfBirth, string province)
        {
            //*****************************************************************************************
            // The current age of majority in the individual provinces and territories of Canada is 
            //
            // AGE 18                                          AGE 19
            //-------------------------------                  ----------------------------
            //    AB - Alberta                                    BC - British Columbia
            //    MB - Manitoba                                   NB - New Brunswick
            //    ON - Ontario                                    NL - Newfoundland and Labrador
            //    PE - Prince Edward Island                       NT - NorthwestTerritories
            //    QC - Quebec                                     NS - Nova Scotia
            //    SK - Saksatchewan                               NU - Nunavut
            //                                                    YT - Yukon Territory 
            //*****************************************************************************************

            const string provinceList18 = "AB, MB, ON, PE, QC, SK";
            const string provinceList19 = "BC, NB, NL, NT, NS, NU, YT";

            DateTime now = DateTime.Now;
            DateTime origin = new DateTime(yearOfBirth, monthOfBirth, dayOfBirth);

            var result = "userIsAgeOfMajority";
            var yearsPassed = CalculateAgeCorrect(origin, now);

            if (yearsPassed < 6)
                result = "userIsNotEligible";

            else if ((yearsPassed >= 6 && yearsPassed < 18) && (provinceList18.IndexOf(province.ToUpper(), StringComparison.Ordinal) > -1))
                result = "userIsMinor";

            else if ((yearsPassed >= 6 && yearsPassed < 19) && (provinceList19.IndexOf(province.ToUpper(), StringComparison.Ordinal) > -1))
                result = "userIsMinor";

            return result;
        }

    }
}
