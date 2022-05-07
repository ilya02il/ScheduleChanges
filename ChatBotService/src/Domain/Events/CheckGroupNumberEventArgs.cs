using System.Threading.Tasks;

namespace Domain.Events
{
    public delegate Task<bool> CheckGroupNumberEventHandler(object sender, CheckGroupNumberEventArgs e);

    public class CheckGroupNumberEventArgs
    {
        public string EducOrgName { get; }
        public string GroupNumber { get; }

        public CheckGroupNumberEventArgs(string educOrgName, string groupNumber)
        {
            EducOrgName = educOrgName;
            GroupNumber = groupNumber;
        }
    }
}
