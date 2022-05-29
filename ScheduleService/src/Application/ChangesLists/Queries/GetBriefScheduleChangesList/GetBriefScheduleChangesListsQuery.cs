using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Queries.GetBriefScheduleChangesList
{
    public class GetBriefScheduleChangesListsQuery : IRequest<IEnumerable<BriefChangesListDto>>
    {
        public Guid EducOrgId { get; init; }

        public GetBriefScheduleChangesListsQuery(Guid educOrgId)
        {
            EducOrgId = educOrgId;
        }
    }

    public class GetBriefScheduleChangesListQueryHandler : IRequestHandler<GetBriefScheduleChangesListsQuery, IEnumerable<BriefChangesListDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetBriefScheduleChangesListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BriefChangesListDto>> Handle(GetBriefScheduleChangesListsQuery request, CancellationToken cancellationToken)
        {
            return await _context.ChangesLists
                .AsNoTracking()
                .OrderBy(cl => cl.Date)
                .Select(cl => new BriefChangesListDto(cl.Id, cl.Date))
                .ToListAsync(cancellationToken);
        }
    }
}
