using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Common.Interfaces;
using Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Application.ChangesLists.Queries
{
    public class GetChangesListByEducOrgIdQuery : IRequest<ChangesListEntity>
    {
        public Guid EducOrgId { get; init; }

        public GetChangesListByEducOrgIdQuery() { }
        public GetChangesListByEducOrgIdQuery(Guid educOrgId)
        {
            EducOrgId = educOrgId;
        }
    }

    public class GetChangesListByIdQueryHandler : IRequestHandler<GetChangesListByEducOrgIdQuery, ChangesListEntity>
    {
        private readonly IApplicationDbContext _context;

        public GetChangesListByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChangesListEntity> Handle(GetChangesListByEducOrgIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.ChangesLists
                .AsNoTracking()
                .Include(cl => cl.ListItems)
                .FirstAsync(cl => cl.EducationalOrgId == request.EducOrgId, cancellationToken);
        }
    }
}
