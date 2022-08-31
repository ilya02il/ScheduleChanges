using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.DeleteChangesList
{
    public class DeleteChangesListCommand : IRequest<bool>
    {
        public Guid Id { get; init; }

        public DeleteChangesListCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteChangesListCommandHandler : IRequestHandler<DeleteChangesListCommand, bool>
    {
        private readonly IWriteDbContext _context;

        public DeleteChangesListCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteChangesListCommand request, CancellationToken cancellationToken)
        {
            var changesList = _context.ChangesLists
                .FirstOrDefault(cl => cl.Id == request.Id);

            _context.ChangesLists.Remove(changesList);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
