using Application.ScheduleWithChangesList.Queries.GetEducOrgsListQuery;
using Application.ScheduleWithChangesList.Queries.GetGroupNumbersListQuery;
using Application.ScheduleWithChangesList.Queries.GetScheduleWithChangesListQuery;
using AutoMapper;
using Grpc.Core;
using MediatR;
using DatedSchedules.Messages;
using DatedSchedules.Service;
using System.Threading.Tasks;

namespace ServiceAPI.GrpcServices;

public class DatedSchedulesService : GrpcSchedule.GrpcScheduleBase
{
    private readonly ISender _sender;
    private readonly IMapper _mapper;

    public DatedSchedulesService(ISender sender, IMapper mapper)
    {
        _sender = sender;
        _mapper = mapper;
    }

    public async override Task<GetDatedScheduleResponse> GetDatedSchedule(GetDatedScheduleRequest request, ServerCallContext context)
    {
        var senderRequest = new GetScheduleWithChangesListQuery()
        {
            Date = request.Date.ToDateTimeOffset(),
            EducOrgName = request.EducOrgName,
            GroupNumber = request.GroupNumber
        };

        var senderResponse = await _sender.Send(senderRequest, context.CancellationToken);

        return _mapper.Map<GetDatedScheduleResponse>(senderResponse);
    }

    public async override Task<GetEducOrgsListResponse> GetEducOrgsList(GetEducOrgsListRequest request, ServerCallContext context)
    {
        var senderResponse = await _sender.Send(new GetEducOrgsListQuery(), context.CancellationToken);

        var response = new GetEducOrgsListResponse();
        response.EducOrgNamesList.AddRange(senderResponse);

        return response;
    }

    public async override Task<GetGroupNumbersListResponse> GetGroupNumbersList(GetGroupNumbersListRequest request, ServerCallContext context)
    {
        var senderResponse = await _sender.Send(new GetGroupNumbersListQuery(request.EducOrgName, request.YearOfStudy),
            context.CancellationToken);

        var response = new GetGroupNumbersListResponse();
        response.GroupNumbersList.AddRange(senderResponse);

        return response;
    }
}