using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Paging;
using App.Data.Repositories.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BusinessLogic.Interfaces
{
    public interface IForumService
    {
        Task<IEnumerable<ForumResource>> GetAll();
        Task<PagedResultResource<ForumResource>> GetAllPaged(int? page, int pageSize);
        Task<ForumResource> GetForumById(int id);

        Task<ForumResource> CreateForum(CreateForumResource forum);
        Task UpdateForum(int forumToBeUpdatedId, UpdateForumResource forum);

        Task DeleteForum(int forumToBeDeletedId);
        Task AddModeratorToForum(AddModeratorToForumResource model);
    }
}
