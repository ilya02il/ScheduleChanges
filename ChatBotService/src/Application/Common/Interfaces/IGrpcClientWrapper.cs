using Domain.Dtos;
using Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    public interface IGrpcScheduleClientWrapper
    {
        Task<DatedScheduleDto> GetDatedSchedule(EducationalInfo educationalInfo, DateTimeOffset date);
        Task<IList<string>> GetEducOrgsList();
        Task<IList<string>> GetGroupNumbersList(string educOrgName, int yearOfStudy);
    }
}
