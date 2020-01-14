using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Resources.Post;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BusinessLogic.Interfaces
{
    public interface IPostService
    {
        Task<IEnumerable<PostResource>> GetAll();
        Task<PagedResultResource<PostResource>> GetAllPaged(int? page, int pageSize);

        Task<PostResource> GetPostById(int id);

        Task<IEnumerable<PostResource>> GetPostsByThreadId(int threadId);
        Task<PagedResultResource<PostResource>> GetPostsByThreadIdPaged(int threadId, int? page, int pageSize);

        Task<int> GetPostsCountByThreadId(int threadId);

        Task<IEnumerable<PostResource>> GetPostsByForumId(int forumId);
        Task<PagedResultResource<PostResource>> GetPostsByForumIdPaged(int forumId, int? page, int pageSize);

        Task<int> GetPostsCountByForumId(int forumId);

        Task<IEnumerable<PostResource>> GetPostsByUserId(string userId);
        Task<PagedResultResource<PostResource>> GetPostsByUserIdPaged(string userId, int? page, int pageSize);

        Task<int> GetPostsCountByUserId(string userId);

        Task<IEnumerable<PostResource>> GetPostsByUserName(string userName);
        Task<PagedResultResource<PostResource>> GetPostsByUserNamePaged(string userName, int? page, int pageSize);
        Task<int> GetPostsCountByUserName(string userName);

        Task<PostResource> CreatePost(CreatePostResource newPost);
        Task UpdatePost(int postToBeUpdatedId, UpdatePostResource post);
        Task DeletePost(int postToBeDeletedId);
    }
}
