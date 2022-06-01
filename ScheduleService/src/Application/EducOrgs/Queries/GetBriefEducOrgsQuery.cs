using Application.Common.Interfaces;
using Application.EducOrgs.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Queries
{
    public class GetBriefEducOrgsQuery : IRequest<IEnumerable<BriefEducOrgDto>>
    {
    }

    public class GetBriefEducOrgsQueryHandler : IRequestHandler<GetBriefEducOrgsQuery, IEnumerable<BriefEducOrgDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBriefEducOrgsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BriefEducOrgDto>> Handle(GetBriefEducOrgsQuery request, CancellationToken cancellationToken)
        {
            return await _context.EducationalOrgs
                .AsNoTracking()
                .Select(eo => new BriefEducOrgDto(eo.Id, eo.Name))
                .ToListAsync(cancellationToken);
        }
    }
}
