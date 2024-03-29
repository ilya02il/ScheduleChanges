﻿using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleLists.Commands
{
    public class DeleteScheduleListCommand : IRequest<bool>
    {
        public Guid Id { get; init; }

        public DeleteScheduleListCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteScheduleListCommandHandler : IRequestHandler<DeleteScheduleListCommand, bool>
    {
        private readonly IWriteDbContext _context;

        public DeleteScheduleListCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteScheduleListCommand request, CancellationToken cancellationToken)
        {
            var scheduleList = _context.ScheduleLists
                .FirstOrDefault(sl => sl.Id == request.Id);
            var scheduleListItems = _context.ScheduleListItems
                .Where(sl => sl.ScheduleListId == scheduleList.Id);

            if (scheduleListItems.Any())
                _context.ScheduleListItems.RemoveRange(scheduleListItems);

            _context.ScheduleLists.Remove(scheduleList);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
