using Application.EducOrgs.Dtos;
using Application.Tests.Integration.Helpers;
using ServiceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Tests.Integration.Helpers;
using Xunit;

namespace Tests.Integration
{
    public class EducOrgsControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private const string BaseRoute = ApiBaseRoute.BaseRoute + "/educational-orgs";
        private readonly TestWebApplicationFactory _factory;

        public EducOrgsControllerTests(TestWebApplicationFactory fixture)
        {
            _factory = fixture;
        }

        [Fact]
        public async Task Get_BriefEducOrgs_Should_Return_Right_Result()
        {
            using var client = _factory.CreateClient();

            var result = await client.GetAsync(BaseRoute + "/brief");
            var resultBody = await result.Content.ReadAsAsync<List<BriefEducOrgDto>>();

            Assert.True(result.IsSuccessStatusCode);
            Assert.Equal("educOrg1", resultBody.First().Name);
        }

        [Fact]
        public async Task Post_CreateEducOrg_Response_Success()
        {
            using var client = _factory.CreateClientWithTestAuth(new("Admin"));

            var body = new
            {
                Name = "educOrg2"
            };

            var result = await client
                .PostAsync(BaseRoute, TestHelpers.ToJsonBody(body));

            var resultContent = result.Content.ReadAsStringAsync().Result.Trim('\"');

            Assert.True(result.IsSuccessStatusCode);
            Assert.True(Guid.TryParse(resultContent, out _));
        }

        [Fact]
        public async Task Post_CreateEducOrg_Response_Bad_Request()
        {
            using var client = _factory.CreateClientWithTestAuth(new("Admin"));

            var body = new
            {
                Name = "educOrg1"
            };

            var result = await client
                .PostAsync(BaseRoute, TestHelpers.ToJsonBody(body));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Put_UpdateEducOrg_Should_Response_NoContent()
        {
            using var client = _factory.CreateClientWithTestAuth(new("Admin"));

            var educOrgId = client.GetAsync(BaseRoute + "/brief")
                .Result
                .Content
                .ReadAsAsync<List<BriefEducOrgDto>>()
                .Result
                .First()
                .Id;

            var body = new
            {
                Name = "educOrg2"
            };

            var result = await client.PutAsync(BaseRoute + '/' + educOrgId,
                TestHelpers.ToJsonBody(body));

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task Put_UpdateEducOrg_Should_Response_BadRequest()
        {
            using var client = _factory.CreateClientWithTestAuth(new("Admin"));

            var body = new
            {
                Name = "educOrg3"
            };

            var result = await client.PutAsync(BaseRoute + '/' + Guid.NewGuid(),
                TestHelpers.ToJsonBody(body));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Delete_EducOrg_Should_Response_NoContent()
        {
            using var client = _factory.CreateClientWithTestAuth(new("Admin"));

            var educOrgId = client.GetAsync(BaseRoute + "/brief")
                .Result
                .Content
                .ReadAsAsync<List<BriefEducOrgDto>>()
                .Result
                .First()
                .Id;

            var result = await client.DeleteAsync(BaseRoute + '/' + educOrgId);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task Delete_EducOrg_Should_Response_Bad_Request()
        {
            using var client = _factory.CreateClientWithTestAuth(new("Admin"));

            var result = await client.DeleteAsync(BaseRoute + '/' + Guid.NewGuid());

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }
    }
}
