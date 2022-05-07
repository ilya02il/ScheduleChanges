using Domain.Dtos;
using System;
using System.Threading.Tasks;

namespace Domain.Events
{
    public delegate Task<DatedScheduleDto> GetDatedScheduleEventHandler(object sender, GetDatedScheduleEventArgs e);

    public class GetDatedScheduleEventArgs
    {
        public string EducOrgName { get; }
        public string GroupNumber { get; }
        public DateTimeOffset Date { get; }

        public GetDatedScheduleEventArgs(string educOrgName, string groupNumber, DateTimeOffset date)
        {
            EducOrgName = educOrgName;
            GroupNumber = groupNumber;
            Date = date;
        }
    }
}
