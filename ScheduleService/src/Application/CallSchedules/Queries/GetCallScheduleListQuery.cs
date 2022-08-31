using Application.CallSchedules.Dtos;
using Application.Common.Interfaces;
using Dapper;
using Domain.Entities;
using MediatR;
using System;
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
        private readonly IReadDapperContext _context;

        public GetCallScheduleListQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<CallScheduleListDto> Handle(GetCallScheduleListQuery request, CancellationToken cancellationToken)
        {
            var connection = _context.CreateConnection();

            var query = $@"
                select
                    LessonCalls.Id as {nameof(LessonCallEntity.Id)},
                    Position as {nameof(LessonCallEntity.Position)},
                    StartTime as {nameof(LessonCallEntity.StartTime)},
                    EndTime as {nameof(LessonCallEntity.EndTime)}
                from LessonCalls
                where
                    {nameof(LessonCallEntity.EducationalOrgId)} = @{nameof(request.EducOrgId)} and
                    {nameof(LessonCallEntity.DayOfWeek)} = @{nameof(request.DayOfWeek)}
                order by
                    {nameof(LessonCallEntity.Position)}
                ";

            var command = new CommandDefinition(query,
                    new { request.EducOrgId, request.DayOfWeek },
                    cancellationToken: cancellationToken
                );

            var lessonCalls = await connection.QueryAsync<CallScheduleListItemDto>(command);

            var callScheduleList = new CallScheduleListDto()
            {
                DayOfWeek = request.DayOfWeek,
                ListItems = lessonCalls
            };

            return callScheduleList;
        }
    }
}
