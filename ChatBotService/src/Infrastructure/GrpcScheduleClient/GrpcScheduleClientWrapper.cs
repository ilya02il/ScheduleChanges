using Application.Common.Interfaces;
using AutoMapper;
using Domain.Dtos;
using Domain.ValueObjects;
using Google.Protobuf.WellKnownTypes;
using ScheduleService;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.GrpcScheduleClient
{
    public class GrpcScheduleClientWrapper : IGrpcScheduleClientWrapper
    {
        private readonly GrpcSchedule.GrpcScheduleClient _grpcClient;
        private readonly IMapper _mapper;

        public GrpcScheduleClientWrapper(GrpcSchedule.GrpcScheduleClient grpcClient,
            IMapper mapper)
        {
            _grpcClient = grpcClient;
            _mapper = mapper;
        }

        public async Task<DatedScheduleDto> GetDatedSchedule(EducationalInfo educationalInfo, DateTimeOffset date)
        {
            var grpcRequest = new GetDatedScheduleRequest()
            {
                Date = date.ToTimestamp(),
                EducOrgName = educationalInfo.EducOrgName,
                GroupNumber = educationalInfo.GroupNumber
            };

            var requestResult = await _grpcClient.GetDatedScheduleAsync(grpcRequest);

            return _mapper.Map<DatedScheduleDto>(requestResult);
        }

        public async Task<IList<string>> GetEducOrgsList()
        {
            var grpcRequest = new GetEducOrgsListRequest();

            var response = await _grpcClient.GetEducOrgsListAsync(grpcRequest);

            return response.EducOrgNamesList;
        }

        public async Task<IList<string>> GetGroupNumbersList(string educOrgName, int yearOfStudy)
        {
            var grpcRequest = new GetGroupNumbersListRequest()
            {
                EducOrgName = educOrgName,
                YearOfStudy = yearOfStudy
            };

            var response = await _grpcClient.GetGroupNumbersListAsync(grpcRequest);

            return response.GroupNumbersList;
        }
    }
}
