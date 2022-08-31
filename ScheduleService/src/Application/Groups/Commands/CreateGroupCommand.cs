using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class CreateGroupCommand : IRequest<Guid>
    {
        public Guid EducOrgId { get; set; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }
    }

    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, Guid>
    {
        private readonly IWriteDbContext _context;

        public CreateGroupCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateGroupCommand request,
            CancellationToken cancellationToken)
        {
            var newGroup = new GroupEntity(request.EducOrgId,
                request.GroupNumber,
                request.YearOfStudy);

            _context.Groups.Add(newGroup);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("The group has not been created.");

            return newGroup.Id;
        }
    }
}
