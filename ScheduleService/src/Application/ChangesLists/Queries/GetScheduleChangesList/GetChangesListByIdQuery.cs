using Application.Common.Interfaces;
using Dapper;
using Domain.Common;
using Domain.Entities;
using MediatR;
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
        private readonly IReadDapperContext _context;

        public GetChangesListByIdQueryHandler(IReadDapperContext context)
        {
            _context = context;
        }

        public async Task<ChangesListEntity> Handle(GetChangesListByIdQuery request, CancellationToken cancellationToken)
        {
            var connection = _context.CreateConnection();

            var query = $@"
                select
                    ChangesLists.Id as {nameof(ChangesListEntity.Id)},
                    Date as {nameof(ChangesListEntity.Date)},
                    IsOddWeek as {nameof(ChangesListEntity.IsOddWeek)},
                    EducationalOrgId as {nameof(ChangesListEntity.EducationalOrgId)}
                from ChangesLists
                where
                    {nameof(ChangesListEntity.Id)} = @{nameof(request.Id)}
                ";

            var command = new CommandDefinition(query,
                    new { request.Id },
                    cancellationToken: cancellationToken
                );

            var changesList = await connection.QueryFirstOrDefaultAsync<ChangesListEntity>(command);

            query = $@"
                select
                    select
                    ChangesListItems.Id as ChangesListItemId,
	                ListItems.ItemInfo_Position as {nameof(ListItemEntity.ItemInfo.Position)},
	                ListItems.ItemInfo_SubjectName as {nameof(ListItemEntity.ItemInfo.SubjectName)},
	                ListItems.ItemInfo_TeacherInitials as {nameof(ListItemEntity.ItemInfo.TeacherInitials)},
	                ListItems.ItemInfo_Auditorium as {nameof(ListItemEntity.ItemInfo.Auditorium)},
	                GroupId as {nameof(ChangesListItemEntity.GroupId)},
	                ChangesListId as {nameof(ChangesListItemEntity.ChangesListId)}
                from ChangesListItems
                    left join ListItems on ChangesListItems.Id = ListItems.Id
                where
                    {nameof(ChangesListItemEntity.ChangesListId)} = @{nameof(changesList.Id)}
                ";

            command = new CommandDefinition(query,
                    new { changesList.Id },
                    cancellationToken: cancellationToken
                );

            changesList.AppendItems(await connection.QueryAsync<ChangesListItemEntity>(command));

            return changesList;
        }
    }
}
