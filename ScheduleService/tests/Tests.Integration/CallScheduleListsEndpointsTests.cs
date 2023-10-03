using System.Net;
using Application.CallSchedules.Dtos;
using Application.EducOrgs.Dtos;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Tests.Integration.Helpers;
using Xunit;

namespace Tests.Integration
{
    public class CallScheduleListsEndpointsTests : IClassFixture<TestWebApplicationFactory>
    {
        private static readonly WebApplicationFactoryClientOptions ClientOptions = new()
        {
            BaseAddress = new Uri("http://localhost:5000/api/v1/call-schedule-lists")
        };
        
        private const string BaseRoute = "/api/v1/call-schedule-lists";

        private readonly TestWebApplicationFactory _factory;

        public CallScheduleListsEndpointsTests(TestWebApplicationFactory fixture)
        {
            _factory = fixture;
        }

        [Fact]
        public async Task Get_ByEducOrgIdAndDayOfWeek_Should_Return_Right_Result()
        {
            using var client = _factory.CreateClient(ClientOptions);

            var educOrgId = await GetEducOrgId(client);
            var response = await client
                .GetAsync($"?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Monday:D}");

            var responseObj = await response.Content.ReadAsAsync<CallScheduleListDto>();
            var resultListItems = responseObj.ListItems.ToArray();

            Assert.True(response.IsSuccessStatusCode);

            Assert.Equal(DayOfWeek.Monday, responseObj.DayOfWeek);

            Assert.Equal(1, resultListItems[0].Position);
            Assert.Equal(new TimeSpan(8, 30, 0).Ticks, resultListItems[0].StartTime);
            Assert.Equal(new TimeSpan(10, 05, 0).Ticks, resultListItems[0].EndTime);

            Assert.Equal(2, resultListItems[1].Position);
            Assert.Equal(new TimeSpan(10, 15, 0).Ticks, resultListItems[1].StartTime);
            Assert.Equal(new TimeSpan(11, 50, 0).Ticks, resultListItems[1].EndTime);

            Assert.Equal(3, resultListItems[2].Position);
            Assert.Equal(new TimeSpan(12, 30, 0).Ticks, resultListItems[2].StartTime);
            Assert.Equal(new TimeSpan(14, 05, 0).Ticks, resultListItems[2].EndTime);

            Assert.Equal(4, resultListItems[3].Position);
            Assert.Equal(new TimeSpan(14, 15, 0).Ticks, resultListItems[3].StartTime);
            Assert.Equal(new TimeSpan(15, 50, 0).Ticks, resultListItems[3].EndTime);
        }

        [Fact]
        public async Task Get_ByEducOrgIdAndDayOfWeek_Should_Be_Success_And_Return_Empty_List()
        {
            using var client = _factory.CreateClient();

            var educOrgId = await GetEducOrgId(client);

            var result1 = await client
                .GetAsync($"{BaseRoute}/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Sunday:D}");
            
            var result2 = await client
                .GetAsync($"{BaseRoute}/?educOrgId={Guid.NewGuid()}&DayOfWeek={DayOfWeek.Monday:D}");

            Assert.True(result1.IsSuccessStatusCode && result2.IsSuccessStatusCode);
            Assert.Empty(result1.Content.ReadAsAsync<CallScheduleListDto>().Result.ListItems);
            Assert.Empty(result2.Content.ReadAsAsync<CallScheduleListDto>().Result.ListItems);
        }
        
        [Fact]
        public async Task Post_Create_Item_Should_Response_Success()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var body = new
            {
                DayOfWeek = DayOfWeek.Sunday,
                Position = 1,
                StartTime = new TimeSpan(8, 30, 0).Ticks,
                EndTime = new TimeSpan(10, 5, 0).Ticks,
            };

            var result = await client.PostAsync(
                $"{BaseRoute}/items/?educOrgId={educOrgId}",
                TestHelpers.ToJsonBody(body)
            );

            var getResult = await (await client.
                    GetAsync($"{BaseRoute}/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Sunday:D}")
                )
                .Content
                .ReadAsStringAsync();

            Assert.True(result.IsSuccessStatusCode);
            Assert.True(Guid.TryParse((await result.Content.ReadAsStringAsync()).Trim('\"'), out _));
            Assert.Contains((await result.Content.ReadAsStringAsync()).Trim('\"'), getResult);
        }

        [Fact]
        public async Task Post_Create_Item_Should_Response_Bad_Request()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var body = new
            {
                DayOfWeek = DayOfWeek.Sunday,
                Position = 1,
                StartTime = new TimeSpan(8, 30, 0).Ticks,
                EndTime = new TimeSpan(10, 5, 0).Ticks,
            };

            var result = await client.PostAsync(
                $"{BaseRoute}/items/?educOrgId={Guid.NewGuid()}",
                TestHelpers.ToJsonBody(body)
            );

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Put_Update_Item_Should_Response_Success()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var callScheduleItemId = client
                .GetAsync($"{BaseRoute}/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Monday:D}")
                .Result
                .Content
                .ReadAsAsync<CallScheduleListDto>()
                .Result
                .ListItems
                .First()
                .Id;

            var putBody = new
            {
                Position = 2,
                StartTime = new TimeSpan(10, 15, 0).Ticks,
                EndTime = new TimeSpan(11, 50, 0).Ticks
            };

            var result = await client
                .PutAsync($"{BaseRoute}/items/{callScheduleItemId}", TestHelpers.ToJsonBody(putBody));

            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Put_Update_Item_Should_Response_Bad_Request()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var putBody = new
            {
                Position = 2,
                StartTime = new TimeSpan(10, 15, 0).Ticks,
                EndTime = new TimeSpan(11, 50, 0).Ticks
            };

            var result = await client.PutAsync(BaseRoute + $"/items/{Guid.NewGuid()}",
                TestHelpers.ToJsonBody(putBody));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Delete_Item_Should_Be_Success()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var itemId = client
                .GetAsync($"{BaseRoute}/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Monday:D}")
                .Result
                .Content
                .ReadAsAsync<CallScheduleListDto>()
                .Result
                .ListItems
                .First()
                .Id;

            var result = await client.DeleteAsync($"{BaseRoute}/items/{itemId}");

            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_Item_Should_Response_Bad_Request()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var result = await client.DeleteAsync($"{BaseRoute}/items/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        private static async Task<Guid?> GetEducOrgId(HttpClient client)
        {
            var response = await client.GetAsync($"/api/v1/educational-orgs/brief");
            var educOrgsJsonString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<IEnumerable<BriefEducOrgDto>>(educOrgsJsonString)
                ?.FirstOrDefault()
                ?.Id;
        }
    }
}
