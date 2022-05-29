using Application.Common.Interfaces;
using Application.Groups.Dtos;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries
{
    public class GetBriefGroupsByEducOrgIdQuery : IRequest<IEnumerable<BriefGroupDto>>
    {
        public Guid EducOrgId { get; init; }

        public GetBriefGroupsByEducOrgIdQuery(Guid educOrgId)
        {
            EducOrgId = educOrgId;
        }
    }

    public class GetBriefGroupsByEducOrgIdQueryHandler
        : IRequestHandler<GetBriefGroupsByEducOrgIdQuery, IEnumerable<BriefGroupDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBriefGroupsByEducOrgIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BriefGroupDto>> Handle(GetBriefGroupsByEducOrgIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Groups
                .AsNoTracking()
                .Where(g => g.EducationalOrgId == request.EducOrgId)
                .Select(g => new BriefGroupDto(g.Id, g.GroupNumber))
                .ToListAsync(cancellationToken);
        }
    }
}
