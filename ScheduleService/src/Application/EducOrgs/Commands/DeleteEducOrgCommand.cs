using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Commands
{
    public class DeleteEducOrgCommand : IRequest<bool>
    {
        public Guid Id { get; init; }

        public DeleteEducOrgCommand(Guid id)
        {
            Id = id;
        }
    }

    public class DeleteEducOrgCommandHandler : IRequestHandler<DeleteEducOrgCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public DeleteEducOrgCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(DeleteEducOrgCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.EducationalOrgs
                .FirstOrDefaultAsync(eo => eo.Id == request.Id, cancellationToken);

            if (entity is null)
                return false;

            _context.EducationalOrgs.Remove(entity);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
