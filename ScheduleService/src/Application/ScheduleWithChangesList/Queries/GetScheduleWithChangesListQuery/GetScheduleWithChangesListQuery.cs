using Application.Common.Interfaces;
using AutoMapper;
using Dapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery
{
    public class GetScheduleWithChangesListQuery : IRequest<ScheduleWithChangesDto>
    {
        public DateTimeOffset Date { get; init; }
        public string EducOrgName { get; init; }
        public string GroupNumber { get; init; }
    }

    public class GetScheduleWithChangesListQueryHandler : IRequestHandler<GetScheduleWithChangesListQuery, ScheduleWithChangesDto>
    {
        private readonly IReadDapperContext _context;
        private readonly IMapper _mapper;
         
        public GetScheduleWithChangesListQueryHandler(IReadDapperContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ScheduleWithChangesDto> Handle(GetScheduleWithChangesListQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            //взять из базы изменения с нужными названием образовательной организации
            //и номером группы

            var query = $@"
                select top(1)
	                EducationalOrgs.Id
                from EducationalOrgs
                where
	                Name = @{nameof(request.EducOrgName)}
                ";

            var command = new CommandDefinition(query,
                    new { request.EducOrgName },
                    cancellationToken: cancellationToken
                );

            var educOrgId = await connection.ExecuteScalarAsync<Guid>(command);

            query = $@"
                select
	                ChangesLists.Id as {nameof(ChangesListEntity.Id)},
	                Date as {nameof(ChangesListEntity.Date)},
	                IsOddWeek as {nameof(ChangesListEntity.IsOddWeek)},
	                EducationalOrgId as {nameof(ChangesListEntity.EducationalOrgId)}
                from ChangesLists
                where
	                {nameof(ChangesListEntity.Date)} = @{nameof(request.Date)} and
	                {nameof(ChangesListEntity.EducationalOrgId)} = @{nameof(educOrgId)}
                ";

            command = new CommandDefinition(query,
                    new { request.Date, educOrgId },
                    cancellationToken: cancellationToken
                );

            var changesList = await connection.QueryFirstOrDefaultAsync<ChangesListEntity>(command);

            query = $@"
                select top(1)
	                Groups.Id
                from Groups
                where
	                EducationalOrgId = @{nameof(educOrgId)} and
	                GroupNumber = @{nameof(request.GroupNumber)}
                ";

            command = new CommandDefinition(query,
                    new { educOrgId, request.GroupNumber },
                    cancellationToken: cancellationToken
                );

            var groupId = await connection.ExecuteScalarAsync<Guid>(command);

            query = $@"
                select
                    ChangesListItems.Id as ChangesListItemId,
	                ListItems.ItemInfo_Position as {nameof(ListItemEntity.ItemInfo.Position)},
	                ListItems.ItemInfo_SubjectName as {nameof(ListItemEntity.ItemInfo.SubjectName)},
	                ListItems.ItemInfo_TeacherInitials as {nameof(ListItemEntity.ItemInfo.TeacherInitials)},
	                ListItems.ItemInfo_Auditorium as {nameof(ListItemEntity.ItemInfo.Auditorium)},
	                GroupId as {nameof(ChangesListItemEntity.GroupId)},
	                ChangesListId as {nameof(ChangesListItemEntity.ChangesListId)}
                from ChangesListItems
                    left join ListItems on ChangesListItems.Id = ListItems.Id
                where
                    {nameof(ChangesListItemEntity.ChangesListId)} = @{nameof(changesList.Id)} and
                    {nameof(ChangesListItemEntity.GroupId)} = @{nameof(groupId)}
                order by
                    {nameof(ListItemEntity.ItemInfo.Position)}
                ";

            command = new CommandDefinition(query,
                    new { changesList.Id, groupId },
                    cancellationToken: cancellationToken
                );

            changesList.AppendItems(await connection.QueryAsync<ChangesListItemEntity>(command));

            //взять из базы расписание с нужными названием образовательной организации
            //и номером группы

            query = $@"
                select
                    SheduleLists.Id as {nameof(ScheduleListEntity.Id)},
                    DayOfWeek as {nameof(ScheduleListEntity.DayOfWeek)},
                    GroupId as {nameof(ScheduleListEntity.GroupId)}
                from ScheduleLists
                where
                    {nameof(ScheduleListEntity.DayOfWeek)} = {nameof(request.Date.DayOfWeek)} and
                    {nameof(ScheduleListEntity.GroupId)} = {nameof(groupId)}
                ";

            command = new CommandDefinition(query,
                    new { request.Date.DayOfWeek, groupId },
                    cancellationToken: cancellationToken
                );

            var scheduleList = await connection.QueryFirstOrDefaultAsync<ScheduleListEntity>(command);

            query = $@"
                select
                    ScheduleListItems.Id as {nameof(ScheduleListItemEntity.Id)},
                    IsOddWeek as {nameof(ScheduleListItemEntity.IsOddWeek)},
                    ListItems.ItemInfo_Position as {nameof(ListItemEntity.ItemInfo.Position)},
	                ListItems.ItemInfo_SubjectName as {nameof(ListItemEntity.ItemInfo.SubjectName)},
	                ListItems.ItemInfo_TeacherInitials as {nameof(ListItemEntity.ItemInfo.TeacherInitials)},
	                ListItems.ItemInfo_Auditorium as {nameof(ListItemEntity.ItemInfo.Auditorium)},
                    ScheduleListId as {nameof(ScheduleListItemEntity.ScheduleListId)}
                from ScheduleListItems
                    left join ListItems on ScheduleListItems.Id = ListItems.Id
                where
                    {nameof(ScheduleListItemEntity.ScheduleListId)} = @{nameof(scheduleList.Id)} and
                    (
                        {nameof(ScheduleListItemEntity.IsOddWeek)} = @{nameof(changesList.IsOddWeek)} or
                        {nameof(ScheduleListItemEntity.IsOddWeek)} = NULL
                    )
                order by
                    {nameof(ListItemEntity.ItemInfo.Position)}
                ";

            command = new CommandDefinition(query,
                    new { scheduleList.Id, changesList.IsOddWeek },
                    cancellationToken: cancellationToken
                );

            scheduleList.AppendItems(await connection.QueryAsync<ScheduleListItemEntity>(command));

            if (scheduleList.ListItems.Count == 0)
                return new ScheduleWithChangesDto()
                {
                    EducOrgName = request.EducOrgName,
                    GroupNumber = request.GroupNumber,
                    Date = request.Date,
                    ScheduleItems = new List<ScheduleWithChangesListItemDto>()
                };

            query = $@"
                select
                    LessonCalls.Id as {nameof(LessonCallEntity.Id)},
                    Position as {nameof(LessonCallEntity.Position)},
                    StartTime as {nameof(LessonCallEntity.StartTime)},
                    EndTime as {nameof(LessonCallEntity.EndTime)}
                from LessonCalls
                    left join DatedLessonCalls on LessonCalls.Id = DatedLessonCalls.Id
                where
                    {nameof(LessonCallEntity.EducationalOrgId)} = @{nameof(educOrgId)} and
                    (
                        {nameof(LessonCallEntity.DayOfWeek)} = @{nameof(request.Date.DayOfWeek)} or
                        {nameof(DatedLessonCallEntity.Date)} = @{nameof(request.Date)}
                    )
                ";

            command = new CommandDefinition(query,
                    new { educOrgId, request.Date, request.Date.DayOfWeek },
                    cancellationToken: cancellationToken
                );

            var lessonCalls = await connection.QueryAsync<LessonCallEntity>(command);

            ScheduleWithChangesListItemDto buffer;
            LessonCallEntity lessonCallsBuffer;

            var resultItemsList = new List<ScheduleWithChangesListItemDto>();

            for (int i = 0; i < scheduleList.ListItems.Count; i++)
            {
                buffer = null;

                if (changesList is not null)
                {
                    for (int j = 0; j < changesList.ListItems.Count; j++)
                    {
                        if (!(changesList.ListItems[j].ItemInfo.Position <= scheduleList.ListItems[i].ItemInfo.Position))
                            break;

                        if (changesList.ListItems[j].ItemInfo.Position < scheduleList.ListItems[i].ItemInfo.Position)
                            i--;

                        buffer = _mapper.Map<ScheduleWithChangesListItemDto>(changesList.ListItems[j]);
                        lessonCallsBuffer = lessonCalls.Single(lc => lc.Position == changesList.ListItems[j].ItemInfo.Position);

                        buffer.StartTime = lessonCallsBuffer.StartTime;
                        buffer.EndTime = lessonCallsBuffer.EndTime;

                        changesList.ListItems.Remove(changesList.ListItems[j]);
                    }
                }

                if (buffer is null)
                {
                    buffer = _mapper.Map<ScheduleWithChangesListItemDto>(scheduleList.ListItems[i]);
                    lessonCallsBuffer = lessonCalls.Single(lc => lc.Position == scheduleList.ListItems[i].ItemInfo.Position);

                    buffer.StartTime = lessonCallsBuffer.StartTime;
                    buffer.EndTime = lessonCallsBuffer.EndTime;
                }

                resultItemsList.Add(buffer);
            }

            return new ScheduleWithChangesDto()
            {
                Date = request.Date,
                EducOrgName = request.EducOrgName,
                GroupNumber = request.GroupNumber,
                ScheduleItems = resultItemsList
            };
        }
    }
}
