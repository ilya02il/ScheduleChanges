using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.UpdateChangesList
{
    public class UpdateChangesListCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public DateTimeOffset Date { get; init; }
        public bool IsOddWeek { get; init; }
    }

    public class UpdateChangesListCommandHandler : IRequestHandler<UpdateChangesListCommand, bool>
    {
        private readonly IWriteDbContext _context;

        public UpdateChangesListCommandHandler(IWriteDbContext context)
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
