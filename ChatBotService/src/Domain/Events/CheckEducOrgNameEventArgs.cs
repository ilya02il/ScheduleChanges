using System.Threading.Tasks;

namespace Domain.Events
{
    public delegate Task<bool> CheckEducOrgNameEventHandler(object sender, CheckEducOrgNameEventArgs e);

    public class CheckEducOrgNameEventArgs
    {
        public string EducOrgName { get; }

        public CheckEducOrgNameEventArgs(string educOrgName)
        {
            EducOrgName = educOrgName;
        }
    }
}
