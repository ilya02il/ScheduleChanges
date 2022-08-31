using Application.Common.Interfaces;
using Dapper;
using MediatR;
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
        private readonly IReadDapperContext _context;

        public GetGroupNumbersListQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> Handle(GetGroupNumbersListQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query =
                $@"declare @educOrgId as uniqueidentifier =
                select top(1)
                    EducationalOrgs.Id
                from EducationalOrgs
                where
                    Name = @{nameof(request.EducOrgName)}

                select
                    GroupNumber
                from Groups
                where
                    EducationalOrgId = @educOrgId and
                    YearOfStudy = @{nameof(request.YearOfStudy)}";

            var command = new CommandDefinition(query,
                    new { request.EducOrgName, request.YearOfStudy },
                    cancellationToken: cancellationToken
                );

            var result = await connection.QueryAsync<Result>(command);

            return result.Select(r => r.GroupNumber);

            //var educOrgId = await _context.EducationalOrgs
            //    .AsNoTracking()
            //    .Where(eo => eo.Name == request.EducOrgName)
            //    .Select(eo => eo.Id)
            //    .SingleAsync(cancellationToken);

            //var result = await _context.Groups
            //    .AsNoTracking()
            //    .Where(g => g.EducationalOrgId == educOrgId && g.YearOfStudy == request.YearOfStudy)
            //    .Select(g => g.GroupNumber)
            //    .ToListAsync(cancellationToken);

            //return result;
        }

        private class Result
        {
            public string GroupNumber { get; set; }
        }
    }
}
