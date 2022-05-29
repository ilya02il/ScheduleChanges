using Application.Groups.Commands;
using Application.Groups.Queries;
using AutoMapper;
using Google.Protobuf.Collections;
using Groups.Messages;
using Groups.Service;
using Grpc.Core;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcAPI.Services
{
    public class GroupsService : GrpcGroupsLists.GrpcGroupsListsBase
    {
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public GroupsService(ISender sender, IMapper mapper)
        {
            _sender = sender;
            _mapper = mapper;
        }


        public async override Task<GetGroupsListByEducOrgIdResponse> GetGroupsListByEducOrgId(GetByEducOrgIdRequest request, ServerCallContext context)
        {
            var senderRequest = new GetGroupsByEducOrgIdQuery(Guid.Parse(request.EducOrgId));
            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            var response = new GetGroupsListByEducOrgIdResponse();

            response.Groups
                .AddRange(_mapper.Map<IEnumerable<Group>>(senderResponse));

            return response;
        }

        public async override Task<GetBriefGroupsByEducOrgIdRespose> GetBriefGroupsByEducOrgId(GetByEducOrgIdRequest request, ServerCallContext context)
        {
            var senderRequest = new GetBriefGroupsByEducOrgIdQuery(Guid.Parse(request.EducOrgId));
            var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

            var response = new GetBriefGroupsByEducOrgIdRespose();

            response.BriefGroups
                .AddRange(_mapper.Map<IEnumerable<GroupBrief>>(senderResponse));

            return response;
        }

        public async override Task<SucceedResponse> CreateGroup(CreateGroupRequest request, ServerCallContext context)
        {
            var senderRequest = new CreateGroupCommand()
            {
                EducOrgId = Guid.Parse(request.EducOrgId),
                GroupNumber = request.GroupNumber,
                YearOfStudy = request.YearOfStudy
            };

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }

        public async override Task<SucceedResponse> UpdateGroup(UpdateGroupInfoRequest request, ServerCallContext context)
        {
            var senderRequest = new UpdateGroupCommand()
            {
                Id = Guid.Parse(request.Id),
                GroupNumber = request.GroupNumber,
                YearOfStudy = request.YearOfStudy
            };

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }

        public override async Task<SucceedResponse> DeleteGroup(DeleteGroupRequest request, ServerCallContext context)
        {
            var senderRequest = new DeleteGroupCommand(Guid.Parse(request.Id));

            var response = new SucceedResponse()
            {
                IsSucceed = await _sender.Send(senderRequest, context.CancellationToken)
            };

            return response;
        }
    }
}
