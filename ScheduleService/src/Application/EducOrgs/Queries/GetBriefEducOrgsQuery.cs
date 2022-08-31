using Application.Common.Interfaces;
using Application.EducOrgs.Dtos;
using Dapper;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Queries
{
    public class GetBriefEducOrgsQuery : IRequest<IEnumerable<BriefEducOrgDto>>
    {
    }

    public class GetBriefEducOrgsQueryHandler : IRequestHandler<GetBriefEducOrgsQuery, IEnumerable<BriefEducOrgDto>>
    {
        private readonly IReadDapperContext _context;

        public GetBriefEducOrgsQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BriefEducOrgDto>> Handle(GetBriefEducOrgsQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query =
            @$"select
                  EducationalOrgs.Id as {nameof(BriefEducOrgDto.Id)},
                  Name as {nameof(BriefEducOrgDto.Name)}
              from EducationalOrgs";

            var command = new CommandDefinition(query, cancellationToken);

            return await connection.QueryAsync<BriefEducOrgDto>(command);
        }
    }
}
