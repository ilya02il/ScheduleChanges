using System;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.CallScheduleLists;
using CallSchedules.Service;
using CallSchedules.Messages;
using AutoMapper;

namespace WebAPI.Services
{
    public class GrpcCallScheduleListsClientService
    {
        private readonly GrpcCallScheduleService.GrpcCallScheduleServiceClient _grpcClient;
        private readonly IMapper _mapper;

        public GrpcCallScheduleListsClientService(GrpcCallScheduleService.GrpcCallScheduleServiceClient grpcClient,
            IMapper mapper)
        {
            _grpcClient = grpcClient;
            _mapper = mapper;
        }

        public async Task<CallScheduleListDto> GetCallScheduleList(Guid educOrgId,
            System.DayOfWeek dayOfWeek,
            CancellationToken cancellationToken)
        {
            var request = new GetCallScheduleListRequest()
            {
                EducOrgId = educOrgId.ToString(),
                DayOfWeek = (CallSchedules.Messages.DayOfWeek)dayOfWeek
            };

            var requestResult = await _grpcClient.GetCallScheduleListAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<CallScheduleListDto>(requestResult);
        }

        public async Task<bool> CreateCallScheduleListItem(Guid educOrgId,
            CreateCallScheduleListItemDto listItem,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<CreateCallScheduleListItemRequest>(listItem);
            request.EducOrgId = educOrgId.ToString();

            var requestResult = await _grpcClient.CreateCallScheduleListItemAsync(request,
                cancellationToken: cancellationToken);

            return requestResult.IsSucceed;
        }

        public async Task<bool> UpdateCallScheduleListItem(Guid id,
            UpdateCallScheduleListItemDto listItem,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<UpdateCallScheduleListItemRequest>(listItem);
            request.Id = id.ToString();

            var requestResult = await _grpcClient.UpdateCallScheduleListItemAsync(request,
                cancellationToken: cancellationToken);

            return requestResult.IsSucceed;
        }

        public async Task<bool> DeleteCallScheduleListItem(Guid id,
            CancellationToken cancellationToken)
        {
            var request = new DeleteCallScheduleListItemRequest()
            {
                Id = id.ToString()
            };

            var requestResult = await _grpcClient.DeleteCallScheduleListItemAsync(request,
                cancellationToken: cancellationToken);

            return requestResult.IsSucceed;
        }
    }
}
