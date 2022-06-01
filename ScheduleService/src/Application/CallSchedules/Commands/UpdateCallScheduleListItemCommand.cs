using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CallSchedules.Commands
{
    public class UpdateCallScheduleListItemCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public int Position { get; init; }
        public long StartTime { get; init; }
        public long EndTime { get; init; }
    }

    public class UpdateCallScheduleListItemCommandHandler : IRequestHandler<UpdateCallScheduleListItemCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateCallScheduleListItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCallScheduleListItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.LessonCalls
                .FirstOrDefault(lc => lc.Id == request.Id);

            entity.UpdateLessonCallInfo(request.Position,
                entity.DayOfWeek,
                TimeSpan.FromTicks(request.StartTime),
                TimeSpan.FromTicks(request.EndTime));

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
