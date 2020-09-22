using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using ApplicationCore.BindingModels;
using Microsoft.EntityFrameworkCore;
using Web.IntegrationTests.Configuration;
using Xunit;

namespace Web.IntegrationTests
{
    [Collection("ControllersCollection")]
    public class CommentsControllerTests
    {
        private readonly ControllerTestsBase fixture;
        private readonly string url = "/api/comments/";

        public CommentsControllerTests(ControllerTestsBase fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Get_NoParams_ReturnsOk()
        {
            var response = await fixture.Client.GetAsync(url);
            Assert.True(response.StatusCode == HttpStatusCode.OK);
        }

        [Fact]
        public async Task AddComment_MissingParams_ReturnsBadRequest()
        {
            var response = await fixture.Client.PostAsJsonAsync(url, new CommentBm
            {
                IsPositive = true
            });
            Assert.True(response.StatusCode == HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task ChangeToPositive_GoodId_ChangesToPositive()
        {
            var commentId = fixture.Context.Comments.AsNoTracking().FirstOrDefault().Id;
            await fixture.Client.PutAsJsonAsync(url + commentId + "/toPositive", new object());
            Assert.True(fixture.Context.Comments.AsNoTracking().FirstOrDefault(c => c.Id == commentId).IsPositive);
        }

        [Fact]
        public async Task DeleteComment_GoodId_DeletesComment()
        {
            var commentId = fixture.Context.Comments.AsNoTracking().FirstOrDefault().Id;
            var commentsCount = fixture.Context.Comments.Count();
            await fixture.Client.DeleteAsync(url + commentId);
            Assert.True(fixture.Context.Comments.Count() == commentsCount - 1);
        }
    }
}
