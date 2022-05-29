using Application.Common.Interfaces;
using Application.ScheduleWithChangesList.Queries;
using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.UnitTests
{
    public class GetScheduleWithChangesListQueryHandlerTests
    {
        private const string EducOrgName1 = "EducOrgName1";
        private const string GroupNumber1 = "GroupNumber1";
        private readonly DateTime _date = new(2022, 4, 25);

        private Mock<IApplicationDbContext> GetIApplicationDbContextMock()
        {
            var educOrgs = new List<EducationalOrgEntity>()
            {
                new(EducOrgName1)
            };

            var groups = new List<GroupEntity>()
            {
                new(educOrgs[0].Id, GroupNumber1, 4)
            };

            var groupsSetMock = new Mock<DbSet<GroupEntity>>();

            groupsSetMock.As<IQueryable<GroupEntity>>()
                .Setup(m => m.Provider)
                .Returns(groups.AsQueryable().Provider);
            groupsSetMock.As<IQueryable<GroupEntity>>()
                .Setup(m => m.Expression)
                .Returns(groups.AsQueryable().Expression);
            groupsSetMock.As<IQueryable<GroupEntity>>()
                .Setup(m => m.ElementType)
                .Returns(groups.AsQueryable().ElementType);
            groupsSetMock.As<IQueryable<GroupEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(groups.AsQueryable().GetEnumerator());

            var educOrgsSetMock = new Mock<DbSet<EducationalOrgEntity>>();

            educOrgsSetMock.As<IQueryable<EducationalOrgEntity>>()
                .Setup(m => m.Provider)
                .Returns(educOrgs.AsQueryable().Provider);
            educOrgsSetMock.As<IQueryable<EducationalOrgEntity>>()
                .Setup(m => m.Expression)
                .Returns(educOrgs.AsQueryable().Expression);
            educOrgsSetMock.As<IQueryable<EducationalOrgEntity>>()
                .Setup(m => m.ElementType)
                .Returns(educOrgs.AsQueryable().ElementType);
            educOrgsSetMock.As<IQueryable<EducationalOrgEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(educOrgs.AsQueryable().GetEnumerator());

            var changesLists = new List<ChangesListEntity>()
            {
                new(educOrgs[0].Id, _date, false)
            };

            var changesListItems = new List<ChangesListItemEntity>()
            {
                new(changesLists[0].Id, groups[0].Id, new()
                {
                    Position = 1,
                    SubjectName = "lesson1",
                    TeacherInitials = "teacher1",
                    Auditorium = "aud1"
                })
            };

            changesLists[0].ListItems.AddRange(changesListItems);

            var changesListsSetMock = new Mock<DbSet<ChangesListEntity>>();

            changesListsSetMock.As<IQueryable<ChangesListEntity>>()
                .Setup(m => m.Provider)
                .Returns(changesLists.AsQueryable().Provider);
            changesListsSetMock.As<IQueryable<ChangesListEntity>>()
                .Setup(m => m.Expression)
                .Returns(changesLists.AsQueryable().Expression);
            changesListsSetMock.As<IQueryable<ChangesListEntity>>()
                .Setup(m => m.ElementType)
                .Returns(changesLists.AsQueryable().ElementType);
            changesListsSetMock.As<IQueryable<ChangesListEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(changesLists.AsQueryable().GetEnumerator());

            var changesListItemsSetMock = new Mock<DbSet<ChangesListItemEntity>>();

            changesListItemsSetMock.As<IQueryable<ChangesListItemEntity>>()
                .Setup(m => m.Provider)
                .Returns(changesListItems.AsQueryable().Provider);
            changesListItemsSetMock.As<IQueryable<ChangesListItemEntity>>()
                .Setup(m => m.Expression)
                .Returns(changesListItems.AsQueryable().Expression);
            changesListItemsSetMock.As<IQueryable<ChangesListItemEntity>>()
                .Setup(m => m.ElementType)
                .Returns(changesListItems.AsQueryable().ElementType);
            changesListItemsSetMock.As<IQueryable<ChangesListItemEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(changesListItems.AsQueryable().GetEnumerator());

            var scheduleLists = new List<ScheduleListEntity>()
            {
                new(groups[0].Id, _date.DayOfWeek)
            };

            var scheduleListItems = new List<ScheduleListItemEntity>()
            {
                new(scheduleLists[0].Id, null, new()
                {
                    Position = 1,
                    SubjectName = "lesson2",
                    TeacherInitials = "teacher2",
                    Auditorium = "aud2"
                }),
                new(scheduleLists[0].Id, true, new()
                {
                    Position = 2,
                    SubjectName = "lesson2",
                    TeacherInitials = "teacher2",
                    Auditorium = "aud2"
                }),
                new(scheduleLists[0].Id, false, new()
                {
                    Position = 2,
                    SubjectName = "lesson1",
                    TeacherInitials = "teacher1",
                    Auditorium = "aud1"
                }),
                new(scheduleLists[0].Id, null, new()
                {
                    Position = 3,
                    SubjectName = "lesson1",
                    TeacherInitials = "teacher1",
                    Auditorium = "aud1"
                })
            };

            scheduleLists[0].ListItems.AddRange(scheduleListItems);

            var scheduleListsSetMock = new Mock<DbSet<ScheduleListEntity>>();

            scheduleListsSetMock.As<IQueryable<ScheduleListEntity>>()
                .Setup(m => m.Provider)
                .Returns(scheduleLists.AsQueryable().Provider);
            scheduleListsSetMock.As<IQueryable<ScheduleListEntity>>()
                .Setup(m => m.Expression)
                .Returns(scheduleLists.AsQueryable().Expression);
            scheduleListsSetMock.As<IQueryable<ScheduleListEntity>>()
                .Setup(m => m.ElementType)
                .Returns(scheduleLists.AsQueryable().ElementType);
            scheduleListsSetMock.As<IQueryable<ScheduleListEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(scheduleLists.AsQueryable().GetEnumerator());

            var scheduleListItemsSetMock = new Mock<DbSet<ScheduleListItemEntity>>();

            scheduleListItemsSetMock.As<IQueryable<ScheduleListItemEntity>>()
                .Setup(m => m.Provider)
                .Returns(scheduleListItems.AsQueryable().Provider);
            scheduleListItemsSetMock.As<IQueryable<ScheduleListItemEntity>>()
                .Setup(m => m.Expression)
                .Returns(scheduleListItems.AsQueryable().Expression);
            scheduleListItemsSetMock.As<IQueryable<ScheduleListItemEntity>>()
                .Setup(m => m.ElementType)
                .Returns(scheduleListItems.AsQueryable().ElementType);
            scheduleListItemsSetMock.As<IQueryable<ScheduleListItemEntity>>()
                .Setup(m => m.GetEnumerator())
                .Returns(scheduleListItems.AsQueryable().GetEnumerator());

            var mock = new Mock<IApplicationDbContext>();

            mock.Setup(m => m.ChangesLists).Returns(changesListsSetMock.Object);
            mock.Setup(m => m.ChangesListItems).Returns(changesListItemsSetMock.Object);
            mock.Setup(m => m.ScheduleLists).Returns(scheduleListsSetMock.Object);
            mock.Setup(m => m.ScheduleListItems).Returns(scheduleListItemsSetMock.Object);
            mock.Setup(m => m.Groups).Returns(groupsSetMock.Object);
            mock.Setup(m => m.EducationalOrgs).Returns(educOrgsSetMock.Object);

            return mock;
        }

        [Test]
        public async Task Handle_Should_Return_Expected_Dto()
        {
            var contextMock = GetIApplicationDbContextMock();
            var mapperConfig = new MapperConfiguration(cfg =>
            {
               // cfg.AddProfile<MappingProfile>();
            });
            var mapper = mapperConfig.CreateMapper();
            var handler = new GetScheduleWithChangesListQueryHandler(contextMock.Object, mapper);

            var expected = new ScheduleWithChangesDto()
            {
                Date = _date,
                EducOrgName = EducOrgName1,
                GroupNumber = GroupNumber1,
                ScheduleItems = new List<ScheduleWithChangesListItemDto>()
                {
                    new()
                    {
                        Position = 1,
                        Discipline = "lesson1",
                        Teacher = "teacher1",
                        Auditorium = "aud1"
                    },
                    new()
                    {
                        Position = 2,
                        Discipline = "lesson1",
                        Teacher = "teacher1",
                        Auditorium = "aud1"
                    },
                    new()
                    {
                        Position = 3,
                        Discipline = "lesson1",
                        Teacher = "teacher1",
                        Auditorium = "aud1"
                    }
                }
            };

            var request = new GetScheduleWithChangesListQuery()
            {
                Date = _date,
                EducOrgName = EducOrgName1,
                GroupNumber = GroupNumber1
            };
            var result = await handler.Handle(request, CancellationToken.None);

            Assert.AreEqual(expected.Date, result.Date);
            Assert.AreEqual(expected.EducOrgName, result.EducOrgName);
            Assert.AreEqual(expected.GroupNumber, result.GroupNumber);
            Assert.AreEqual(expected.ScheduleItems.ToArray(), result.ScheduleItems.ToArray());
        }
    }
}
