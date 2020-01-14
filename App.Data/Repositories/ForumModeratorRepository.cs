using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Interfaces.Repositories;
using App.Data.Repositories.Base;

namespace App.Data.Repositories
{
    public class ForumToModeratorRepository : BaseRepository<ForumToModerator>, IForumToModeratorRepository
    {
        public ForumToModeratorRepository(ForumDbContext context) : base(context) { }
    }
}
