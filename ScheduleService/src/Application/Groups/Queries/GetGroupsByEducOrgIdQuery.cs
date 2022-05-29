using Application.Common.Interfaces;
using Application.Groups.Dtos;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GetGroupsByEducOrgIdQueryHandler(IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> Handle(GetGroupsByEducOrgIdQuery request, CancellationToken cancellationToken)
        {
            var groups = await _context.Groups
                .AsNoTracking()
                .Where(g => g.EducationalOrgId == request.EducOrgId)
                .ToListAsync(cancellationToken);

            return _mapper.Map<List<GroupDto>>(groups);
        }
    }
}
