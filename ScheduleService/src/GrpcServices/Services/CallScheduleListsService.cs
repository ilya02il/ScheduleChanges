using Application.CallSchedules.Commands;
using Application.CallSchedules.Queries;
using AutoMapper;
using CallSchedules.Messages;
using CallSchedules.Service;
using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcAPI.Services
{
    public class CallScheduleListsService : GrpcCallScheduleService.GrpcCallScheduleServiceBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public CallScheduleListsService(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }

        public override async Task<GetCallScheduleListResponse> GetCallScheduleList(GetCallScheduleListRequest request,
            ServerCallContext context)
        {
            var senderRequest = new GetCallScheduleListQuery(Guid.Parse(request.EducOrgId),
                (System.DayOfWeek)request.DayOfWeek);

            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            var response = new GetCallScheduleListResponse()
            {
                DayOfWeek = request.DayOfWeek
            };

            response.ListItems.AddRange(_mapper.Map<IEnumerable<CallScheduleListItem>>(senderResponse.ListItems));

            return response;
        }

        public override async Task<SucceedResponse> CreateCallScheduleListItem(CreateCallScheduleListItemRequest request,
            ServerCallContext context)
        {
            var senderRequest = new CreateCallScheduleListItemCommand()
            {
                EducOrgId = Guid.Parse(request.EducOrgId),
                DayOfWeek = (System.DayOfWeek)request.DayOfWeek,
                Position = request.Position,
                StartTime = request.StartTime.ToTimeSpan(),
                EndTime = request.EndTime.ToTimeSpan()
            };

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }

        public override async Task<SucceedResponse> UpdateCallScheduleListItem(UpdateCallScheduleListItemRequest request,
            ServerCallContext context)
        {
            var senderRequest = new UpdateCallScheduleListItemCommand()
            {
                Id = Guid.Parse(request.Id),
                Position = request.Position,
                StartTime = request.StartTime.ToTimeSpan(),
                EndTime = request.EndTime.ToTimeSpan()
            };

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }

        public override async Task<SucceedResponse> DeleteCallScheduleListItem(DeleteCallScheduleListItemRequest request,
            ServerCallContext context)
        {
            var senderRequest = new DeleteCallScheduleListItemCommand()
            {
                Id = Guid.Parse(request.Id)
            };

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }
    }
}
