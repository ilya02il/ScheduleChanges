using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Commands
{
    public class CreateEducOrgCommand : IRequest<bool>
    {
        public string Name { get; init; }
    }

    public class CreateEducOrgCommandHandler : IRequestHandler<CreateEducOrgCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public CreateEducOrgCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateEducOrgCommand request, CancellationToken cancellationToken)
        {
            var entity = new EducationalOrgEntity(request.Name);

            _context.EducationalOrgs.Add(entity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
