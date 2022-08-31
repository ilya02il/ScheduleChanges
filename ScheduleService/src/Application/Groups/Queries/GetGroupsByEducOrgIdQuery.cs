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
    public class GetGroupsByEducOrgIdQuery : IRequest<IEnumerable<GroupDto>>
    {
        public Guid EducOrgId { get; init; }

        public GetGroupsByEducOrgIdQuery(Guid educOrgId)
        {
            EducOrgId = educOrgId;
        }
    }

    public class GetGroupsByEducOrgIdQueryHandler : IRequestHandler<GetGroupsByEducOrgIdQuery, IEnumerable<GroupDto>>
    {
        private readonly IReadDapperContext _context;
        //private readonly IMapper _mapper;

        public GetGroupsByEducOrgIdQueryHandler(IReadDapperContext context
            /*IMapper mapper*/)
        {
            _context = context;
            //_mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsByEducOrgIdQuery request, CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query =
                $@"select
                    Groups.Id as {nameof(GroupDto.Id)},
                    GroupNumber as {nameof(GroupDto.GroupNumber)},
                    YearOfStudy as {nameof(GroupDto.YearOfStudy)}
                from Groups
                where
                    EducationalOrgId = @{nameof(request.EducOrgId)}
                order by
                    {nameof(GroupDto.YearOfStudy)},
                    {nameof(GroupDto.GroupNumber)}";

            var command = new CommandDefinition(query,
                    new { request.EducOrgId },
                    cancellationToken: cancellationToken
                );

            return await connection.QueryAsync<GroupDto>(command);
        }
    }
}
