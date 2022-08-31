using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.CallSchedules.Commands
{
    public class DeleteCallScheduleListItemCommand : IRequest<bool>
    {
        public Guid Id { get; init; }

        public DeleteCallScheduleListItemCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteCallScheduleListItemCommandHandler : IRequestHandler<DeleteCallScheduleListItemCommand, bool>
    {
        private readonly IWriteDbContext _context;

        public DeleteCallScheduleListItemCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteCallScheduleListItemCommand request, CancellationToken cancellationToken)
        {
            var entity = _context.LessonCalls
                .FirstOrDefault(lc => lc.Id == request.Id);

            if (entity is null)
                throw new Exception("There is no item with such as id.");

            _context.LessonCalls.Remove(entity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
