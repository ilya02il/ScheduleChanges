using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Queries.GetScheduleChangesList
{
    public class GetChangesListByIdQuery : IRequest<ChangesListEntity>
    {
        public Guid Id { get; init; }

        public GetChangesListByIdQuery() { }
        public GetChangesListByIdQuery(Guid id)
        {
            Id = id;
        }
    }

    public class GetChangesListByIdQueryHandler : IRequestHandler<GetChangesListByIdQuery, ChangesListEntity>
    {
        private readonly IApplicationDbContext _context;

        public GetChangesListByIdQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ChangesListEntity> Handle(GetChangesListByIdQuery request, CancellationToken cancellationToken)
        {
            var result = await _context.ChangesLists
                .AsNoTracking()
                .Include(cl => cl.ListItems)
                .FirstOrDefaultAsync(cl => cl.Id == request.Id, cancellationToken);

            return result;
        }
    }
}
