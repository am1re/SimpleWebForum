using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Resources.Post;
using App.BusinessLogic.Validators.Post;
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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PostService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PostResource> CreatePost(CreatePostResource newPost)
        {
            var validator = new CreatePostResourceValidator();
            var validationResult = await validator.ValidateAsync(newPost);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var postData = _mapper.Map<CreatePostResource, Post>(newPost);

            await _unitOfWork.Posts.AddAsync(postData);
            await _unitOfWork.CommitAsync();

            var postModel = _mapper.Map<Post, PostResource>(postData);
            return postModel;
        }

        public async Task DeletePost(int postToBeDeletedId)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(postToBeDeletedId);
            if (post == null) throw ExceptionBuilder.Create("Post with provided Id does not exist");

            _unitOfWork.Posts.Remove(post);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<PostResource>> GetAll()
        {
            var posts = await _unitOfWork.Posts.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);

            return result;
        }

        public async Task<PagedResultResource<PostResource>> GetAllPaged(int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Posts.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<PostResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<PostResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<PostResource> GetPostById(int id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null) throw ExceptionBuilder.Create("Post with provided Id does not exist");

            var result = _mapper.Map<Post, PostResource>(post);
            return result;
        }

        public async Task<IEnumerable<PostResource>> GetPostsByForumId(int forumId)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var posts = forum.Threads.SelectMany(t => t.Posts);
            var result = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);
            return result;
        }

        public async Task<PagedResultResource<PostResource>> GetPostsByForumIdPaged(int forumId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");
            
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var posts = forum.Threads.SelectMany(t => t.Posts).Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<PostResource>
            {
                Data = _mapper.Map<IEnumerable<PostResource>>(posts),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = posts.Count()
            };
            return result;
        }

        public async Task<IEnumerable<PostResource>> GetPostsByThreadId(int threadId)
        {
            var thread = await _unitOfWork.Threads.GetByIdAsync(threadId);
            if (thread == null) throw ExceptionBuilder.Create("Thread with provided Id does not exist");

            var posts = thread.Posts;
            var result = _mapper.Map<IEnumerable<PostResource>>(posts);
            return result;
        }

        public async Task<PagedResultResource<PostResource>> GetPostsByThreadIdPaged(int threadId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var thread = await _unitOfWork.Threads.GetByIdAsync(threadId);
            if (thread == null) throw ExceptionBuilder.Create("Thread with provided Id does not exist");

            var posts = thread.Posts.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<PostResource>
            {
                Data = _mapper.Map<IEnumerable<PostResource>>(posts),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = posts.Count()
            };
            return result;
        }

        public async Task<IEnumerable<PostResource>> GetPostsByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var posts = user.Posts;
            var result = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);
            return result;
        }

        public async Task<PagedResultResource<PostResource>> GetPostsByUserIdPaged(string userId, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            var posts = user.Posts.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<PostResource>
            {
                Data = _mapper.Map<IEnumerable<PostResource>>(posts),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = posts.Count()
            };
            return result;
        }

        public async Task<IEnumerable<PostResource>> GetPostsByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("Post with provided UserName does not exist");

            var posts = user.Posts;
            var result = _mapper.Map<IEnumerable<Post>, IEnumerable<PostResource>>(posts);
            return result;
        }

        public async Task<PagedResultResource<PostResource>> GetPostsByUserNamePaged(string userName, int? page, int pageSize)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            var posts = user.Posts.Skip(pageSize * (int)page).Take(pageSize);
            var result = new PagedResultResource<PostResource>
            {
                Data = _mapper.Map<IEnumerable<PostResource>>(posts),
                CurrentPage = (int)page,
                PageSize = pageSize,
                RowCount = posts.Count()
            };
            return result;
        }

        public async Task<int> GetPostsCountByForumId(int forumId)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            int count = forum.Threads.SelectMany(t => t.Posts).Count();
            return count;
        }

        public async Task<int> GetPostsCountByThreadId(int threadId)
        {
            var thread = await _unitOfWork.Threads.GetByIdAsync(threadId);
            if (thread == null) throw ExceptionBuilder.Create("Thread with provided Id does not exist");

            var count = thread.Posts.Count;
            return count;
        }

        public async Task<int> GetPostsCountByUserId(string userId)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(userId);
            if (user == null) throw ExceptionBuilder.Create("User with provided Id does not exist");

            int count = user.Posts.Count;
            return count;
        }

        public async Task<int> GetPostsCountByUserName(string userName)
        {
            var user = await _unitOfWork.Users.SingleOrDefaultAsync(u => string.Equals(userName, u.UserName));
            if (user == null) throw ExceptionBuilder.Create("User with provided UserName does not exist");

            int count = user.Posts.Count;
            return count;
        }

        public async Task UpdatePost(int postToBeUpdatedId, UpdatePostResource post)
        {
            var validator = new UpdatePostResourceValidator();
            var validationResult = await validator.ValidateAsync(post);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualPost = await _unitOfWork.Posts.GetByIdAsync(postToBeUpdatedId);
            if (actualPost == null) throw ExceptionBuilder.Create("Post with provided Id does not exist");

            actualPost.Content = (!string.IsNullOrEmpty(post.Content)) ? post.Content : actualPost.Content;
            await _unitOfWork.CommitAsync();
        }
    }
}
