using Application.CallSchedules.Dtos;
using Application.Common.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CallSchedules.Queries
{
    public class GetCallScheduleListQuery : IRequest<CallScheduleListDto>
    {
        public Guid EducOrgId { get; init; }
        public DayOfWeek DayOfWeek { get; init; }

        public GetCallScheduleListQuery(Guid educOrgId, DayOfWeek dayOfWeek)
        {
            EducOrgId = educOrgId;
            DayOfWeek = dayOfWeek;
        }
    }

    public class GetCallScheduleListQueryHandler : IRequestHandler<GetCallScheduleListQuery, CallScheduleListDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetCallScheduleListQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CallScheduleListDto> Handle(GetCallScheduleListQuery request, CancellationToken cancellationToken)
        {
            var lessonCalls = _context.LessonCalls
                .AsNoTracking()
                .Where(lc => lc.EducationalOrgId == request.EducOrgId && lc.DayOfWeek == request.DayOfWeek)
                .OrderBy(lc => lc.Position);

            var callScheduleList = new CallScheduleListDto()
            {
                DayOfWeek = request.DayOfWeek,
                ListItems = await lessonCalls.Select(lc => _mapper.Map<CallScheduleListItemDto>(lc))
                    .ToArrayAsync(cancellationToken)
            };

            return callScheduleList;
        }
    }
}
