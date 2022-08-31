using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class DeleteGroupCommand : IRequest<bool>
    {
        public Guid Id { get; init; }

        public DeleteGroupCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand, bool>
    {
        private readonly IWriteDbContext _context;

        public DeleteGroupCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var groupEntity = _context.Groups
                .FirstOrDefault(g => g.Id == request.Id);

            if (groupEntity is null)
                throw new Exception("Group is not found in database.");

            _context.Groups.Remove(groupEntity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
