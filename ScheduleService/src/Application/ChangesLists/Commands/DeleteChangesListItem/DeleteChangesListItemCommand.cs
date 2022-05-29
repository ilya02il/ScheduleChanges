using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.DeleteChangesListItem
{
    public class DeleteChangesListItemCommand : IRequest<bool>
    {
        public Guid Id { get; init; }

        public DeleteChangesListItemCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteChangesListItemCommandHandler 
        : IRequestHandler<DeleteChangesListItemCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteChangesListItemCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteChangesListItemCommand request, CancellationToken cancellationToken)
        {
            var changesListItem = _context.ChangesListItems
                .FirstOrDefault(li => li.Id == request.Id);

            _context.ChangesListItems.Remove(changesListItem);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
