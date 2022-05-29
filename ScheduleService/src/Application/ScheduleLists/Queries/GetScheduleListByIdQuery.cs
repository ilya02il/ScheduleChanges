using Application.Common.Interfaces;
using Application.ScheduleLists.Dtos;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
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
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetScheduleListByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ScheduleListDto> Handle(GetScheduleListByIdQuery request, CancellationToken cancellationToken)
        {
            var scheduleList = await _context.ScheduleLists
                .Include(sl => sl.ListItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(sl => sl.Id == request.Id, cancellationToken);

            return _mapper.Map<ScheduleListDto>(scheduleList);
        }
    }
}
