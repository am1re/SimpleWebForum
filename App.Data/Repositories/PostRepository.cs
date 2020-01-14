using App.Data.Contexts;
using App.Data.Entities;
using App.Data.Interfaces.Repositories;
using App.Data.Repositories.Base;

namespace App.Data.Repositories
{
    public class PostRepository : BaseRepository<Post>, IPostRepository
    {
        public PostRepository(ForumDbContext context) : base(context) { }
    }
}
