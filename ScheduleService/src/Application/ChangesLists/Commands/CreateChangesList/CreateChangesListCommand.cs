using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.CreateChangesList
{
    public class CreateChangesListCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid EducOrgId { get; set; }
        public DateTimeOffset Date { get; init; }
        public bool IsOddWeek { get; init; }
        public IEnumerable<ChangesListItemDto> ListItems { get; init; }
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
