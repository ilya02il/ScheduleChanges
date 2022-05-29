using Application.Common.Interfaces;
using Application.ScheduleLists.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetScheduleListsByGroupIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleListDto>> Handle(GetScheduleListsByGroupIdQuery request,
            CancellationToken cancellationToken)
        {
            var scheduleLists = await _context.ScheduleLists
                .Include(sl => sl.ListItems)
                .AsNoTracking()
                .Where(sl => sl.GroupId == request.GroupId)
                .OrderBy(sl => sl.DayOfWeek)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<ScheduleListDto>>(scheduleLists);
        }
    }
}
