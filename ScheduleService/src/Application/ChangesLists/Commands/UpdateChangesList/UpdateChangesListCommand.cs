using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.UpdateChangesList
{
    public class UpdateChangesListCommand : IRequest<bool>
    {
        public Guid Id { get; init; }
        public DateTimeOffset Date { get; init; }
        public bool IsOddWeek { get; init; }
    }

    public class UpdateChangesListCommandHandler : IRequestHandler<UpdateChangesListCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateChangesListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateChangesListCommand request, CancellationToken cancellationToken)
        {
            var changesList = _context.ChangesLists
                .FirstOrDefault(sl => sl.Id == request.Id);

            changesList.UpdateListInfo(request.Date, request.IsOddWeek);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
