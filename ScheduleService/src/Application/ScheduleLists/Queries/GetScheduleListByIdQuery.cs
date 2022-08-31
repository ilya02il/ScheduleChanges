using Application.Common.Interfaces;
using Application.ScheduleLists.Dtos;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleLists.Queries
{
    public class GetScheduleListByIdQuery : IRequest<ScheduleListDto>
    {
        public Guid Id { get; init; }

        public GetScheduleListByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetScheduleListByIdQueryHandler : IRequestHandler<GetScheduleListByIdQuery, ScheduleListDto>
    {
        private readonly IReadDapperContext _context;
        //private readonly IMapper _mapper;

        public GetScheduleListByIdQueryHandler(IReadDapperContext context/*, IMapper mapper*/)
        {
            _context = context;
            //_mapper = mapper;
        }

        public async Task<ScheduleListDto> Handle(GetScheduleListByIdQuery request, CancellationToken cancellationToken)
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
                    ScheduleLists.Id = @{nameof(request.Id)}";

            var command = new CommandDefinition(query,
                    new { request.Id },
                    cancellationToken: cancellationToken
                );

            var listDictionary = new Dictionary<Guid, ScheduleListDto>();

            var result = await connection.QueryAsync<ScheduleListDto, ScheduleListItemDto, ScheduleListDto>(command,
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

            return result.FirstOrDefault();

            //var scheduleList = await _context.ScheduleLists
            //    .Include(sl => sl.ListItems)
            //    .AsNoTracking()
            //    .FirstOrDefaultAsync(sl => sl.Id == request.Id, cancellationToken);

            //return _mapper.Map<ScheduleListDto>(scheduleList);
        }
    }
}
