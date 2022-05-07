using Application.ChatEntities.Commands.HandleMessageCommand;
using Application.Common.Interfaces;
using Domain;
using Domain.Dtos;
using Domain.Entities;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.UnitTests.Commands
{
    public class HandleMessageCommandHandlerTests
    {
        private readonly Dictionary<string, string> _responses = BotResponsesStrings.GetResponsesStrings();

        private const string DefaultEducOrgName = "Default education organization name.";
        private const string DefaultGroupNumber = "GROUP";
        private const string DefaultUsername = "DefaultUsername";

        private void SetupDbContextMock(Mock<IApplicationDbContext> mock)
        {
            var chatBotCollection = new List<ChatBotEntity>()
            {
                new(1, 1, new()
                {
                    Username = DefaultUsername,
                    EducationalInfo = new()
                    {
                        EducOrgName = DefaultEducOrgName,
                        GroupNumber = DefaultGroupNumber
                    }
                })
            }.AsQueryable();
            var mockChatBotSet = new Mock<DbSet<ChatBotEntity>>();

            mockChatBotSet.As<IQueryable<ChatBotEntity>>()
                .Setup(cb => cb.Provider)
                .Returns(chatBotCollection.Provider);
            mockChatBotSet.As<IQueryable<ChatBotEntity>>()
                .Setup(cb => cb.Expression)
                .Returns(chatBotCollection.Expression);
            mockChatBotSet.As<IQueryable<ChatBotEntity>>()
                .Setup(cb => cb.ElementType)
                .Returns(chatBotCollection.ElementType);
            mockChatBotSet.As<IQueryable<ChatBotEntity>>()
                .Setup(cb => cb.GetEnumerator())
                .Returns(chatBotCollection.GetEnumerator());

            mock.Setup(m => m.Chats)
                .Returns(mockChatBotSet.Object);

            mock.Setup(m => m.SaveChangesAsync(CancellationToken.None))
                .Returns(Task.FromResult(1));
        }

        private void SetupDistributedCacheMock(Mock<IDistributedCacheWrapper> mock)
        {
            //mock.Setup(dc => dc.GetStringAsync<DatedScheduleEntity>(It.IsAny<string>()))
                //.Returns(Task.FromResult(new DatedScheduleEntity()));

           // mock.Setup(dc => dc.SetStringAsync(
                //It.IsAny<DatedScheduleEntity>(),
                //It.IsAny<string>(),
                //null
                //)
            //);
        }

        private void SetupGrpcClientMock(Mock<IGrpcScheduleClientWrapper> mock)
        {
            //mock.Setup(gc => gc.CheckEducOrgName(It.IsAny<string>()))
            //    .Returns(Task.FromResult(true));

            //mock.Setup(gc => gc.CheckGroupNumber(It.IsAny<EducationalInfo>()))
            //    .Returns(Task.FromResult(true));

            mock
                .Setup(gc => gc.GetDatedSchedule(
                    It.IsAny<EducationalInfo>(),
                    It.IsAny<DateTimeOffset>()
                    )
                )
                .Returns(Task.FromResult(new DatedScheduleDto()));
        }

        [Test]
        public async Task Handle_Should_Return_ChatBotResponse_With_Message_StartMsg()
        {
            var mockDbContext = new Mock<IApplicationDbContext>();
            var mockDistributedCache = new Mock<IDistributedCacheWrapper>();
            var mockGrpcClient = new Mock<IGrpcScheduleClientWrapper>();

            SetupDbContextMock(mockDbContext);
            SetupDistributedCacheMock(mockDistributedCache);
            SetupGrpcClientMock(mockGrpcClient);

            var handler = new HanldeMessageCommandHandler(mockDbContext.Object,
                mockDistributedCache.Object,
                mockGrpcClient.Object);

            var command = new HandleMessageCommand()
            {
                Text = "/start"
            };

            var result = await handler.Handle(command, CancellationToken.None);

            Assert.AreEqual(_responses["startMsg"], result.Messages[0]);
        }
    }
}
