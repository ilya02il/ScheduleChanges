using Application.Common.Interfaces;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ScheduleWithChangesList.Queries.GetEducOrgsListQuery
{
    public class GetEducOrgsListQuery : IRequest<IEnumerable<string>> { }

    public class GetEducOrgsListQueryHandler : IRequestHandler<GetEducOrgsListQuery, IEnumerable<string>>
    {
        private readonly IReadDapperContext _context;

        public GetEducOrgsListQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<string>> Handle(GetEducOrgsListQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query =
                @$"select
                    Name
                from EducationalOrgs
                order by
                    Name";

            var command = new CommandDefinition(query, cancellationToken: cancellationToken);

            var result = await connection.QueryAsync<Result>(command);

            return result.Select(r => r.Name);

            //var result = await _context.EducationalOrgs
            //    .AsNoTracking()
            //    .Select(eo => eo.Name)
            //    .ToListAsync(cancellationToken);

            //return result;
        }

        private class Result
        {
            public string Name { get; init; }
        }
    }
}
