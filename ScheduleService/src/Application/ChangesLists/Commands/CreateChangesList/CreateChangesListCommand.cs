using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.CreateChangesList
{
    public class CreateChangesListCommand : IRequest<bool>
    {
        public Guid EducOrgId { get; set; }
        public DateTimeOffset Date { get; set; }
        public bool IsOddWeek { get; set; }
        public IEnumerable<ChangesListItemDto> ListItems { get; set; }
    }

    public class CreateChangesListCommandHandler : IRequestHandler<CreateChangesListCommand, bool>
    {
        private readonly IApplicationDbContext _context;

        public CreateChangesListCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(CreateChangesListCommand request, CancellationToken cancellationToken)
        {

            var changesList = new ChangesListEntity(request.EducOrgId,
                request.Date.LocalDateTime,
                request.IsOddWeek);

            _context.ChangesLists.Add(changesList);

            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
