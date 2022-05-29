using Application.ScheduleLists.Commands;
using Application.ScheduleLists.Dtos;
using Application.ScheduleLists.Queries;
using AutoMapper;
using Grpc.Core;
using MediatR;
using ScheduleLists.Messages;
using ScheduleLists.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcAPI.Services
{
    public class ScheduleListsService : GrpcScheduleLists.GrpcScheduleListsBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public ScheduleListsService(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;

        }

        public async override Task<GetScheduleListsByGroupIdResponse> GetScheduleListsByGroupId(GetScheduleListsByGroupIdRequest request,
            ServerCallContext context)
        {
            var senderRequest = new GetScheduleListsByGroupIdQuery(Guid.Parse(request.GroupId));
            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            var response = new GetScheduleListsByGroupIdResponse();
            response.Lists
                .AddRange(senderResponse.Select(item => _mapper.Map<ScheduleList>(item)));

            return response;
        }

        public async override Task<ScheduleList> GetScheduleListById(GetScheduleListByIdRequest request,
            ServerCallContext context)
        {
            var senderRequest = new GetScheduleListByIdQuery(Guid.Parse(request.Id));
            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            return _mapper.Map<ScheduleList>(senderResponse);
        }

        public async override Task<SucceedResponse> CreateScheduleList(CreateScheduleListRequest request,
            ServerCallContext context)
        {
            var senderRequest = new CreateScheduleListCommand(Guid.Parse(request.GroupId),
                (System.DayOfWeek)request.DayOfWeek);

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }

        public async override Task<SucceedResponse> UpdateScheduleList(ScheduleList request,
            ServerCallContext context)
        {
            var listItems = _mapper.Map<IEnumerable<ScheduleListItemDto>>(request.ListItems);

            var senderRequest = new UpdateScheduleListCommand()
            {
                Id = Guid.Parse(request.Id),
                DayOfWeek = (System.DayOfWeek)request.DayOfWeek,
                ListItems = new List<ScheduleListItemDto>(listItems)
            };

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }

        public async override Task<SucceedResponse> DeleteScheduleList(DeleteScheduleListRequest request,
            ServerCallContext context)
        {
            var senderRequest = new DeleteScheduleListCommand(Guid.Parse(request.Id));

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }
    }
}
