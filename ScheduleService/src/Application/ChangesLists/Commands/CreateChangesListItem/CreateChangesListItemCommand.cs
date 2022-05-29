using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.CreateChangesListItem
{
    public class CreateChangesListItemCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid ListId { get; set; }
        public Guid GroupId { get; init; }
        public int Position { get; init; }
        public string Discipline { get; init; }
        public string Teacher { get; init; }
        public string Auditorium { get; init; }
    }

    public class UpdateChangesListItemCommandHandler 
        : IRequestHandler<CreateChangesListItemCommand, bool>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UpdateChangesListItemCommandHandler(IApplicationDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(CreateChangesListItemCommand request,
            CancellationToken cancellationToken)
        {
            var changesListItem = _mapper.Map<ChangesListItemEntity>(request);

            _context.ChangesListItems.Add(changesListItem);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
