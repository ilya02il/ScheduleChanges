﻿using Application.Common.Interfaces;
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
        public string StartTime { get; init; }
        public string EndTime { get; init; }
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
                TimeSpan.Parse(request.StartTime),
                TimeSpan.Parse(request.EndTime));

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
