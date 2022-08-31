using Application.Common.Interfaces;
using Application.Groups.Dtos;
using Dapper;
using MediatR;
using System;
using System.Collections.Generic;
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
        private readonly IReadDapperContext _context;

        public GetBriefGroupsByEducOrgIdQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BriefGroupDto>> Handle(GetBriefGroupsByEducOrgIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query = @$"select
                            Groups.Id as {nameof(BriefGroupDto.Id)},
                            GroupNumber as {nameof(BriefGroupDto.GroupNumber)}
                        from Groups
                        where
                            EducationalOrgId = @{nameof(request.EducOrgId)}
                        order by
                            YearOfStudy,
                            {nameof(BriefGroupDto.GroupNumber)}";

            var command = new CommandDefinition(query,
                    new { request.EducOrgId },
                    cancellationToken: cancellationToken
                );

            return await connection.QueryAsync<BriefGroupDto>(command);

            //return await _context.Groups
            //    .AsNoTracking()
            //    .Where(g => g.EducationalOrgId == request.EducOrgId)
            //    .Select(g => new BriefGroupDto(g.Id, g.GroupNumber))
            //    .ToListAsync(cancellationToken);
        }
    }
}
