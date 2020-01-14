using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Interfaces.Repositories;
using App.Data.Repositories.Base;

namespace App.Data.Repositories
{
    public class ThreadRepository : BaseRepository<Thread>, IThreadRepository
    {
        public ThreadRepository(ForumDbContext context) : base(context) { }
    }
}
