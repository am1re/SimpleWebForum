using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Interfaces.Repositories;
using App.Data.Repositories.Base;

namespace App.Data.Repositories
{
    public class ForumRepository : BaseRepository<Forum>, IForumRepository
    {
        public ForumRepository(ForumDbContext context) : base(context) { }
    }
}
