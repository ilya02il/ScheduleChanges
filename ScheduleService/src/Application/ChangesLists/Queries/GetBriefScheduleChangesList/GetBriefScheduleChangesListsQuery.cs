using Application.Common.Interfaces;
using Dapper;
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
        private readonly IReadDapperContext _context;

        public GetBriefScheduleChangesListQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BriefChangesListDto>> Handle(GetBriefScheduleChangesListsQuery request, CancellationToken cancellationToken)
        {
            var connection = _context.CreateConnection();

            var query = $@"
                select
                    ChangesLists.Id as {nameof(BriefChangesListDto.Id)},
                    Date as {nameof(BriefChangesListDto.Date)}
                from ChangesLists
                where
                    EducationalOrgId = @{nameof(request.EducOrgId)}
                order by
                    {nameof(BriefChangesListDto.Date)}
                ";

            var command = new CommandDefinition(query,
                    new { request.EducOrgId },
                    cancellationToken: cancellationToken
                );

            return await connection.QueryAsync<BriefChangesListDto>(command);
        }
    }
}
