using App.BusinessLogic.Interfaces;
using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Paging;
using App.BusinessLogic.Validators.Forum;
using App.Data.Entities;
using App.Data.Interfaces;
using App.Data.Repositories.Paging;
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace App.BusinessLogic.Services
{
    public class ForumService : IForumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ForumService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task AddModeratorToForum(AddModeratorToForumResource resource)
        {
            // todo: add validator, add api endpoint for method
            //var validator = new AddModeratorToForumResourceValidator();
            //var validationResult = await validator.ValidateAsync(resource);
            //if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var data = _mapper.Map<AddModeratorToForumResource, ForumToModerator>(resource);
            await _unitOfWork.ForumToModerators.AddAsync(data);
            await _unitOfWork.CommitAsync();
        }

        public async Task<ForumResource> CreateForum(CreateForumResource newForum)
        {
            var validator = new CreateForumResourceValidator();
            var validationResult = await validator.ValidateAsync(newForum);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var forumData = _mapper.Map<CreateForumResource, Forum>(newForum);

            await _unitOfWork.Forums.AddAsync(forumData);
            await _unitOfWork.CommitAsync();

            var forumModel = _mapper.Map<Forum, ForumResource>(forumData);

            return forumModel;
        }

        public async Task DeleteForum(int forumToBeDeletedId)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(forumToBeDeletedId);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            _unitOfWork.Forums.Remove(forum);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ForumResource>> GetAll()
        {
            var forums = await _unitOfWork.Forums.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Forum>, IEnumerable<ForumResource>>(forums);

            return result;
        }

        public async Task<PagedResultResource<ForumResource>> GetAllPaged(int? page, int pageSize = 5)
        {
            if (!page.HasValue) throw new ArgumentNullException("Please, specify the page");

            var pagedData = await _unitOfWork.Forums.GetAllPagedAsync((int)page, pageSize);
            var result = new PagedResultResource<ForumResource>(pagedData)
            {
                Data = _mapper.Map<IEnumerable<ForumResource>>(pagedData.Results)
            };
            return result;
        }

        public async Task<ForumResource> GetForumById(int id)
        {
            var forum = await _unitOfWork.Forums.GetByIdAsync(id);
            if (forum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            var result = _mapper.Map<Forum, ForumResource>(forum);
            return result;
        }

        public async Task UpdateForum(int forumToBeUpdatedId, UpdateForumResource forum)
        {
            var validator = new UpdateForumResourceValidator();
            var validationResult = await validator.ValidateAsync(forum);
            if (!validationResult.IsValid) throw new ValidationException(validationResult.Errors);

            var actualForum = await _unitOfWork.Forums.GetByIdAsync(forumToBeUpdatedId);
            if (actualForum == null) throw ExceptionBuilder.Create("Forum with provided Id does not exist");

            actualForum.Name = (!string.IsNullOrEmpty(forum.Name)) ? forum.Name : actualForum.Name;
            actualForum.Description = (!string.IsNullOrEmpty(forum.Description)) ? forum.Description : actualForum.Description;
            actualForum.IsActive = (forum.IsActive.HasValue) ? (bool)forum.IsActive : actualForum.IsActive;

            await _unitOfWork.CommitAsync();
        }
    }
}
