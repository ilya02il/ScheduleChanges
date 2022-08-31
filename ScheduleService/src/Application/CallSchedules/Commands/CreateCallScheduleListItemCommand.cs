using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CallSchedules.Commands
{
    public class CreateCallScheduleListItemCommand : IRequest<Guid>
    {
        [JsonIgnore]
        public Guid EducOrgId { get; set; }
        public DayOfWeek DayOfWeek { get; init; }
        public int Position { get; init; }
        public long StartTime { get; init; }
        public long EndTime { get; init; }
    }

    public class CreateCallScheduleListItemCommandHandler : IRequestHandler<CreateCallScheduleListItemCommand, Guid>
    {
        private readonly IWriteDbContext _context;

        public CreateCallScheduleListItemCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateCallScheduleListItemCommand request, CancellationToken cancellationToken)
        {
            if (!_context.EducationalOrgs.Any(eo => eo.Id == request.EducOrgId))
                throw new Exception("There is no educational organization with such an id.");

            var newEntity = new LessonCallEntity(request.EducOrgId,
                request.Position,
                request.DayOfWeek,
                request.StartTime,
                request.EndTime);

            _context.LessonCalls.Add(newEntity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("Call schedule list item hasn't been added.");

            return newEntity.Id;
        }
    }
}
