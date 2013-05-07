using System;
using System.Data.Entity.Infrastructure;
using MMContest.Models;

namespace MMContest.Dal
{
    public class UnitOfWork : IDisposable
    {
        private readonly CrmContext _context = new CrmContext();
        private GenericRepository<Ballot> _ballotRepository;
        private GenericRepository<Contest> _contestRepository;
        private GenericRepository<Friend> _friendRepository;
        private GenericRepository<User> _userRepository;
        private GenericRepository<MinorUser> _minorUserRepository;
        private GenericRepository<Vote> _voteRepository;
        private GenericRepository<MinorVote> _minorVoteRepository;
        private GenericRepository<UserContestRegistration> _userContestRegistrationRepository;
        private GenericRepository<MinorUserContestRegistration> _minorUserContestRegistrationRepository;
        private GenericRepository<InstantWin> _instantwinRepository;
        private GenericRepository<Pin> _pinRepository;

        public GenericRepository<Ballot> BallotRepository
        {
            get { return _ballotRepository ?? (_ballotRepository = new GenericRepository<Ballot>(_context)); }
        }

        public GenericRepository<Contest> ContestRepository
        {
            get { return _contestRepository ?? (_contestRepository = new GenericRepository<Contest>(_context)); }
        }

        public GenericRepository<Friend> FriendRepository
        {
            get { return _friendRepository ?? (_friendRepository = new GenericRepository<Friend>(_context)); }
        }

        public GenericRepository<MinorUser> MinorUserRepository
        {
            get { return _minorUserRepository ?? (_minorUserRepository = new GenericRepository<MinorUser>(_context)); }
        }

        public GenericRepository<User> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new GenericRepository<User>(_context)); }
        }
        
        public GenericRepository<UserContestRegistration> UserContestRegistrationRepository
        {
            get { return _userContestRegistrationRepository ?? (_userContestRegistrationRepository = new GenericRepository<UserContestRegistration>(_context)); }
        }

        public GenericRepository<MinorUserContestRegistration> MinorUserContestRegistrationRepository
        {
            get { return _minorUserContestRegistrationRepository ?? (_minorUserContestRegistrationRepository = new GenericRepository<MinorUserContestRegistration>(_context)); }
        }

        public GenericRepository<Vote> VoteRepository
        {
            get { return _voteRepository ?? (_voteRepository = new GenericRepository<Vote>(_context)); }
        }

        public GenericRepository<MinorVote> MinorVoteRepository
        {
            get { return _minorVoteRepository ?? (_minorVoteRepository = new GenericRepository<MinorVote>(_context)); }
        }

        public GenericRepository<InstantWin> InstantWinRepository
        {
            get { return _instantwinRepository ?? (_instantwinRepository = new GenericRepository<InstantWin>(_context)); }
        }

        public GenericRepository<Pin> PinRepository
        {
            get { return _pinRepository ?? (_pinRepository = new GenericRepository<Pin>(_context)); }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            objectContext.Detach(entity);
        }
    }
}
