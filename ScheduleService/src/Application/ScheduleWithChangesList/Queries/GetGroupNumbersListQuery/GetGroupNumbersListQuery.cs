using Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleWithChangesList.Queries.GetGroupNumbersListQuery
{
    public class GetGroupNumbersListQuery : IRequest<IEnumerable<string>>
    {
        public string EducOrgName { get; init; }
        public int YearOfStudy { get; init; }

        public GetGroupNumbersListQuery(string educOrgName, int yearOfStudy)
        {
            EducOrgName = educOrgName;
            YearOfStudy = yearOfStudy;
        }
    }

    public class GetGroupNumbersListQueryHandler : IRequestHandler<GetGroupNumbersListQuery, IEnumerable<string>>
    {
        private readonly IApplicationDbContext _context;

        public GetGroupNumbersListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> Handle(GetGroupNumbersListQuery request, CancellationToken cancellationToken)
        {
            var educOrgId = await _context.EducationalOrgs
                .AsNoTracking()
                .Where(eo => eo.Name == request.EducOrgName)
                .Select(eo => eo.Id)
                .SingleAsync(cancellationToken);

            var result = await _context.Groups
                .AsNoTracking()
                .Where(g => g.EducationalOrgId == educOrgId && g.YearOfStudy == request.YearOfStudy)
                .Select(g => g.GroupNumber)
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
