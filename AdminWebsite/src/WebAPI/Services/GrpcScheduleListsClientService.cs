using AutoMapper;
using ScheduleLists.Messages;
using ScheduleLists.Service;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.ScheduleLists;

namespace WebAPI.Services
{
    public class GrpcScheduleListsClientService
    {
        private readonly GrpcScheduleLists.GrpcScheduleListsClient _grpcClient;
        private readonly IMapper _mapper;

        public GrpcScheduleListsClientService(GrpcScheduleLists.GrpcScheduleListsClient grpcClient,
            IMapper mapper)
        {
            _grpcClient = grpcClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ScheduleListDto>> GetScheduleListsByGroupId(Guid groupId,
            CancellationToken cancellationToken)
        {
            var request = new GetScheduleListsByGroupIdRequest()
            {
                GroupId = groupId.ToString()
            };

            var requestResult = await _grpcClient.GetScheduleListsByGroupIdAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<ScheduleListDto>>(requestResult.Lists);
        }

        public async Task<ScheduleListDto> GetScheduleListById(Guid id,
            CancellationToken cancellationToken)
        {
            var request = new GetScheduleListByIdRequest()
            {
                Id = id.ToString()
            };

            var requestResult = await _grpcClient.GetScheduleListByIdAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<ScheduleListDto>(requestResult);
        }

        public async Task<bool> CreateScheduleList(Guid groupId,
            CreateScheduleListDto listItem,
            CancellationToken cancellationToken)
        {
            try
            {
                var request = new CreateScheduleListRequest
                {
                    GroupId = groupId.ToString(),
                    DayOfWeek = (ScheduleLists.Messages.DayOfWeek)listItem.DayOfWeek
                };

                var requestResult = await _grpcClient.CreateScheduleListAsync(request,
                    cancellationToken: cancellationToken);

                return requestResult.IsSucceed;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateScheduleList(Guid id,
            UpdateScheduleListDto listItem,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ScheduleList>(listItem);
            request.Id = id.ToString();

            var requestResult = await _grpcClient.UpdateScheduleListAsync(request,
                cancellationToken: cancellationToken);

            return requestResult.IsSucceed;
        }

        public async Task<bool> DeleteScheduleList(Guid id,
            CancellationToken cancellationToken)
        {
            try
            {
                var request = new DeleteScheduleListRequest()
                {
                    Id = id.ToString()
                };

                var requestResult = await _grpcClient.DeleteScheduleListAsync(request,
                    cancellationToken: cancellationToken);

                return requestResult.IsSucceed;
            }
            catch
            {
                return false;
            }
        }
    }
}
