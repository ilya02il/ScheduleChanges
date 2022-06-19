using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
         
        public GetScheduleWithChangesListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ScheduleWithChangesDto> Handle(GetScheduleWithChangesListQuery request, CancellationToken cancellationToken)
        {
            //взять из базы изменения с нужными названием образовательной организации
            //и номером группы

            var educOrgId = _context.EducationalOrgs
                .AsNoTracking()
                .Where(eo => eo.Name == request.EducOrgName)
                .Select(eo => eo.Id)
                .First();
            var groupId = _context.Groups
                .AsNoTracking()
                .Where(g => g.GroupNumber == request.GroupNumber &&
                    g.EducationalOrgId == educOrgId)
                .Select(g => g.Id)
                .First();

            var changesList = await _context.ChangesLists
                .AsNoTracking()
                .FirstOrDefaultAsync(cl => cl.EducationalOrgId == educOrgId &&
                    cl.Date.Date == request.Date.Date.Date,
                    cancellationToken);


            var changesListItems = changesList is null ? 
                null :
                _context.ChangesListItems
                    .AsNoTracking()
                    .Where(li => li.ChangesListId == changesList.Id && li.GroupId == groupId)
                    .OrderBy(li => li.ItemInfo.Position)
                    .ToList();

            //взять из базы расписание с нужными названием образовательной организации
            //и номером группы

            var isOddWeek = changesList?.IsOddWeek;

            var scheduleList = await _context.ScheduleLists
                .AsNoTracking()
                .FirstOrDefaultAsync(sl => sl.DayOfWeek == request.Date.DayOfWeek &&
                    sl.GroupId == groupId,
                    cancellationToken);

            var scheduleListItems = scheduleList is null ?
                null :
                _context.ScheduleListItems
                    .AsNoTracking()
                    .Where(li => li.ScheduleListId == scheduleList.Id &&
                        (li.IsOddWeek == (isOddWeek ?? true) ||
                        li.IsOddWeek == null))
                    .OrderBy(li => li.ItemInfo.Position)
                    .ToList();

            if (scheduleListItems is null)
                return new ScheduleWithChangesDto()
                {
                    EducOrgName = request.EducOrgName,
                    GroupNumber = request.GroupNumber,
                    Date = request.Date,
                    ScheduleItems = new List<ScheduleWithChangesListItemDto>()
                };

            var resultItemsList = new List<ScheduleWithChangesListItemDto>();

            IQueryable<LessonCallEntity> lessonCalls = _context.DatedLessonCalls
                .AsNoTracking()
                .Where(dlc => dlc.EducationalOrgId == educOrgId &&
                    dlc.Date == request.Date);

            if (!lessonCalls.Any())
            {
                lessonCalls = _context.LessonCalls
                    .AsNoTracking()
                    .Where(lc => lc.EducationalOrgId == educOrgId &&
                        lc.DayOfWeek == request.Date.DayOfWeek);
            }

            var lessonCallsList = lessonCalls.ToList();

            ScheduleWithChangesListItemDto buffer;
            LessonCallEntity lessonCallsBuffer;

            for (int i = 0; i < scheduleListItems.Count; i++)
            {
                buffer = null;

                if (changesList is not null)
                {
                    for (int j = 0; j < changesListItems.Count; j++)
                    {
                        if (!(changesListItems[j].ItemInfo.Position <= scheduleListItems[i].ItemInfo.Position))
                            break;

                        if (changesListItems[j].ItemInfo.Position < scheduleListItems[i].ItemInfo.Position)
                            i--;

                        buffer = _mapper.Map<ScheduleWithChangesListItemDto>(changesListItems[j]);
                        lessonCallsBuffer = lessonCallsList.Single(lc => lc.Position == changesListItems[j].ItemInfo.Position);

                        buffer.StartTime = lessonCallsBuffer.StartTime;
                        buffer.EndTime = lessonCallsBuffer.EndTime;

                        changesListItems.Remove(changesListItems[j]);
                    }
                }

                if (buffer is null)
                {
                    buffer = _mapper.Map<ScheduleWithChangesListItemDto>(scheduleListItems[i]);
                    lessonCallsBuffer = lessonCallsList.Single(lc => lc.Position == scheduleListItems[i].ItemInfo.Position);

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
