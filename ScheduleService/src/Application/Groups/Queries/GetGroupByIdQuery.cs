using Application.Common.Interfaces;
using Application.Groups.Dtos;
using Dapper;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Queries
{
    public class GetGroupByIdQuery : IRequest<GroupDto>
    {
        public Guid Id { get; init; }

        public GetGroupByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetGroupByIdQueryHandler : IRequestHandler<GetGroupByIdQuery, GroupDto>
    {
        private readonly IReadDapperContext _context;

        public GetGroupByIdQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<GroupDto> Handle(GetGroupByIdQuery request,
            CancellationToken cancellationToken)
        {
            using var connection = _context.CreateConnection();

            var query = $@"
                select
                    Groups.Id as {nameof(GroupDto.Id)},
                    GroupNumber as {nameof(GroupDto.GroupNumber)},
                    YearOfStudy as {nameof(GroupDto.YearOfStudy)}
                from Groups
                where
                    {nameof(GroupDto.Id)} = @{nameof(request.Id)}";

            var command = new CommandDefinition(query,
                    new { request.Id },
                    cancellationToken: cancellationToken
                );

            return await connection.QueryFirstOrDefaultAsync<GroupDto>(command);
        }
    }
}
