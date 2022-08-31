using Application.Common.Interfaces;
using AutoMapper;
using Domain.ValueObjects;
using MediatR;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands.UpdateChangesListItem
{
    public class UpdateChangesListItemCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public Guid GroupId { get; init; }
        public int Position { get; init; }
        public string Discipline { get; init; }
        public string Teacher { get; init; }
        public string Auditorium { get; init; }
    }

    public class UpdateChangesListItemCommandHandler 
        : IRequestHandler<UpdateChangesListItemCommand, bool>
    {
        private readonly IWriteDbContext _context;
        private readonly IMapper _mapper;

        public UpdateChangesListItemCommandHandler(IWriteDbContext context,
            IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateChangesListItemCommand request, CancellationToken cancellationToken)
        {
            var changesListItem = _context.ChangesListItems
                .FirstOrDefault(li => li.Id == request.Id);

            changesListItem.UpdateItemInfo(_mapper.Map<ItemInfo>(request));
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
