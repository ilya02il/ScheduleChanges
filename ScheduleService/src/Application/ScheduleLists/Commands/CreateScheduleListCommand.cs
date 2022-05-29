using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleLists.Commands
{
    public class CreateScheduleListCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid GroupId { get; set; }
        public DayOfWeek DayOfWeek { get; init; }

        public CreateScheduleListCommand(Guid groupId, DayOfWeek dayOfWeek)
        {
            GroupId = groupId;
            DayOfWeek = dayOfWeek;
        }
    }

    public class CreateScheduleListCommandHandler : IRequestHandler<CreateScheduleListCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public CreateScheduleListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateScheduleListCommand request, CancellationToken cancellationToken)
        {
            var scheduleList = new ScheduleListEntity(request.GroupId, request.DayOfWeek);

            _context.ScheduleLists.Add(scheduleList);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
