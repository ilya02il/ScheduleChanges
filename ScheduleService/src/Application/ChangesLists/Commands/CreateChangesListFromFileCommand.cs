using Application.Common.Interfaces;
using Application.Common.Mappings.TableMappings;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Application.ChangesLists.Commands
{
    public class CreateChangesListFromFileCommand : IRequest<int>
    {
        public DateTime Date { get; init; }
        public IFormFile File { get; init; }
        public ChangesTableMapDto TableMap { get; init; }
    }

    public class CreateChangesListFromStreamCommandHandler : IRequestHandler<CreateChangesListFromFileCommand, int>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITableFileParser<ItemInfo> _fileParser;

        public CreateChangesListFromStreamCommandHandler(IApplicationDbContext context, ITableFileParser<ItemInfo> fileParser)
        {
            _context = context;
            _fileParser = fileParser;
        }

        public async Task<int> Handle(CreateChangesListFromFileCommand request, CancellationToken cancellationToken)
        {
            try
            {
                IEnumerable<ItemInfo> changesListItemsFromFile;

                if (request.TableMap is null)
                {
                    changesListItemsFromFile = _fileParser.Parse(request.File.OpenReadStream());
                }

                else
                {
                    var tableMap = new TableMap<ItemInfo>();

                    tableMap.AddColumnMap(item => item.Position, request.TableMap.Position.ToString());
                    tableMap.AddColumnMap(item => item.SubjectName, request.TableMap.SubjectName);
                    tableMap.AddColumnMap(item => item.TeacherInitials, request.TableMap.TeacherInitials);
                    tableMap.AddColumnMap(item => item.Auditorium, request.TableMap.Auditorium);

                    changesListItemsFromFile = _fileParser.Parse(request.File.OpenReadStream(), tableMap);
                }

                if (changesListItemsFromFile is null || !changesListItemsFromFile.Any())
                    throw new Exception("File is empty");

                //var changesList = new ChangesListEntity(request.Date, changesListItemsFromFile.Select(item => new ChangesListItemEntity(item)).ToList());
                //_context.ChangesLists.Add(changesList);

                return await _context.SaveChangesAsync(cancellationToken);
            }
            finally
            {
                _fileParser.Dispose();
                GC.Collect();
            }
        }
    }
}
