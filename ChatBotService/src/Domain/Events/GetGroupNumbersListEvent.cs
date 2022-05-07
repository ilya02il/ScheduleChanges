using Domain.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Events
{
    public delegate Task<IList<string>> GetGroupNumbersListEventHandler(object source, GetGroupNumbersListEventArgs args);

    public class GetGroupNumbersListEventArgs
    {
        public string EducOrgName { get; init; }
        public int YearOfStudy { get; init; }


        public GetGroupNumbersListEventArgs(string educOrgName, int yearOfStudy)
        {
            EducOrgName = educOrgName;
            YearOfStudy = yearOfStudy;
        }
    }
}
