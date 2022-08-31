using Application.Common.Interfaces;
using Application.ScheduleLists.Dtos;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleLists.Queries
{
    public class GetScheduleListsByGroupIdQuery : IRequest<IEnumerable<ScheduleListDto>>
    {
        public Guid GroupId { get; init; }

        public GetScheduleListsByGroupIdQuery(Guid groupId)
        {
            GroupId = groupId;
        }
    }

    public class GetScheduleListsByGroupIdQueryHandler
        : IRequestHandler<GetScheduleListsByGroupIdQuery, IEnumerable<ScheduleListDto>>
    {
        private readonly IReadDapperContext _context;
        //private readonly IMapper _mapper;

        public GetScheduleListsByGroupIdQueryHandler(IReadDapperContext context/*, IMapper mapper*/)
        {
            _context = context;
            //_mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleListDto>> Handle(GetScheduleListsByGroupIdQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query =
                $@"select
                    ScheduleLists.Id as {nameof(ScheduleListDto.Id)},
                    ScheduleLists.DayOfWeek as {nameof(ScheduleListDto.DayOfWeek)},
                    ScheduleListItems.Id as {nameof(ScheduleListItemDto.ItemId)},
                    ListItems.ItemInfo_Position as {nameof(ScheduleListItemDto.Position)},
	                ListItems.ItemInfo_SubjectName as {nameof(ScheduleListItemDto.Discipline)},
	                ListItems.ItemInfo_TeacherInitials as {nameof(ScheduleListItemDto.Teacher)},
	                ListItems.ItemInfo_Auditorium as {nameof(ScheduleListItemDto.Auditorium)},
	                ScheduleListItems.IsOddWeek as {nameof(ScheduleListItemDto.IsOddWeek)}
                from ScheduleLists
                    left join ScheduleListItems on ScheduleLists.Id = ScheduleListId
	                left join ListItems on ScheduleListItems.Id = ListItems.Id
                where
                    ScheduleList.GroupId = @{nameof(request.GroupId)}";

            var command = new CommandDefinition(query,
                    new { request.GroupId },
                    cancellationToken: cancellationToken
                );

            var listDictionary = new Dictionary<Guid, ScheduleListDto>();

            return await connection.QueryAsync<ScheduleListDto, ScheduleListItemDto, ScheduleListDto>(command,
                (list, item) =>
                {
                    if (!listDictionary.TryGetValue(list.Id, out ScheduleListDto listEntry))
                    {
                        listEntry = list;
                        listDictionary.Add(list.Id, listEntry);
                    }

                    if (item.ItemId != Guid.Empty)
                        listEntry.ListItems.Add(item);

                    return listEntry;
                },
                splitOn: nameof(ScheduleListItemDto.ItemId)
            );

            //var scheduleLists = await _context.ScheduleLists
            //    .Include(sl => sl.ListItems)
            //    .AsNoTracking()
            //    .Where(sl => sl.GroupId == request.GroupId)
            //    .OrderBy(sl => sl.DayOfWeek)
            //    .ToListAsync(cancellationToken);

            //return _mapper.Map<List<ScheduleListDto>>(scheduleLists);
        }
    }
}
