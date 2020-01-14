using App.BusinessLogic.Mapping;
using App.BusinessLogic.Resources.Forum;
using App.BusinessLogic.Resources.Post;
using App.Data.Entities;
using AutoFixture;
using AutoMapper;
using Newtonsoft.Json;
using System.Linq;
using Xunit;

namespace App.BusinessLogic.Tests.Mapping
{
    public class ValueResolversTests
    {
        private readonly IFixture _fixture;
        private readonly IMapper _mapper;

        private readonly Forum forumData;
        private readonly ForumResource forumResource;

        public ValueResolversTests()
        {
            var mapProfile = new MappingProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(mapProfile));
            _mapper = new Mapper(configuration);

            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            forumData = _fixture.Create<Forum>();
            forumResource = _mapper.Map<Forum, ForumResource>(forumData);
        }

        [Fact]
        public void ForumToForumModel_ForumPropertiesTransfered()
        {
            var f = forumData;
            var fResource = forumResource;

            Assert.Equal(f.CreatedAt, fResource.CreatedAt);
            Assert.Equal(f.Description, fResource.Description);
            Assert.Equal(f.ForumId, fResource.Id);
            Assert.Equal(f.IsActive, fResource.IsActive);
            Assert.Equal(f.Name, fResource.Name);

            int threadsCount = f.Threads.Count;
            Assert.Equal(threadsCount, fResource.ThreadsCount);
        }

        [Fact]
        public void ForumToForumModel_LastPostForumResolver_Resolved()
        {
            var f = forumData;
            var fResource = forumResource;

            var lastPost = new Post();
            foreach (var thread in f.Threads)
            {
                lastPost = thread.Posts.OrderByDescending(t => t.CreatedAt).First();
            }
            var lastPostModel = _mapper.Map<Post, PostResource>(lastPost);

            var obj1 = JsonConvert.SerializeObject(lastPostModel);
            var obj2 = JsonConvert.SerializeObject(fResource.LastPost);
            Assert.Equal(obj1, obj2);
        }

        [Fact]
        public void ForumToForumModel_PostsCountForumResolver_Resolved()
        {
            var f = forumData;
            var fResource = forumResource;

            int postsCount = 0;
            foreach (var thread in f.Threads)
                postsCount += thread.Posts.Count;

            Assert.Equal(postsCount, fResource.PostsCount);
        }
    }
}
