using System;
using System.Globalization;
using MMContest.Dal;
using MMContest.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Linq;


namespace MMContest
{
    public abstract class ContestLogic
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();

        #region User Methods

        public string RegisterCrm(User myUser, string emailServerAddress, string sendingClient)
        {
            var result = "";

            // GET the friend record if this was from an invitation
            int? OriginalFriendId;
            if (myUser.OriginalFriendId != null)
            {
                OriginalFriendId = CheckFriendInvitation(myUser.Email, Convert.ToInt32(myUser.ContestSignupId));
            }
            else
                OriginalFriendId = null;

            try
            {
                // Verify if user already exists in DB.
                // if yes: UPDATE user account
                // if no: CREATE new user account
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == myUser.Email);
                if (user != null)
                {
                    user.Salutation = myUser.Salutation;
                    user.FirstName = myUser.FirstName;
                    user.LastName = myUser.LastName;
                    user.Address1 = myUser.Address1;
                    user.Address2 = myUser.Address2;
                    user.City = myUser.City;
                    user.Province = myUser.Province;
                    user.PostalCode = myUser.PostalCode;
                    user.Telephone = myUser.Telephone;
                    user.Language = myUser.Language;
                    user.OptIn = myUser.OptIn;
                    user.YearOfBirth = myUser.YearOfBirth;
                    user.OriginalFriendId = OriginalFriendId;      // Keep the value from the database
                    user.ContestSignupId = user.ContestSignupId;        // Keep the value from the database
                    user.Fbuid = string.IsNullOrEmpty(user.Fbuid) ? myUser.Fbuid : user.Fbuid;
                    user.DateCreated = user.DateCreated;                // Keep the value from the database
                    user.DateModified = DateTime.Now;
                    user.Password = GeneratePassword();                 // Generate new password for security

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    result = user.UserId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";

                }
                else
                {
                    var newUser = new User
                    {
                        Salutation = myUser.Salutation,
                        FirstName = myUser.FirstName,
                        LastName = myUser.LastName,
                        Email = myUser.Email,
                        Address1 = myUser.Address1,
                        Address2 = myUser.Address2,
                        City = myUser.City,
                        Province = myUser.Province,
                        PostalCode = myUser.PostalCode,
                        Telephone = myUser.Telephone,
                        Language = myUser.Language,
                        OptIn = myUser.OptIn,
                        YearOfBirth = myUser.YearOfBirth,
                        OriginalFriendId = OriginalFriendId,
                        ContestSignupId = myUser.ContestSignupId,
                        Fbuid = myUser.Fbuid,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now,
                        Password = GeneratePassword()
                    };

                    _unitOfWork.UserRepository.Insert(newUser);
                    _unitOfWork.Save();

                    // Send welcome email IF user opted in to the CRM program
                    if (myUser.OptIn)
                    {
                        var emailResult = SendEmail(emailServerAddress, myUser.Email, myUser.Language, sendingClient);
                    }

                    result = newUser.UserId > 0 ? "AccountCreationSuccessful" : "AccountCreationFailed";
                }
            }
            catch (Exception ae)
            {
                result = ae.Message;
                return result;
            }
            return result;
        }


        public string Register(User myUser, string emailServerAddress, string sendingClient)
        {
            var result = "";

            try
            {
                // Verify if User is Age of Majority in his province
                if (!IsAgeOfMajority(myUser.YearOfBirth, myUser.Province))
                    throw new ApplicationException("UserNotAgeOfMajority");

                // GET the friend record if this was from an invitation
                int? OriginalFriendId;
                if (myUser.OriginalFriendId != null)
                {
                    OriginalFriendId = CheckFriendInvitation(myUser.Email, Convert.ToInt32(myUser.ContestSignupId));
                }
                else
                    OriginalFriendId = null;

                // Verify if user already exists in DB.
                // if yes: UPDATE user account
                // if no: CREATE new user account
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == myUser.Email);
                if (user != null)
                {
                    user.Salutation = myUser.Salutation;
                    user.FirstName = myUser.FirstName;
                    user.LastName = myUser.LastName;
                    user.Address1 = myUser.Address1;
                    user.Address2 = myUser.Address2;
                    user.City = myUser.City;
                    user.Province = myUser.Province;
                    user.PostalCode = myUser.PostalCode;
                    user.Telephone = myUser.Telephone;
                    user.Language = myUser.Language;
                    user.OptIn = myUser.OptIn;
                    user.YearOfBirth = myUser.YearOfBirth;
                    user.OriginalFriendId = OriginalFriendId;
                    user.ContestSignupId = user.ContestSignupId;        // Keep the value from the database
                    user.Fbuid = string.IsNullOrEmpty(user.Fbuid) ? myUser.Fbuid : user.Fbuid;
                    user.DateCreated = user.DateCreated;                // Keep the value from the database
                    user.DateModified = DateTime.Now;

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    result = user.UserId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";

                }
                else
                {
                    var newUser = new User
                    {
                        Salutation = myUser.Salutation,
                        FirstName = myUser.FirstName,
                        LastName = myUser.LastName,
                        Email = myUser.Email,
                        Address1 = myUser.Address1,
                        Address2 = myUser.Address2,
                        City = myUser.City,
                        Province = myUser.Province,
                        PostalCode = myUser.PostalCode,
                        Telephone = myUser.Telephone,
                        Language = myUser.Language,
                        OptIn = myUser.OptIn,
                        YearOfBirth = myUser.YearOfBirth,
                        OriginalFriendId = OriginalFriendId,
                        ContestSignupId = myUser.ContestSignupId,
                        Fbuid = myUser.Fbuid,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };

                    _unitOfWork.UserRepository.Insert(newUser);
                    _unitOfWork.Save();

                    // Send welcome email IF user opted in to the CRM program
                    if (myUser.OptIn)
                    {
                        var emailResult = SendEmail(emailServerAddress, myUser.Email, myUser.Language, sendingClient);
                    }

                    result = newUser.UserId > 0 ? "AccountCreationSuccessful" : "AccountCreationFailed";
                }
            }
            catch (Exception ae)
            {
                switch (ae.Message)
                {
                    case "UserNotAgeOfMajority":
                        result = "NotAgeOfMajority";
                        break;
                }
                return result;
            }
            return result;
        }


        public string Register(User myUser, int minimumAgeRequiredToEnter, string emailServerAddress, string sendingClient)
        {
            var result = "";
            //DateTime dob = myUser.DateOfBirth.GetValueOrDefault(DateTime.Now);

            try
            {
                // Verify if user is old enough to enter the contest.
                //if (!IsMinimumAge(minimumAgeRequiredToEnter, dob))
                //    throw new ApplicationException("UserNotAgeOfMajority");

                // GET the friend record if this was from an invitation
                int? originalFriendId;
                if (myUser.OriginalFriendId != null)
                {
                    originalFriendId = CheckFriendInvitation(myUser.Email, Convert.ToInt32(myUser.ContestSignupId));
                }
                else
                    originalFriendId = null;

                // Verify if user already exists in DB.
                // if yes: UPDATE user account
                // if no: CREATE new user account
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == myUser.Email);
                if (user != null)
                {
                    user.Salutation = myUser.Salutation;
                    user.FirstName = myUser.FirstName;
                    user.LastName = myUser.LastName;
                    user.Address1 = myUser.Address1;
                    user.Address2 = myUser.Address2;
                    user.City = myUser.City;
                    user.Province = myUser.Province;
                    user.PostalCode = myUser.PostalCode;
                    user.Telephone = myUser.Telephone;
                    user.Language = myUser.Language;
                    user.OptIn = myUser.OptIn;
                    user.DateOfBirth = myUser.DateOfBirth.GetValueOrDefault();
                    user.YearOfBirth = myUser.DateOfBirth.GetValueOrDefault().Year.ToString();
                    user.OriginalFriendId = null;
                    user.ContestSignupId = user.ContestSignupId;        // Keep the value from the database
                    user.Fbuid = string.IsNullOrEmpty(user.Fbuid) ? myUser.Fbuid : user.Fbuid;
                    user.DateCreated = user.DateCreated;                // Keep the value from the database
                    user.DateModified = DateTime.Now;

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    result = user.UserId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";

                }
                else
                {
                    var newUser = new User
                    {
                        Salutation = myUser.Salutation,
                        FirstName = myUser.FirstName,
                        LastName = myUser.LastName,
                        Email = myUser.Email,
                        Address1 = myUser.Address1,
                        Address2 = myUser.Address2,
                        City = myUser.City,
                        Province = myUser.Province,
                        PostalCode = myUser.PostalCode,
                        Telephone = myUser.Telephone,
                        Language = myUser.Language,
                        OptIn = myUser.OptIn,
                        DateOfBirth = myUser.DateOfBirth.GetValueOrDefault(),
                        YearOfBirth = myUser.DateOfBirth.GetValueOrDefault().Year.ToString(),
                        OriginalFriendId = originalFriendId,
                        ContestSignupId = myUser.ContestSignupId,
                        Fbuid = myUser.Fbuid,
                        DateCreated = DateTime.Now,
                        DateModified = DateTime.Now
                    };

                    _unitOfWork.UserRepository.Insert(newUser);
                    _unitOfWork.Save();

                    // Send welcome email IF user opted in to the CRM program
                    if (myUser.OptIn)
                    {
                        var emailResult = SendEmail(emailServerAddress, myUser.Email, myUser.Language, sendingClient);
                    }

                    result = newUser.UserId > 0 ? "AccountCreationSuccessful" : "AccountCreationFailed";
                }
            }
            catch (Exception ae)
            {
                switch (ae.Message)
                {
                    case "UserNotAgeOfMajority":
                        result = "NotAgeOfMajority";
                        break;
                }
                return result;
            }
            return result;
        }

        public string Register(int? salutation,
            string firstName,
            string lastName,
            string email,
            string address1,
            string address2,
            string city,
            string province,
            string postalCode,
            string telephone,
            string language,
            bool optin,
            string yearOfBirth,
            int inviterId,
            int? contestSignupId,
            string fbuid,
            string emailServerAddress, 
            string sendingClient)
        {
            var result = "";

            try
            {
                // Verify if User is Age of Majority in his province
                if (!IsAgeOfMajority(yearOfBirth, province))
                    throw new ApplicationException("UserNotAgeOfMajority");

                // GET the friend record if this was from an invitation
                int? OriginalFriendId;
                
                if (inviterId != null)
                {
                    OriginalFriendId = CheckFriendInvitation(email, Convert.ToInt32(contestSignupId));
                }
                else
                    OriginalFriendId = null;

                // Verify if user already exists in DB.
                // if yes: UPDATE user account
                // if no: CREATE new user account
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);
                if (user != null)
                {
                    user.Salutation = salutation;
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.Address1 = address1;
                    user.Address2 = address2;
                    user.City = city;
                    user.Province = province;
                    user.PostalCode = postalCode;
                    user.Telephone = telephone;
                    user.Language = language;
                    user.OptIn = optin;
                    user.YearOfBirth = yearOfBirth;
                    user.OriginalFriendId = OriginalFriendId;
                    user.ContestSignupId = user.ContestSignupId;     // Keep the value from the database
                    user.Fbuid = string.IsNullOrEmpty(user.Fbuid) ? fbuid : user.Fbuid;
                    user.DateCreated = user.DateCreated;             // Keep the value from the database
                    user.DateModified = DateTime.Now;

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    result = user.UserId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";

                }
                else
                {
                    var newUser = new User
                                      {
                                          Salutation = salutation,
                                          FirstName = firstName,
                                          LastName = lastName,
                                          Email = email,
                                          Address1 = address1,
                                          Address2 = address2,
                                          City = city,
                                          Province = province,
                                          PostalCode = postalCode,
                                          Telephone = telephone,
                                          Language = language,
                                          OptIn = optin,
                                          YearOfBirth = yearOfBirth,
                                          OriginalFriendId = OriginalFriendId,
                                          ContestSignupId = contestSignupId,
                                          Fbuid = fbuid,
                                          DateCreated = DateTime.Now,
                                          DateModified = DateTime.Now
                                      };

                    _unitOfWork.UserRepository.Insert(newUser);
                    _unitOfWork.Save();

                    // Send welcome email IF user opted in to the CRM program
                    if (optin)
                    {
                        var emailResult = SendEmail(emailServerAddress, email, language, sendingClient);
                    }

                    result = newUser.UserId > 0 ? "AccountCreationSuccessful" : "AccountCreationFailed";
                }
            }
            catch (Exception ae)
            {
                switch (ae.Message)
                {
                    case "UserNotAgeOfMajority":
                        result = "NotAgeOfMajority";
                        break;
                }
                return result;
            }
            return result;
        }


        public string Register(int? salutation,
            string firstName,
            string lastName,
            string email,
            string address1,
            string address2,
            string city,
            string province,
            string postalCode,
            string telephone,
            string language,
            bool optin,
            DateTime dateOfBirth,
            int inviterId,
            int? contestSignupId,
            int minimumAgeRequiredToEnter,
            string fbuid,
            string emailServerAddress, 
            string sendingClient)
        {
            var result = "test";

            try
            {
                // Verify if user is old enough to enter the contest.
                if (!IsMinimumAge(minimumAgeRequiredToEnter, dateOfBirth))
                    throw new ApplicationException("UserNotAgeOfMajority");

                // GET the friend record if this was from an invitation
                int? OriginalFriendId;
                if (inviterId != null)
                {
                    OriginalFriendId = CheckFriendInvitation(email, Convert.ToInt32(contestSignupId));
                }
                else
                    OriginalFriendId = null;

                // Verify if user already exists in DB.
                // if yes: UPDATE user account
                // if no: CREATE new user account
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);
                if (user != null)
                {
                    user.Salutation = salutation;
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.Address1 = address1;
                    user.Address2 = address2;
                    user.City = city;
                    user.Province = province;
                    user.PostalCode = postalCode;
                    user.Telephone = telephone;
                    user.Language = language;
                    user.OptIn = optin;
                    user.DateOfBirth = dateOfBirth;
                    user.YearOfBirth = dateOfBirth.Year.ToString(CultureInfo.InvariantCulture);
                    user.OriginalFriendId = OriginalFriendId;
                    user.ContestSignupId = user.ContestSignupId;           // Keep the value from the database
                    user.Fbuid = string.IsNullOrEmpty(user.Fbuid) ? fbuid : user.Fbuid;
                    user.DateCreated = user.DateCreated;                   // Keep the value from the database
                    user.DateModified = DateTime.Now;

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    result = user.UserId > 0 ? "AccountUpdateSuccessful" : "AccountUpdateFailed";

                }
                else
                {
                    var newUser = new User
                                      {
                                          Salutation = salutation,
                                          FirstName = firstName,
                                          LastName = lastName,
                                          Email = email,
                                          Address1 = address1,
                                          Address2 = address2,
                                          City = city,
                                          Province = province,
                                          PostalCode = postalCode,
                                          Telephone = telephone,
                                          Language = language,
                                          OptIn = optin,
                                          DateOfBirth = dateOfBirth,
                                          YearOfBirth = dateOfBirth.Year.ToString(),
                                          OriginalFriendId = OriginalFriendId,
                                          ContestSignupId = contestSignupId,
                                          Fbuid = fbuid,
                                          DateCreated = DateTime.Now,
                                          DateModified = DateTime.Now
                                      };

                    _unitOfWork.UserRepository.Insert(newUser);
                    _unitOfWork.Save();

                    // Send welcome email IF user opted in to the CRM program
                    if (optin)
                    {
                        var emailResult = SendEmail(emailServerAddress, email, language, sendingClient);
                    }

                    result = newUser.UserId > 0 ? "AccountCreationSuccessful" : "AccountCreationFailed";
                }
            }
            catch (Exception ae)
            {
                switch (ae.Message)
                {
                    case "UserNotAgeOfMajority":
                        {
                            result = "NotAgeOfMajority";
                            break;
                        }
                    default:
                        {
                            result = ae.Message;
                            break;
                        }
                }
                return result;
            }
            return result;
        }


        public User GetUser(string email)
        {
            User user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            if (user != null)
                return user;

            return null;
        }


        public User GetUser(int userId)
        {
            User user = _unitOfWork.UserRepository.GetSingle(u => u.UserId == userId);

            if (user != null)
                return user;

            return null;
        }


        public bool CreateUserContestRegistration(int userId, int contestId)
        {
            var userContestRegistration = new UserContestRegistration
                             {
                                 UserId = userId,
                                 ContestId = contestId,
                                 DateCreated = DateTime.Now
                             };

            _unitOfWork.UserContestRegistrationRepository.Insert(userContestRegistration);
            _unitOfWork.Save();

            return userContestRegistration.Id > 0;
        }


        public bool IsPasswordValid(string email, string password)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            return (user.Password == password ? true : false);
        }


        public bool UpdateUserPassword(string email)
        {
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);

            user.Password = GeneratePassword();
            user.DateModified = DateTime.Now;

            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.Save();

            return true;
        }


        public string UnsubscribeUser(string email)
        {
            var result = "";

            try
            {
                // Verify if user already exists in DB.
                // if yes: UPDATE user account
                var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);
                if (user != null)
                {
                    user.OptIn = false;
                    user.Password = GeneratePassword();
                    user.DateModified = DateTime.Now;

                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();

                    result = user.UserId > 0 ? "UnsubscribeSuccessful" : "UnsubscribeFailed";

                }
                
            }
            catch (Exception ae)
            {
                result = ae.Message;
                return result;
            }
            return result;
        }


        public bool UpdateUserCrm(string email, string emailServerAddress, string sendingClient, string subjectLine, string body)
        {
            var result = false;

            // Send email with Link for use to click to finish the update profile process
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);
            if (user != null)
            {
                try
                {
                    SendEmail(emailServerAddress, email, user.Language, sendingClient, subjectLine, body);
                    result = true;
                }
                catch (Exception ae)
                {
                    result = false;
                    return result;
                }
            }

            return result;
        }


        public bool SendUserEmail(string email, string emailServerAddress, string sendingClient, string subjectLine, string body, string header, string footer)
        {
            var result = false;

            // Send email with Link for use to click to finish the update profile process
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);
            if (user != null)
            {
                try
                {
                    SendEmail(emailServerAddress, email, user.Language, sendingClient, subjectLine, body, header, footer);
                    result = true;
                }
                catch (Exception ae)
                {
                    result = false;
                    return result;
                }
            }

            return result;
        }



        public bool IsUserPasswordEmpty(string email)
        {
            var result = false;

            // If user password is null then a password is generated
            var user = _unitOfWork.UserRepository.GetSingle(u => u.Email == email);
            if (user != null)
            {
                if (user.Password == null)
                {
                    try
                    {
                        UpdateUserPassword(email);
                        result = true;
                    }
                    catch (Exception ae)
                    {
                        result = false;
                        return result;
                    }
                }

            }
            return result;
        }

        #endregion    

        #region Ballot Methods

        public int CreateNewBallot(int userId, int contestId, string ballotSource, string ipAddress)
        {
            var ballot = new Ballot
                             {
                                 UserId = userId,
                                 ContestId = contestId,
                                 Source = ballotSource,
                                 IpAddress = ipAddress,
                                 DateCreated = DateTime.Now
                             };

            _unitOfWork.BallotRepository.Insert(ballot);
            _unitOfWork.Save();

            return ballot.BallotId;
        }

        public int? CheckFriendInvitation(string inviteeEmail, int contestId)
        {
            // GET the friend record if this was from an invitation
            int? originalFriendId;
            
            var friend = _unitOfWork.FriendRepository.GetMany(f => f.Email == inviteeEmail && f.ContestId == contestId).OrderBy(f => f.DateCreated).First();

            originalFriendId = friend.ReferrerUserId;

            // IF this User has been invited then award a ballot to the Inviter
            if (originalFriendId.HasValue && originalFriendId > 0)
            {
                AwardShareBallot(originalFriendId.Value, Convert.ToInt32(contestId));
            }

            return originalFriendId;
        }

        public bool AwardShareBallot(int inviterId, int contestId, string ballotSource = "Invitation")
        {
            // Verify if inviter has maximum ShareBallots in THIS contest
            var user = _unitOfWork.UserRepository.GetById(inviterId);
            var counter = user.Ballots.Count(b => b.ContestId == contestId && b.IsShareBonusBallot == true);

            // FriendBallotLimit default is set to 10.  Check project settings to change.
            if (counter >= Properties.Settings.Default.FriendBallotLimit)
                return false;

            // Otherwise
            var ballot = new Ballot
            {
                UserId = inviterId,
                ContestId = contestId,
                Source = ballotSource,
                IpAddress = ballotSource,
                DateCreated = DateTime.Now,
                IsShareBonusBallot = true
            };

            _unitOfWork.BallotRepository.Insert(ballot);
            _unitOfWork.Save();

            return ballot.BallotId > 0;
        }


        // RETURNS the number of extra ballots received from friend invitations who have registered in the current contest
        public int TotalExtraFriendBallotsEarned(int inviterId, int contestId)
        {
            User user = _unitOfWork.UserRepository.GetById(inviterId);
            int numberOfExtraBallotsEarned = user.Ballots.Count(b => b.ContestId == contestId && b.IsShareBonusBallot == true);
            return numberOfExtraBallotsEarned;
        }

        #endregion

        #region Vote Methods

        public void CreateVote(int userId, int contestId, string voteValue, string ipAddress)
        {
            // INSERT VOTE
            var vote = new Vote
            {
                UserId = userId,
                ContestId = contestId,
                BallotId = null,
                VoteValue = voteValue,
                IpAddress = ipAddress,
                DateCreated = DateTime.Now
            };

            _unitOfWork.VoteRepository.Insert(vote);
            _unitOfWork.Save();
        }

        #endregion

        #region Friend Methods

        public Friend GetFriendById(int friendId)
        {
            return _unitOfWork.FriendRepository.GetById(friendId);
        }

        public void CreateNewFriend(Friend friend, string language, string emailServerAddress, string sendingClient, string emailSubject, string emailBody, string emailHeader, string emailFooter)
        {
            friend.DateCreated = DateTime.Now;
            
            _unitOfWork.FriendRepository.Insert(friend);
            _unitOfWork.Save();

            // Send welcome email IF user opted in to the CRM program
            var emailResult = SendEmail(emailServerAddress, friend.Email, language, sendingClient, emailSubject, emailBody, emailHeader, emailFooter);
        }

        
        // RETURNS the number of friends a user has invited in the current contest
        public int TotalFriendInvitations(int contestId, int inviterUserId)
        {
            int numberOfFriendInvitations = _unitOfWork.FriendRepository.GetMany(f => f.ReferrerUserId == inviterUserId && f.ContestId == contestId).Count();
            return numberOfFriendInvitations;
        }

        #endregion

        #region Contest Methods

        public Contest GetContestById(int contestId)
        {
            return _unitOfWork.ContestRepository.GetById(contestId);
        }

        #endregion

        #region InstantWin Methods

        public int IsInstantWinner(int userId, int contestId, string ballotSource, string ipAddress)
        {
            InstantWin nextIW = _unitOfWork.InstantWinRepository.GetMany(iw => iw.ContestId == contestId && iw.UserId == null).OrderBy(o => o.DateProposed).FirstOrDefault();

            if (nextIW != null && nextIW.DateProposed < DateTime.Now)
            {
                nextIW.UserId = userId;
                nextIW.DateAwarded = DateTime.Now;
                nextIW.IsWinner = true;
                nextIW.IpAddress = ipAddress;
                nextIW.Source = ballotSource;
                nextIW.DateModified = DateTime.Now;

                _unitOfWork.InstantWinRepository.Update(nextIW);
                _unitOfWork.Save();
                return nextIW.Id;
            }

            return 0;
        }

        public int IsInstantWinner(int userId, int contestId, string ballotSource, string ipAddress, string emailServerAddress, string sendingClient, string emailSubject, string emailBody, string emailHeader, string emailFooter)
        {
            InstantWin nextIW = _unitOfWork.InstantWinRepository.GetMany(iw => iw.ContestId == contestId && iw.UserId == null).OrderBy(o => o.DateProposed).FirstOrDefault();

            if (nextIW != null && nextIW.DateProposed < DateTime.Now)
            {
                nextIW.UserId = userId;
                nextIW.DateAwarded = DateTime.Now;
                nextIW.IsWinner = true;
                nextIW.IpAddress = ipAddress;
                nextIW.Source = ballotSource;
                nextIW.DateModified = DateTime.Now;

                _unitOfWork.InstantWinRepository.Update(nextIW);
                _unitOfWork.Save();

                // Send Instant Win Email
                User user = _unitOfWork.UserRepository.GetSingle(u => u.UserId == userId);
                emailBody = emailBody.Replace("[PIN]", nextIW.PrizeEn);
                var emailResult = SendEmail(emailServerAddress, user.Email, user.Language, sendingClient, emailSubject, emailBody, emailHeader, emailFooter);

                return nextIW.Id;
            }

            return 0;
        }

        public InstantWin GetInstantWin(int id)
        {
            return _unitOfWork.InstantWinRepository.GetSingle(iw => iw.Id == id);
        }

        #endregion

        #region Pin Methods

        public bool DoesPinExist(string pin, int contestId)
        {
            var pins = _unitOfWork.PinRepository.GetMany(p => p.PinNumber == pin && p.ContestId == contestId);
            bool anyPins = pins.Any();

            return anyPins;
        }
  
        public bool IsPinUsed(string pin, int contestId)
        {
            var pins = _unitOfWork.PinRepository.GetMany(p => p.PinNumber == pin && p.ContestId == contestId && p.UserId != null);
            bool anyPins = pins.Any();

            return anyPins;
        }

        public bool UpdatePin(Pin updatedPin)
        {
            _unitOfWork.PinRepository.Update(updatedPin);
            _unitOfWork.Save();

            return true;
        }

        public bool SendConfirmationEmail(int userId, string emailServerAddress, string sendingClient, string emailSubject, string emailBody, string emailHeader, string emailFooter)
        {
            // Send Instant Win Email
            User user = _unitOfWork.UserRepository.GetSingle(u => u.UserId == userId);
            //emailBody = emailBody.Replace("[PIN]", nextIW.PinNumber);
            var emailResult = SendEmail(emailServerAddress, user.Email, user.Language, sendingClient, emailSubject, emailBody, emailHeader, emailFooter);

            return true;
        }

        #endregion

        #region Utility Methods

        private bool IsAgeOfMajority(string yearOfBirth, string province)
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

            var isLegal = true;
            var yearsPassed = Convert.ToInt32(DateTime.Now.Year) - Convert.ToInt32(yearOfBirth);

            if ((yearsPassed < 18) && (provinceList18.IndexOf(province.ToUpper(), StringComparison.Ordinal) > -1))
                isLegal = false;
            else if ((yearsPassed < 19) && (provinceList19.IndexOf(province.ToUpper(), StringComparison.Ordinal) > -1))
                isLegal = false;

            return isLegal;
        }

        private string IsAgeOfMajority(int yearOfBirth, int monthOfBirth, int dayOfBirth, string province)
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

            if ((yearsPassed < 18) && (provinceList18.IndexOf(province.ToUpper(), StringComparison.Ordinal) > -1))
                result = "minorUserAgeOfMajority";

            else if ((yearsPassed < 19) && (provinceList19.IndexOf(province.ToUpper(), StringComparison.Ordinal) > -1))
                result = "minorUserAgeOfMajority";

            return result;
        }

        public int CalculateAgeCorrect(DateTime birthDate, DateTime now)
        {
            int age = now.Year - birthDate.Year;
            if (now.Month < birthDate.Month || (now.Month == birthDate.Month && now.Day < birthDate.Day)) age--;
            return age;
        }

        private bool IsMinimumAge(int minimumAge, DateTime dateOfBirth)
        {
            

            return (Convert.ToInt32(DateTime.Now - dateOfBirth.Date) >= minimumAge ? true : false);
        }

        private string GeneratePassword()
        {
            const string ValidPasswordChars = "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm1234567890";
            var r = new Random();
            var generatedPassword = "";
            for (var i = 0; i < 15; i++)
            {
                var rand = r.NextDouble();
                generatedPassword += ValidPasswordChars.ToCharArray()[(int)Math.Floor(rand * ValidPasswordChars.Length)];
            }

            return generatedPassword;
        }

        public bool SendEmail(string serverAddress, string email, string language, string site)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(serverAddress);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Create a new product
                EmailUser userObject = new EmailUser() { Email = email, Language = language, SiteSource = site };

                // Create the JSON formatter.
                MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

                // Use the JSON formatter to create the content of the request body.
                var content = new ObjectContent<EmailUser>(userObject, jsonFormatter);

                Uri address = new Uri(client.BaseAddress, "/email/api/message/welcome/");
                HttpResponseMessage resp = client.PostAsync(address.ToString(), content).Result;

                return true;
            }
            catch (Exception exc)
            {
                // Error trapping code here
                return false;
            }
        }

        public bool SendEmail(string serverAddress, string email, string language, string site, string subjectLine, string body)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(serverAddress);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Create a new product
                EmailUser userObject = new EmailUser() { Email = email, Language = language, SiteSource = site, CustomSubjectLine = subjectLine, CustomBody = body };

                // Create the JSON formatter.
                MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

                // Use the JSON formatter to create the content of the request body.
                var content = new ObjectContent<EmailUser>(userObject, jsonFormatter);

                Uri address = new Uri(client.BaseAddress, "/email/api/message/custom/");
                HttpResponseMessage resp = client.PostAsync(address.ToString(), content).Result;

                return true;
            }
            catch (Exception exc)
            {
                // Error trapping code here
                return false;
            }
        }

        public bool SendEmail(string serverAddress, string email, string language, string site, string subjectLine, string body, string header, string footer)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(serverAddress);

                // Add an Accept header for JSON format.
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));

                // Create a new product
                EmailUser userObject = new EmailUser() { Email = email, Language = language, SiteSource = site, CustomSubjectLine = subjectLine, CustomBody = body, CustomHeader = header, CustomFooter = footer };

                // Create the JSON formatter.
                MediaTypeFormatter jsonFormatter = new JsonMediaTypeFormatter();

                // Use the JSON formatter to create the content of the request body.
                var content = new ObjectContent<EmailUser>(userObject, jsonFormatter);

                Uri address = new Uri(client.BaseAddress, "/email/api/message/custom/");
                HttpResponseMessage resp = client.PostAsync(address.ToString(), content).Result;

                return true;
            }
            catch (Exception exc)
            {
                // Error trapping code here
                return false;
            }
        }

        #endregion
    
    }
}
