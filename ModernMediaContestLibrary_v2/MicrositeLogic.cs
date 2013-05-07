using System;
using System.Linq;
using MMContest.Dal;

namespace MMContest
{
    public class MicrositeLogic : ContestLogic
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        
        public string EnterContest(string email, int contestId)
        {
            string result;

            if (!DoesEmailExist(email))
                result = "userDoesNotExistInCRM";

            else if (!HasUserEnteredThisContest(email, contestId))
                result = "userHasNotEnteredThisContest";

            else if (HasUserEnteredContestToday(email, contestId))
                result = "userHasAlreadyEnteredToday";
                
            else
                result = "userIsReadyToEnterTheContest";

            return result;
        }

        public int GetUserId(string email)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            if (user != null)
                return user.UserId;
            return -1;
        }

        public bool DoesEmailExist(string email)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            return (user != null);
        }

        public bool HasUserEnteredThisContest(string email, int contestId)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            var counter = user.UserContestRegistrations.Count(ucr => ucr.ContestId == contestId);

            return (counter > 0);
        }

        public bool HasUserVotedToday(string email, int contestId)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            var counter = user.Votes.Count(vote => Convert.ToDateTime(vote.DateCreated).Date == DateTime.Now.Date && vote.ContestId == contestId);

            return (counter > 0);
        }

        public bool HasUserEnteredContestToday(string email, int contestId)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            var counter = user.Ballots.Count(ballot => Convert.ToDateTime(ballot.DateCreated).Date == DateTime.Now.Date && ballot.Source != "Invitation" && ballot.ContestId == contestId);

            return (counter > 0);
        }
    }
}
