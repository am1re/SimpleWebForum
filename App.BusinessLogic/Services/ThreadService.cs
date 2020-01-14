using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Resources.Thread;
using App.BusinessLogic.Validators.Thread;
using App.Data.Entities;
using App.Data.Interfaces;
using AutoMapper;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.BusinessLogic.Services
{
    public class ThreadService : IThreadService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ThreadService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ThreadResource> CreateThread(CreateThreadResource newThread)
        {
            var validator = new CreateThreadResourceValidator();
            var validationResult = await validator.ValidateAsync(newThread);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(newThread.StartedBy.UserName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User provided in StartedBy property does not exist");

            var threadData = _mapper.Map<CreateThreadResource, Thread>(newThread);
            await _unitOfWork.Threads.AddAsync(threadData);
            await _unitOfWork.CommitAsync();

            var threadModel = _mapper.Map<Thread, ThreadResource>(threadData);
            return threadModel;
        }

        public async Task DeleteThread(int threadToBeDeletedId)
        {
            var thread = await _unitOfWork.Threads.GetByIdAsync(threadToBeDeletedId);
            if (thread == null) throw ExceptionBuilder.Create("Thread with provided Id does not exist");

            _unitOfWork.Threads.Remove(thread);

            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ThreadResource>> GetAll()
        {
            var threads = await _unitOfWork.Threads.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Thread>, IEnumerable<ThreadResource>>(threads);

            return result;
        }

        public async Task<PagedResultResource<ThreadResource>> GetAllPaged(int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Threads.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<ThreadResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<ThreadResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<ThreadResource> GetThreadById(int id)
        {
            var thread = await _unitOfWork.Threads.GetByIdAsync(id);
            if (thread == null) throw ExceptionBuilder.Create("Thread with provided Id does not exist");

            var result = _mapper.Map<Thread, ThreadResource>(thread);
            return result;
        }

        public async Task<IEnumerable<ThreadResource>> GetThreadsByForumId(int forumId)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var threads = forum.Threads;
            var result = _mapper.Map<IEnumerable<Thread>, IEnumerable<ThreadResource>>(threads);
            return result;
        }

        public async Task<PagedResultResource<ThreadResource>> GetThreadsByForumIdPaged(int forumId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            // todo: linq not executing even with tolist()
            //var threads = forum.Threads.Skip(pageSize * (int)page).Take(pageSize);
            var threads = forum.Threads;
            var result = new PagedResultResource<ThreadResource>
            {
                Data = _mapper.Map<IEnumerable<ThreadResource>>(threads),
                //CurrentPage = (int)page,
                //PageSize = pageSize,
                CurrentPage = 1,
                PageSize = threads.Count(),
                RowCount = threads.Count()
            };
            return result;
        }

        public async Task<IEnumerable<ThreadResource>> GetThreadsByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var threads = user.Threads;
            var result = _mapper.Map<IEnumerable<Thread>, IEnumerable<ThreadResource>>(threads);
            return result;
        }

        public async Task<PagedResultResource<ThreadResource>> GetThreadsByUserIdPaged(string userId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var posts = user.Threads.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<ThreadResource>
            {
                Data = _mapper.Map<IEnumerable<ThreadResource>>(posts),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = posts.Count()
            };
            return result;
        }

        public async Task<IEnumerable<ThreadResource>> GetThreadsByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var threads = user.Threads;
            var result = _mapper.Map<IEnumerable<Thread>, IEnumerable<ThreadResource>>(threads);

            return result;
        }

        public async Task<PagedResultResource<ThreadResource>> GetThreadsByUserNamePaged(string userName, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var posts = user.Threads.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<ThreadResource>
            {
                Data = _mapper.Map<IEnumerable<ThreadResource>>(posts),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = posts.Count()
            };
            return result;
        }

        public async Task<int> GetThreadsCountByForumId(int forumId)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var count = forum.Threads.Count;
            return count;
        }

        public async Task<int> GetThreadsCountByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var count = user.Threads.Count;
            return count;
        }

        public async Task<int> GetThreadsCountByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var count = user.Threads.Count;
            return count;
        }

        public async Task UpdateThread(int threadToBeUpdatedId, UpdateThreadResource thread)
        {
            var validator = new UpdateThreadResourceValidator();
            var validationResult = await validator.ValidateAsync(thread);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualThread = await _unitOfWork.Threads.GetByIdAsync(threadToBeUpdatedId);
            if (actualThread == null) throw ExceptionBuilder.Create("Thread with provided Id does not exist");

            if (thread.ParentForum != null && thread.ParentForum.Id > 0)
            {
                var forum = await _unitOfWork.Forums.GetByIdAsync(thread.ParentForum.Id);
                actualThread.Forum = (forum != null) ? forum : actualThread.Forum;
            }
            actualThread.Subject = (string.IsNullOrEmpty(thread.Subject)) ? thread.Subject : actualThread.Subject;

            await _unitOfWork.CommitAsync();
        }
    }
}
