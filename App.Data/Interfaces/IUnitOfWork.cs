using App.Data.Interfaces.Repositories;
using System;
using System.Threading.Tasks;

namespace App.Data.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IForumRepository Forums { get; }
        IPostRepository Posts { get; }
        IThreadRepository Threads { get; }
        IForumToModeratorRepository ForumToModerators { get; }
        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}
