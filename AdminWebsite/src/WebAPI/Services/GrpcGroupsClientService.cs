using AutoMapper;
using Groups.Service;
using Groups.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.Groups;
using System.Collections.Generic;

namespace WebAPI.Services
{
    public class GrpcGroupsClientService
    {
        private readonly GrpcGroupsLists.GrpcGroupsListsClient _grpcClient;
        private readonly IMapper _mapper;

        public GrpcGroupsClientService(GrpcGroupsLists.GrpcGroupsListsClient grpcClient,
            IMapper mapper)
        {
            _grpcClient = grpcClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GroupDto>> GetGroupListByEducOrgId(Guid educOrgId,
            CancellationToken cancellationToken)
        {
            var request = new GetByEducOrgIdRequest()
            {
                EducOrgId = educOrgId.ToString()
            };

            var requestResult = await _grpcClient.GetGroupsListByEducOrgIdAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<GroupDto>>(requestResult.Groups);
        }

        public async Task<IEnumerable<GroupBriefDto>> GetBriefGroupsByEducOrgId(Guid educOrgId,
            CancellationToken cancellationToken)
        {
            var request = new GetByEducOrgIdRequest()
            {
                EducOrgId = educOrgId.ToString()
            };

            var requestResult = await _grpcClient.GetBriefGroupsByEducOrgIdAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<GroupBriefDto>>(requestResult.BriefGroups);
        }

        public async Task<bool> CreateGroup(Guid educOrgId,
            CreateGroupDto group,
            CancellationToken cancellationToken)
        {
            var request = new CreateGroupRequest()
            {
                EducOrgId = educOrgId.ToString(),
                GroupNumber = group.GroupNumber,
                YearOfStudy = group.YearOfStudy
            };

            var result = await _grpcClient.CreateGroupAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> UpdateGroupInfo(Guid id,
            GroupInfoDto groupInfo,
            CancellationToken cancellationToken)
        {
            var request = new UpdateGroupInfoRequest()
            {
                Id = id.ToString(),
                GroupNumber = groupInfo.GroupNumber,
                YearOfStudy = groupInfo.YearOfStudy
            };

            var result = await _grpcClient.UpdateGroupAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> DeleteGroup(Guid id,
            CancellationToken cancellationToken)
        {
            var request = new DeleteGroupRequest()
            {
                Id = id.ToString()
            };

            var result = await _grpcClient.DeleteGroupAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }
    }
}
