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
    public class GetChangesListByIdQuery : IRequest<ChangesListEntity>
    {
        public Guid ListId { get; init; }

        public GetChangesListByIdQuery() { }
        public GetChangesListByIdQuery(Guid listId)
        {
            ListId = listId;
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
            return await _context.ChangesLists
                .Include(cl => cl.ListItems)
                .FirstAsync(cl => cl.Id == request.ListId, cancellationToken);
        }
    }

    public class GetChangesListByIdValidator : AbstractValidator<GetChangesListByIdQuery>
    {
        public GetChangesListByIdValidator()
        {
            RuleFor(q => q.ListId)
                .NotEmpty();
        }
    }
}
