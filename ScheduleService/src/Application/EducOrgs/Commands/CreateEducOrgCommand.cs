using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Commands
{
    public class CreateEducOrgCommand : IRequest<Guid>
    {
        public string Name { get; init; }
    }

    public class CreateEducOrgCommandHandler : IRequestHandler<CreateEducOrgCommand, Guid>
    {
        private readonly IWriteDbContext _context;

        public CreateEducOrgCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateEducOrgCommand request, CancellationToken cancellationToken)
        {
            var entity = new EducationalOrgEntity(request.Name);

            _context.EducationalOrgs.Add(entity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result <= 0)
                throw new Exception("The educaional organization has not been created.");

            return entity.Id;
        }
    }
}
