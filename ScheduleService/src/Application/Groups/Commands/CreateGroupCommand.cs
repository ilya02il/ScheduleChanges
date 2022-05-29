using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<bool>
    {
        public Guid EducOrgId { get; init; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }
    }

    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public CreateGroupCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var newGroup = new GroupEntity(request.EducOrgId,
                request.GroupNumber,
                request.YearOfStudy);

            _context.Groups.Add(newGroup);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
