using App.BusinessLogic.Identity;
using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Identity.User;
using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Validators.Identity.User;
using App.Data.Entities;
using App.Data.Entities.Identity;
using App.Data.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace App.BusinessLogic.Services
{
    public class UserSerivce : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ForumUserManager _userManager; // for user creation, roles ..

        public UserSerivce(IMapper mapper, IUnitOfWork unitOfWork, ForumUserManager userManager)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task AddUserToRole(string userName, string role)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            IdentityResult result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded) throw ExceptionBuilder.Create("Eror when adding roles to user - " + result.ToString(), HttpStatusCode.BadRequest);
        }

        public async Task<UserResource> CreateUser(CreateUserResource newUser)
        {
            var validator = new CreateUserResourceValidator();
            var validationResult = await validator.ValidateAsync(newUser);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var user = _mapper.Map<CreateUserResource, User>(newUser);
            IdentityResult result = await _userManager.CreateAsync(user, newUser.Password);
            if (!result.Succeeded) throw ExceptionBuilder.Create("Eror when creating user - " + result.ToString(), HttpStatusCode.BadRequest);

            var userModel = _mapper.Map<UserResource>(user);
            return userModel;
        }

        public async Task DeleteUser(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            _unitOfWork.Users.Remove(user);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<UserResource>> GetAll()
        {
            var users = await _unitOfWork.Users.GetAllAsync();
            var result = _mapper.Map<IEnumerable<User>, IEnumerable<UserResource>>(users);

            return result;
        }

        public async Task<PagedResultResource<UserResource>> GetAllPaged(int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Users.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<UserResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<UserResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<UserResource> GetUserById(string id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var result = _mapper.Map<User, UserResource>(user);
            return result;
        }

        public async Task<UserResource> GetUserByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var result = _mapper.Map<User, UserResource>(user);
            return result;
        }

        public async Task<IEnumerable<UserResource>> GetUsersAsForumModerators(int forumId)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var forumMods = forum.Moderators;
            var result = _mapper.Map<IEnumerable<ForumToModerator>, IEnumerable<UserResource>>(forumMods);
            return result;
        }

        public async Task<PagedResultResource<UserResource>> GetUsersAsForumModeratorsPaged(int forumId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var users = forum.Moderators.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<UserResource>
            {
                Data = _mapper.Map<IEnumerable<UserResource>>(users),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = users.Count()
            };
            return result;
        }

        public async Task UpdateUser(string userName, UpdateUserResource user)
        {
            var validator = new UpdateUserResourceValidator();
            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualUser = await _unitOfWork.Users.SingleOrDefaultAsync(u => u.UserName == userName);
            if (actualUser == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            actualUser.FirstName = (string.IsNullOrEmpty(user.FirstName)) ? user.FirstName : actualUser.FirstName;
            actualUser.LastName = (string.IsNullOrEmpty(user.LastName)) ? user.LastName : actualUser.LastName;
            actualUser.Email = (string.IsNullOrEmpty(user.Email)) ? user.Email : actualUser.Email;

            await _unitOfWork.CommitAsync();
        }
    }
}
