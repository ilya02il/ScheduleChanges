using System;
using Domain.Entities;
using Infrastructure.WriteData;

namespace Tests.Integration.Helpers
{
    internal static class DatabaseSeeds
    {
        public static void Seed(EfWriteDbContext context)
        {
            var educOrg = new EducationalOrgEntity("educOrg1");
            context.EducationalOrgs.Add(educOrg);

            var groups = new GroupEntity[]
            {
                new(educOrg.Id, "group1", 1),
                new(educOrg.Id, "group2", 2)
            };
            context.Groups.AddRange(groups);

            var lessonCalls = new LessonCallEntity[]
            {
                new(educOrg.Id, 1, DayOfWeek.Monday, new TimeSpan(8, 30, 0).Ticks, new TimeSpan(10, 5, 0).Ticks),
                new(educOrg.Id, 2, DayOfWeek.Monday, new TimeSpan(10, 15, 0).Ticks, new TimeSpan(11, 50, 0).Ticks),
                new(educOrg.Id, 3, DayOfWeek.Monday, new TimeSpan(12, 30, 0).Ticks, new TimeSpan(14, 5, 0).Ticks),
                new(educOrg.Id, 4, DayOfWeek.Monday, new TimeSpan(14, 15, 0).Ticks, new TimeSpan(15, 50, 0).Ticks)
            };
            context.LessonCalls.AddRange(lessonCalls);

            var scheduleLists = new ScheduleListEntity[]
            {
                new(groups[0].Id, DayOfWeek.Monday),
                new(groups[1].Id, DayOfWeek.Monday)
            };
            context.ScheduleLists.AddRange(scheduleLists);

            var scheduleListItems = new ScheduleListItemEntity[]
            {
                new(scheduleLists[0].Id,
                    null,
                    new()
                    {
                        Position = 1,
                        SubjectName = "subject1",
                        Auditorium = "aud1",
                        TeacherInitials = "teacher1"
                    }),
                new(scheduleLists[0].Id,
                    true,
                    new()
                    {
                        Position = 2,
                        SubjectName = "subject2",
                        Auditorium = "aud2",
                        TeacherInitials = "teacher2"
                    }),
                new(scheduleLists[0].Id,
                    false,
                    new()
                    {
                        Position = 2,
                        SubjectName = "subject3",
                        Auditorium = "aud3",
                        TeacherInitials = "teacher3"
                    }),
            };
            context.ScheduleListItems.AddRange(scheduleListItems);

            var changesLists = new ChangesListEntity[]
            {
                new(educOrg.Id, new DateTimeOffset(new DateTime(2022, 02, 22), TimeSpan.FromHours(7)), true),
                new(educOrg.Id, new DateTimeOffset(new DateTime(2022, 02, 27), TimeSpan.FromHours(7)), false)
            };
            context.ChangesLists.AddRange(changesLists);

            var changesListItems = new ChangesListItemEntity[]
            {
                new(changesLists[0].Id,
                    groups[0].Id,
                    new()
                    {
                        Position = 3,
                        SubjectName = "subject4",
                        Auditorium = "aud4",
                        TeacherInitials = "teacher4"
                    }),
                new(changesLists[1].Id,
                    groups[0].Id,
                    new()
                    {
                        Position = 3,
                        SubjectName = "subject1",
                        Auditorium = "aud1",
                        TeacherInitials = "teacher1"
                    })
            };
            context.ChangesListItems.AddRange(changesListItems);

            context.SaveChanges();
        }
    }
}
