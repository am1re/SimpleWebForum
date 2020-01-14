using App.Data.Contexts;
using App.Data.Interfaces;
using App.Data.Interfaces.Repositories;
using App.Data.Repositories;
using App.Data.Repositories.Identity;
using System;
using System.Threading.Tasks;

namespace App.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ForumDbContext _context;

        private PostRepository _postRepository;
        private ForumRepository _forumRepository;
        private ThreadRepository _threadRepository;
        private IUserRepository _userRepository;
        private ForumToModeratorRepository _forumToModeratorRepository;

        public UnitOfWork(ForumDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IForumRepository Forums => _forumRepository = _forumRepository ?? new ForumRepository(_context);

        public IPostRepository Posts => _postRepository = _postRepository ?? new PostRepository(_context);

        public IThreadRepository Threads => _threadRepository = _threadRepository ?? new ThreadRepository(_context);

        public IForumToModeratorRepository ForumToModerators => _forumToModeratorRepository = _forumToModeratorRepository ?? new ForumToModeratorRepository(_context);

        public IUserRepository Users => _userRepository = _userRepository ?? new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
