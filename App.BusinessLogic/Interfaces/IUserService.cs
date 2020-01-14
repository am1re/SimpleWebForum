using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Paging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.BusinessLogic.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserResource>> GetAll();
        Task<PagedResultResource<UserResource>> GetAllPaged(int? page, int pageSize);

        Task<UserResource> GetUserById(string id);
        Task<UserResource> GetUserByUserName(string userName);

        Task<UserResource> CreateUser(CreateUserResource newUser);
        Task UpdateUser(string userName, UpdateUserResource user);
        Task DeleteUser(string userName);
        Task AddUserToRole(string userName, string role);

        Task<IEnumerable<UserResource>> GetUsersAsForumModerators(int forumId);
        Task<PagedResultResource<UserResource>> GetUsersAsForumModeratorsPaged(int forumId, int? page, int pageSize);

    }
}
