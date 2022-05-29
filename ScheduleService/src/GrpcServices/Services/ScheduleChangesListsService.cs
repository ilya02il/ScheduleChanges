using Application.ChangesLists.Commands.CreateChangesList;
using Application.ChangesLists.Commands.CreateChangesListItem;
using Application.ChangesLists.Commands.DeleteChangesList;
using Application.ChangesLists.Commands.DeleteChangesListItem;
using Application.ChangesLists.Commands.UpdateChangesList;
using Application.ChangesLists.Commands.UpdateChangesListItem;
using Application.ChangesLists.Queries;
using Application.ChangesLists.Queries.GetBriefScheduleChangesList;
using Application.ChangesLists.Queries.GetScheduleChangesList;
using AutoMapper;
using ChangesLists.Messages;
using ChangesLists.Service;
using Grpc.Core;
using MediatR;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcAPI.Services
{
    public class ScheduleChangesListsService : GrpcScheduleChangesLists.GrpcScheduleChangesListsBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public ScheduleChangesListsService(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public async override Task<GetBriefChangesListCollectionResponse> GetBriefChangesListCollection(GetByEducOrgIdRequest request, ServerCallContext context)
        {
            var senderRequest = new GetBriefScheduleChangesListsQuery(Guid.Parse(request.EducOrgId));

            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            var response = new GetBriefChangesListCollectionResponse();
            response.Lists
                .AddRange(senderResponse.Select(item => _mapper.Map<BriefChangesList>(item)));

            return response;
        }

        public async override Task<GetChangesListByIdResponse> GetScheduleChangesListById(GetByIdRequest request, ServerCallContext context)
        {
            var senderRequest = new GetChangesListByIdQuery()
            {
                Id = Guid.Parse(request.Id)
            };

            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            return _mapper.Map<GetChangesListByIdResponse>(senderResponse);
        }

        public override async Task<SucceedResponse> CreateScheduleChangesList(CreateScheduleChangesListRequest request, ServerCallContext context)
        {
            var senderRequest = new CreateChangesListCommand()
            {
                EducOrgId = Guid.Parse(request.EducOrgId),
                Date = request.Date.ToDateTimeOffset(),
                IsOddWeek = request.IsOddWeek
            };

            var result = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return result;
        }

        public override async Task<SucceedResponse> CreateScheduleChangesListFromFile(IAsyncStreamReader<CreateScheduleChangesListFromFileRequest> requestStream, ServerCallContext context)
        {
            using var fileStream = File.Create("D:\\"+ Guid.NewGuid() + ".exe");

            await foreach (var chunk in requestStream.ReadAllAsync())
            {
                if (chunk.DataCase == CreateScheduleChangesListFromFileRequest.DataOneofCase.FileData)
                {
                    await fileStream.WriteAsync(chunk.FileData.ToByteArray(), context.CancellationToken);
                }
            }

            return await Task.FromResult(new SucceedResponse() { IsSucceed = true });
        }

        public override async Task<SucceedResponse> UpdateScheduleChangesList(UpdateScheduleChangesListRequest request, ServerCallContext context)
        {
            var senderRequest = new UpdateChangesListCommand()
            {
                Id = Guid.Parse(request.Id),
                Date = request.Date.ToDateTimeOffset(),
                IsOddWeek = request.IsOddWeek
            };

            var result = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return result;
        }

        public override async Task<SucceedResponse> DeleteScheduleChangesList(DeleteScheduleChangesListRequest request, ServerCallContext context)
        {
            var senderRequest = new DeleteChangesListCommand()
            {
                Id = Guid.Parse(request.ListId)
            };

            var result = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return result;
        }

        public override async Task<SucceedResponse> CreateScheduleChangesListItem(ChangesListItem request, ServerCallContext context)
        {
            var senderRequest = new CreateChangesListItemCommand()
            {
                ListId = Guid.Parse(request.ChangesListId),
                GroupId = Guid.Parse(request.GroupId),
                Position = request.Position,
                Discipline = request.SubjectName,
                Auditorium = request.Auditorium,
                Teacher = request.TeacherInitials
            };

            var result = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return result;
        }

        public override async Task<SucceedResponse> UpdateScheduleChangesListItem(ChangesListItem request, ServerCallContext context)
        {
            var senderRequest = new UpdateChangesListItemCommand()
            {
                Id = Guid.Parse(request.Id),
                GroupId = Guid.Parse(request.GroupId),
                Position = request.Position,
                Discipline = request.SubjectName,
                Auditorium = request.Auditorium,
                Teacher = request.TeacherInitials
            };

            var result = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return result;
        }

        public override async Task<SucceedResponse> DeleteScheduleChangesListItem(DeleteScheduleChangesItemRequest request, ServerCallContext context)
        {
            var senderRequest = new DeleteChangesListItemCommand(Guid.Parse(request.Id));

            var result = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return result;
        }
    }
}
