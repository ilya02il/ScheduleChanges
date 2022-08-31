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
        private readonly IWriteDbContext _context;

        public UpdateCallScheduleListItemCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateCallScheduleListItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.LessonCalls
                .FirstOrDefault(lc => lc.Id == request.Id);

            if (entity is null)
                throw new Exception("There is no item with such an id.");

            entity.UpdateLessonCallInfo(request.Position,
                entity.DayOfWeek,
                request.StartTime,
                request.EndTime);

            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                return false;

            return true;
        }
    }
}
