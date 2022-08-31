using Application.CallSchedules.Dtos;
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
    public class CallScheduleListsControllerTests 
        : IClassFixture<TestWebApplicationFactory<Startup, TestStartup>>
    {
        private const string BaseRoute = ApiBaseRoute.BaseRoute + "/call-schedule-lists";

        private readonly TestWebApplicationFactory<Startup, TestStartup> _factory;

        public CallScheduleListsControllerTests(TestWebApplicationFactory<Startup, TestStartup> fixture)
        {
            _factory = fixture;
        }

        [Fact]
        public async Task Get_ByEducOrgIdAndDayOfWeek_Should_Return_Right_Result()
        {
            using var client = _factory.CreateClient();

            var educOrgId = await GetEducOrgId(client);
            var response = await client.GetAsync(BaseRoute + $"/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Monday}");

            var responseObj = await response.Content.ReadAsAsync<CallScheduleListDto>();
            var resultListItems = responseObj.ListItems.ToArray();

            ///
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

            var result1 = await client.GetAsync(BaseRoute + $"/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Sunday}");
            var result2 = await client.GetAsync(BaseRoute + $"/?educOrgId={Guid.NewGuid()}&DayOfWeek={DayOfWeek.Monday}");

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

            var result = await client.PostAsync(BaseRoute + $"/items/?educOrgId={educOrgId}",
                TestHelpers.ToJsonBody(body));

            var getResult = await (await client.GetAsync(BaseRoute + $"/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Sunday}"))
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

            var result = await client.PostAsync(BaseRoute + $"/items/?educOrgId={Guid.NewGuid()}",
                TestHelpers.ToJsonBody(body));

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
        }

        [Fact]
        public async Task Put_Update_Item_Should_Response_Success()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var educOrgId = await GetEducOrgId(client);

            var callScheduleItemId = client.GetAsync(BaseRoute + $"/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Monday}")
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

            var result = await client.PutAsync(BaseRoute + $"/items/{callScheduleItemId}",
                TestHelpers.ToJsonBody(putBody));

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

            var itemId = client.GetAsync(BaseRoute + $"/?educOrgId={educOrgId}&dayOfWeek={DayOfWeek.Monday}")
                .Result
                .Content
                .ReadAsAsync<CallScheduleListDto>()
                .Result
                .ListItems
                .First()
                .Id;

            var result = await client.DeleteAsync(BaseRoute + $"/items/{itemId}");

            Assert.True(result.IsSuccessStatusCode);
        }

        [Fact]
        public async Task Delete_Item_Should_Response_Bad_Request()
        {
            using var client = _factory
                .CreateClientWithTestAuth(new TestClaimsProvider("EducOrgManager"));

            var result = await client.DeleteAsync(BaseRoute + $"/items/{Guid.NewGuid()}");

            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
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
