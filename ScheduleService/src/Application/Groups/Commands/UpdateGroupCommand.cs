using Application.Common.Interfaces;
using MediatR;
using System;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Groups.Commands
{
    public class UpdateGroupCommand : IRequest<bool>
    {
        [JsonIgnore]
        public Guid Id { get; set; }
        public string GroupNumber { get; init; }
        public int YearOfStudy { get; init; }

        public UpdateGroupCommand() { }

        public UpdateGroupCommand(Guid id, string groupNumber, int yearOfStudy)
        {
            Id = id;
            GroupNumber = groupNumber;
            YearOfStudy = yearOfStudy;
        }
    }

    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, bool>
    {
        private readonly IWriteDbContext _context;

        public UpdateGroupCommandHandler(IWriteDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var groupEntity = _context.Groups
                .FirstOrDefault(g => g.Id == request.Id);

            if (groupEntity is null)
                return false;

            groupEntity.UpdateGroupInfo(request.GroupNumber, request.YearOfStudy);
            var result = await _context.SaveChangesAsync(cancellationToken);

            return result > 0;
        }
    }
}
