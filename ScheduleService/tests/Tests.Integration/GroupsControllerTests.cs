using Application.Groups.Dtos;
using Application.Tests.Integration.Helpers;
using Domain.Entities;
using Newtonsoft.Json;
using ServiceAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Tests.Integration.Helpers;
using Xunit;

namespace Tests.Integration
{
    public class GroupsControllerTests : IClassFixture<TestWebApplicationFactory>
    {
        private const string BaseRoute = ApiBaseRoute.BaseRoute + "/groups";

        private readonly TestWebApplicationFactory _factory;

        public GroupsControllerTests(TestWebApplicationFactory fixture)
        {
            _factory = fixture;
        }

        [Fact]
        public async Task Get_GroupsListByEducOrgId_Should_Return_Right_Result()
        {
            using var client = _factory.CreateClientWithTestAuth(new("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var result = await client.GetAsync(BaseRoute + $"/?educOrgId={educOrgId}");
            var resultBody = await result.Content.ReadAsAsync<List<GroupDto>>();

            Assert.True(result.IsSuccessStatusCode);

            Assert.Equal("group1", resultBody[0].GroupNumber);
            Assert.Equal(1, resultBody[0].YearOfStudy);

            Assert.Equal("group2", resultBody[1].GroupNumber);
            Assert.Equal(2, resultBody[1].YearOfStudy);
        }

        [Fact]
        public async Task Get_BriefGroupsByEducOrgId_Should_Return_Right_Result()
        {
            using var client = _factory.CreateClient();

            var educOrgId = await GetEducOrgId(client);

            var result = await client
                .GetAsync(BaseRoute + $"/brief/?educOrgId={educOrgId}");
            
            var resultBody = await result.Content.ReadAsAsync<List<BriefGroupDto>>();

            Assert.True(result.IsSuccessStatusCode);

            Assert.Equal("group1", resultBody[0].GroupNumber);
            Assert.Equal("group2", resultBody[1].GroupNumber);
        }

        [Fact]
        public async Task Post_CreateGroup_Should_Response_Created()
        {
            using var client = _factory.CreateClientWithTestAuth(new("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var body = new
            {
                EducOrgId = educOrgId,
                GroupNumber = "group3",
                YearOfStudy = 3
            };

            var result = await client
                .PostAsync(BaseRoute, TestHelpers.ToJsonBody(body));
            
            var resultBody = result.Content
                .ReadAsStringAsync()
                .Result
                .Trim('\"');

            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
            Assert.True(Guid.TryParse(resultBody, out _));
        }

        [Fact]
        public async Task Get_GroupById_Should_Return_Right_Result()
        {
            using var client = _factory.CreateClientWithTestAuth(new("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var body = new
            {
                EducOrgId = educOrgId,
                GroupNumber = "group3",
                YearOfStudy = 3
            };

            var result = await client
                .PostAsync(BaseRoute, TestHelpers.ToJsonBody(body));

            var test = result
                .Content
                .Headers
                .FirstOrDefault(x => x.Key == "Location")
                .Value;
        }

        [Fact]
        public async Task Put_UpdateGroupInfo_Should_Response_NoCntent()
        {
            using var client = _factory.CreateClientWithTestAuth(new("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var groupId = client.GetAsync(BaseRoute + $"/brief/?educOrgId={educOrgId}")
                .Result
                .Content
                .ReadAsAsync<BriefGroupDto[]>()
                .Result
                .First()
                .Id;

            var body = new
            {
                GroupNumber = "group3",
                YearOfStudy = 3
            };

            var result = await client
                .PutAsync(BaseRoute + '/' + groupId, TestHelpers.ToJsonBody(body));

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        [Fact]
        public async Task Delete_Group_Should_Response_NoContent()
        {
            using var client = _factory.CreateClientWithTestAuth(new("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var groupId = client.GetAsync(BaseRoute + $"/brief/?educOrgId={educOrgId}")
                .Result
                .Content
                .ReadAsAsync<BriefGroupDto[]>()
                .Result
                .First()
                .Id;

            var result = await client.DeleteAsync(BaseRoute + '/' + groupId);

            Assert.Equal(HttpStatusCode.NoContent, result.StatusCode);
        }

        private static async Task<Guid> GetEducOrgId(HttpClient client)
        {
            var response = await client.GetAsync(ApiBaseRoute.BaseRoute + "/educational-orgs/brief");
            var educOrgsJsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<EducationalOrgEntity>>(educOrgsJsonString)
                .First()
                .Id;
        }
    }
}
