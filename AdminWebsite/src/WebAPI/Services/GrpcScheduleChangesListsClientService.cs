using AutoMapper;
using ChangesLists.Messages;
using ChangesLists.Service;
using Google.Protobuf;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebAPI.Dtos.ChangesLists;

namespace WebAPI.Services
{
    public class GrpcScheduleChangesListsClientService
    {
        private readonly GrpcScheduleChangesLists.GrpcScheduleChangesListsClient _grpcClient;
        private readonly IMapper _mapper;

        public GrpcScheduleChangesListsClientService(GrpcScheduleChangesLists.GrpcScheduleChangesListsClient grpcClient,
            IMapper mapper)
        {
            _grpcClient = grpcClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BriefChangesListDto>> GetBriefScheduleChangesListByEducOrgId(Guid educOrgId,
            CancellationToken cancellationToken)
        {
            var request = new GetByEducOrgIdRequest()
            {
                EducOrgId = educOrgId.ToString()
            };

            var requestResult = await _grpcClient.GetBriefChangesListCollectionAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<IEnumerable<BriefChangesListDto>>(requestResult.Lists);
        }

        public async Task<ChangesListDto> GetScheduleChangesListById(Guid id,
            CancellationToken cancellationToken)
        {
            var request = new GetByIdRequest()
            {
                Id = id.ToString()
            };

            var requestResult = await _grpcClient.GetScheduleChangesListByIdAsync(request,
                cancellationToken: cancellationToken);

            return _mapper.Map<ChangesListDto>(requestResult);
        }

        public async Task<bool> CreateScheduleChangesList(Guid educOrgId, CreateChangesListDto changesList,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<CreateScheduleChangesListRequest>(changesList);
            request.EducOrgId = educOrgId.ToString();

            var result = await _grpcClient.CreateScheduleChangesListAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> CreateScheduleChangesListFromFile(Guid educOrgId,
            DateTime date,
            bool isOddWeek,
            Stream fileStream,
            CancellationToken cancellationToken)
        {
            var metadataRequest = new CreateScheduleChangesListFromFileRequest()
            {
                Metadata = new ScheduleSchangesListMetadata()
                {
                    EducOrgId = educOrgId.ToString(),
                    Date = date.ToUniversalTime().ToTimestamp(),
                    IsOddWeek = isOddWeek
                }
            };

            var fileDataRequest = new CreateScheduleChangesListFromFileRequest();

            using var call = _grpcClient.CreateScheduleChangesListFromFile(cancellationToken: cancellationToken);
            await call.RequestStream.WriteAsync(metadataRequest, cancellationToken);

            byte[] buffer = new byte[10L * 1024L * 1024L];//10 mb buffer
            int idx = 0;

            do
            {
                while (idx < buffer.Length)
                {
                    int offset = await fileStream.ReadAsync(buffer.AsMemory(idx, buffer.Length - idx), cancellationToken);

                    if (offset == 0)
                        break;

                    idx += offset;
                }

                if (idx != 0)
                {
                    fileDataRequest.FileData = ByteString.CopyFrom(buffer);
                    await call.RequestStream.WriteAsync(fileDataRequest, cancellationToken);
                }

            } while (idx != buffer.Length);

            await call.RequestStream.CompleteAsync();
            var result = await call.ResponseAsync;

            return result.IsSucceed;
        }

        public async Task<bool> UpdateScheduleChangesList(Guid id,
            UpdateChangesListInfoDto changesListInfo,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<UpdateScheduleChangesListRequest>(changesListInfo);
            request.Id = id.ToString();

            var result = await _grpcClient.UpdateScheduleChangesListAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> DeleteScheduleChangesList(Guid id,
            CancellationToken cancellationToken)
        {
            var request = new DeleteScheduleChangesListRequest()
            {
                ListId = id.ToString()
            };

            var result = await _grpcClient.DeleteScheduleChangesListAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> CreateScheduleChangesListItem(Guid listId,
            CreateChangesListItemDto changesListItem,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ChangesListItem>(changesListItem);
            request.ChangesListId = listId.ToString();

            var result = await _grpcClient.CreateScheduleChangesListItemAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> UpdateScheduleChangesListItem(Guid id,
            UpdateChangesListItemInfoDto changesListItemInfo,
            CancellationToken cancellationToken)
        {
            var request = _mapper.Map<ChangesListItem>(changesListItemInfo);
            request.Id = id.ToString();

            var result = await _grpcClient.UpdateScheduleChangesListItemAsync(request,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }

        public async Task<bool> DeleteScheduleChangesListItem(Guid id, CancellationToken cancellationToken)
        {
            var reqest = new DeleteScheduleChangesItemRequest
            {
                Id = id.ToString()
            };

            var result = await _grpcClient.DeleteScheduleChangesListItemAsync(reqest,
                cancellationToken: cancellationToken);

            return result.IsSucceed;
        }
    }
}
