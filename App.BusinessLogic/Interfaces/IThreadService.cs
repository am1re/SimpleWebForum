using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Resources.Thread;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BusinessLogic.Interfaces
{
    public interface IThreadService
    {
        Task<IEnumerable<ThreadResource>> GetAll();
        Task<PagedResultResource<ThreadResource>> GetAllPaged(int? page, int pageSize);

        Task<ThreadResource> GetThreadById(int id);

        Task<IEnumerable<ThreadResource>> GetThreadsByForumId(int forumId);
        Task<PagedResultResource<ThreadResource>> GetThreadsByForumIdPaged(int forumId, int? page, int pageSize);

        Task<int> GetThreadsCountByForumId(int forumId);

        Task<IEnumerable<ThreadResource>> GetThreadsByUserId(string userId);
        Task<PagedResultResource<ThreadResource>> GetThreadsByUserIdPaged(string userId, int? page, int pageSize);
        Task<int> GetThreadsCountByUserId(string userId);

        Task<IEnumerable<ThreadResource>> GetThreadsByUserName(string userName);
        Task<PagedResultResource<ThreadResource>> GetThreadsByUserNamePaged(string userName, int? page, int pageSize);
        Task<int> GetThreadsCountByUserName(string userName);

        Task<ThreadResource> CreateThread(CreateThreadResource newThread);
        Task UpdateThread(int threadToBeUpdatedId, UpdateThreadResource thread);
        Task DeleteThread(int threadToBeDeletedId);
    }
}
