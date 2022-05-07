using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleWithChangesList.Queries.GetEducOrgsListQuery
{
    public class GetEducOrgsListQuery : IRequest<IEnumerable<string>> { }

    public class GetEducOrgsListQueryHandler : IRequestHandler<GetEducOrgsListQuery, IEnumerable<string>>
    {
        private readonly IApplicationDbContext _context;

        public GetEducOrgsListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> Handle(GetEducOrgsListQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.EducationalOrgs
                .AsNoTracking()
                .Select(eo => eo.Name)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
