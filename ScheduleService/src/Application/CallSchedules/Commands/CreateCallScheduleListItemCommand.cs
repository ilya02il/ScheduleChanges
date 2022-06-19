using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CallSchedules.Commands
{
    public class CreateCallScheduleListItemCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid EducOrgId { get; set; }
        public DayOfWeek DayOfWeek { get; init; }
        public int Position { get; init; }
        public string StartTime { get; init; }
        public string EndTime { get; init; }
    }

    public class CreateCallScheduleListItemCommandHandler : IRequestHandler<CreateCallScheduleListItemCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public CreateCallScheduleListItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateCallScheduleListItemCommand request, CancellationToken cancellationToken)
        {
            var newEntity = new LessonCallEntity(request.EducOrgId,
                request.Position,
                request.DayOfWeek,
                TimeSpan.Parse(request.StartTime),
                TimeSpan.Parse(request.EndTime));

            _context.LessonCalls.Add(newEntity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
