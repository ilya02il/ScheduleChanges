using Application.Common.Interfaces;
using Application.EducOrgs.Dtos;
using Application.Groups.Dtos;
using Dapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.EducOrgs.Queries
{
    public class GetEducOrgByIdQuery : IRequest<EducOrgDto>
    {
        public Guid Id { get; init; }

        public GetEducOrgByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetEducOrgByIdQueryHandler : IRequestHandler<GetEducOrgByIdQuery, EducOrgDto>
    {
        private readonly IReadDapperContext _context;

        public GetEducOrgByIdQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<EducOrgDto> Handle(GetEducOrgByIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query = $@"
                select
                    EducationalOrgs.Id as {nameof(EducOrgDto.Id)},
                    Name as {nameof(EducOrgDto.Name)}
                from EducationalOrgs
                where
                    {nameof(EducOrgDto.Id)} = @{nameof(request.Id)}
                ";

            var command = new CommandDefinition(query,
                    new { request.Id },
                    cancellationToken: cancellationToken
                );

            var educOrg = await connection.QueryFirstOrDefaultAsync<EducOrgDto>(command);

            if (educOrg is null)
                return default;

            query = $@"
                select
                    Groups.Id as {nameof(GroupDto.Id)},
                    GroupNumber as {nameof(GroupDto.GroupNumber)},
                    YearOfStudy as {nameof(GroupDto.YearOfStudy)}
                from Groups
                where
                    EducationalOrgId = @{educOrg.Id}
                ";

            command = new CommandDefinition(query,
                    new { educOrg.Id },
                    cancellationToken: cancellationToken
                );

            var groups = await connection.QueryAsync<GroupDto>(command);

            foreach (var group in groups)
                educOrg.Groups.Add(group);

            return educOrg;
        }
    }
}
