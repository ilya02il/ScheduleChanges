using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class UpdateGroupCommand : IRequest<bool>
    {
        public Guid Id { get; init; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }

        public UpdateGroupCommand() { }

        public UpdateGroupCommand(Guid id, string groupNumber, int yearOfStudy)
        {
            Id = id;
            GroupNumber = groupNumber;
            YearOfStudy = yearOfStudy;
        }
    }

    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateGroupCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var groupEntity = _context.Groups
                .FirstOrDefault(g => g.Id == request.Id);

            if (groupEntity is null)
                return false;

            groupEntity.UpdateGroupInfo(request.GroupNumber, request.YearOfStudy);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
