using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Commands
{
    public class UpdateEducOrgCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string Name { get; init; }
    }

    public class UpdateEducOrgCommandHandler : IRequestHandler<UpdateEducOrgCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public UpdateEducOrgCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateEducOrgCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.EducationalOrgs
                .FirstOrDefaultAsync(eo => eo.Id == request.Id, cancellationToken);

            if (entity is null)
                return false;

            entity.UpdateName(request.Name);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
